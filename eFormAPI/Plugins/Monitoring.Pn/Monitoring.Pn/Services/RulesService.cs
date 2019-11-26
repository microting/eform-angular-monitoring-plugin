using System.Collections.Generic;
using Microting.eForm.Dto;
using Microting.eFormApi.BasePn.Abstractions;

namespace Monitoring.Pn.Services
{
    using System;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using Abstractions;
    using Helpers;
    using Infrastructure.Models;
    using Microsoft.AspNetCore.Http;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;
    using Microting.eForm.Infrastructure.Constants;
    using Microting.eFormApi.BasePn.Infrastructure.Models.API;
    using Microting.EformMonitoringBase.Infrastructure.Data;
    using Microting.EformMonitoringBase.Infrastructure.Data.Entities;
    using Microting.EformMonitoringBase.Infrastructure.Enums;
    using Microting.EformMonitoringBase.Infrastructure.Models;
    using Microting.EformMonitoringBase.Infrastructure.Models.Blocks;
    using Newtonsoft.Json;

    public class RulesService : IRulesService
    {
        private readonly IEFormCoreService _coreHelper;
        private readonly ILogger<RulesService> _logger;
        private readonly EformMonitoringPnDbContext _dbContext;
        private readonly IMonitoringLocalizationService _localizationService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public RulesService(
            EformMonitoringPnDbContext dbContext,
            IEFormCoreService coreHelper,
            IMonitoringLocalizationService localizationService,
            IHttpContextAccessor httpContextAccessor,
            ILogger<RulesService> logger)
        {
            _dbContext = dbContext;
            _coreHelper = coreHelper;
            _localizationService = localizationService;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }
        
        public async Task<OperationDataResult<NotificationRulesListModel>> GetRules(NotificationListRequestModel requestModel)
        {
            var core = await _coreHelper.GetCore();
            List<Template_Dto> eForms = new List<Template_Dto>();
            try
            {
                var rules = await _dbContext.Rules
                    .AsNoTracking()
                    .Where(x => x.WorkflowState != Constants.WorkflowStates.Removed)
                    .Skip(requestModel.Offset)
                    .Take(requestModel.PageSize)
                    .Include(x => x.Recipients)
                    .ToListAsync();

                var result = new NotificationRulesListModel();
                foreach (var rule in rules)
                {
                    string eFormName = "";
                    if (eForms.Any(x => x.Id == rule.CheckListId))
                    {
                        eFormName = eForms.First(x => x.Id == rule.CheckListId).Label;
                    }
                    else
                    {
                        eForms.Add(await core.TemplateItemRead(rule.CheckListId));
                        eFormName = eForms.First(x => x.Id == rule.CheckListId).Label;
                    }
                    
                    var ruleModel = new NotificationRuleSimpleModel
                    {
                        Id = rule.Id,
                        EFormName = eFormName,
                        Trigger = RulesBlockHelper.GetRuleTriggerString(rule),
                        Event = "Email"
                    };

                    result.Rules.Add(ruleModel);
                }

                result.Total = await _dbContext.Rules.CountAsync(x =>
                    x.WorkflowState != Constants.WorkflowStates.Removed);

                return new OperationDataResult<NotificationRulesListModel>(true, result);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return new OperationDataResult<NotificationRulesListModel>(
                    false,
                    _localizationService.GetString("ErrorWhileObtainingNotificationRulesInfo"));
            }
        }

        public async Task<OperationResult> CreateNewRule(NotificationRuleModel ruleModel)
        {
            using (var transaction = await _dbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    var notificationRule = new NotificationRule()
                    {
                        Subject = ruleModel.Subject,
                        Text = ruleModel.Text,
                        AttachReport = ruleModel.AttachReport,
                        DataItemId = ruleModel.DataItemId,
                        Data = ruleModel.Data.ToString(),
                        CheckListId = ruleModel.CheckListId,
                        RuleType = ruleModel.RuleType,
                        CreatedByUserId = UserId,
                        UpdatedByUserId = UserId,
                    };

                    await notificationRule.Save(_dbContext);

                    foreach (var recipientModel in ruleModel.Recipients)
                    {
                        var recipient = new Recipient()
                        {
                            CreatedByUserId = UserId,
                            UpdatedByUserId = UserId,
                            Email = recipientModel.Email,
                            NotificationRuleId = notificationRule.Id,
                        };
                        await recipient.Save(_dbContext);
                    }

                    transaction.Commit();

                    return new OperationResult(
                        true,
                        _localizationService.GetString("NotificationRuleCreatedSuccessfully"));
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    _logger.LogError(e.Message);
                    return new OperationResult(
                        false,
                        _localizationService.GetString("ErrorWhileCreatingNotificationRule"));
                }
            }
        }

        public async Task<OperationDataResult<NotificationRuleModel>> GetRuleById(int id)
        {
            try
            {
                var rule = await _dbContext.Rules
                    .FirstOrDefaultAsync(
                        x => x.Id == id
                             && x.WorkflowState != Constants.WorkflowStates.Removed);

                if (rule == null)
                {
                    return new OperationDataResult<NotificationRuleModel>(false,
                        _localizationService.GetString("NotificationRuleNotFound"));
                }

                var recipients = await _dbContext.Recipients
                    .Where(x => x.NotificationRuleId == rule.Id
                                && x.WorkflowState != Constants.WorkflowStates.Removed)
                    .Select(x => new RecipientModel()
                    {
                        Id = x.Id,
                        Email = x.Email,
                    }).ToListAsync();

                var ruleModel = new NotificationRuleModel()
                {
                    Id = rule.Id,
                    CheckListId = rule.CheckListId,
                    DataItemId = rule.DataItemId,
                    RuleType = rule.RuleType,
                    AttachReport = rule.AttachReport,
                    Subject = rule.Subject,
                    Text = rule.Text,
                    Recipients = recipients
                };

                var jsonSettings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Include
                };

                switch (rule.RuleType)
                {
                    case RuleType.Select:
                        ruleModel.Data = JsonConvert.DeserializeObject<SelectBlock>(rule.Data, jsonSettings);
                        break;
                    case RuleType.CheckBox:
                        ruleModel.Data = JsonConvert.DeserializeObject<CheckBoxBlock>(rule.Data, jsonSettings);
                        break;
                    case RuleType.Number:
                        ruleModel.Data = JsonConvert.DeserializeObject<NumberBlock>(rule.Data, jsonSettings);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                return new OperationDataResult<NotificationRuleModel>(true, ruleModel);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return new OperationDataResult<NotificationRuleModel>(
                    false,
                    _localizationService.GetString("ErrorWhileObtainingNotificationRulesInfo"));
            }
        }

        public async Task<OperationResult> UpdateRule(NotificationRuleModel ruleModel)
        {
            using (var transaction = await _dbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    var rule = await _dbContext.Rules
                        .Include(x=>x.Recipients)
                        .FirstOrDefaultAsync(x => x.WorkflowState != Constants.WorkflowStates.Removed
                                                  && x.Id == ruleModel.Id);

                    if (rule == null)
                    {
                        return new OperationResult(false,
                            _localizationService.GetString("NotificationRuleNotFound"));
                    }

                    rule.AttachReport = ruleModel.AttachReport;
                    rule.Data = ruleModel.Data.ToString();
                    rule.RuleType = ruleModel.RuleType;
                    rule.Subject = ruleModel.Subject;
                    rule.CheckListId = ruleModel.CheckListId;
                    rule.Text = ruleModel.Text;
                    rule.DataItemId = ruleModel.DataItemId;

                    await rule.Update(_dbContext);

                    var recipientsDelete = await _dbContext.Recipients
                        .Where(r => r.NotificationRuleId == rule.Id && ruleModel.Recipients.All(rm => rm.Id != r.Id))
                        .ToListAsync();

                    foreach (var rd in recipientsDelete)
                    {
                        await rd.Delete(_dbContext);
                    }
                    
                    foreach (var recipientModel in ruleModel.Recipients.Where(r => r.Id == null))
                    {
                        var recipient = new Recipient
                        {
                            Email = recipientModel.Email,
                            NotificationRuleId = rule.Id,
                            CreatedByUserId = UserId,
                            UpdatedByUserId = UserId
                        };

                        await recipient.Save(_dbContext);
                    }

                    transaction.Commit();
                    return new OperationResult(
                        true, 
                        _localizationService.GetString("NotificationRuleHasBeenUpdated"));
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    _logger.LogError(e.Message);
                    return new OperationResult(
                        false,
                        _localizationService.GetString("ErrorWhileUpdatingNotificationRule"));
                }
            }
        }

        public async Task<OperationResult> DeleteRule(int id)
        {
            using (var transaction = await _dbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    var notificationRule = await _dbContext.Rules
                        .FirstOrDefaultAsync(x => x.Id == id);

                    if (notificationRule == null)
                    {
                        return new OperationResult(
                            false,
                            _localizationService.GetString("NotificationRuleNotFound"));
                    }

                    var recipients = await _dbContext.Recipients
                        .Where(x => x.NotificationRuleId == notificationRule.Id)
                        .ToListAsync();

                    foreach (var recipient in recipients)
                    {
                        await recipient.Delete(_dbContext);
                    }

                    await notificationRule.Delete(_dbContext);
                    transaction.Commit();
                    return new OperationResult(
                        true, 
                        _localizationService.GetString("NotificationRuleDeletedSuccessfully"));
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    _logger.LogError(e.Message);
                    return new OperationResult(false, _localizationService.GetString("ErrorWhileRemovingNotificationRule"));
                }
            }
        }

        private int UserId
        {
            get
            {
                var value = _httpContextAccessor?.HttpContext.User?.FindFirstValue(ClaimTypes.NameIdentifier);
                return value == null ? 0 : int.Parse(value);
            }
        }
    }
}
