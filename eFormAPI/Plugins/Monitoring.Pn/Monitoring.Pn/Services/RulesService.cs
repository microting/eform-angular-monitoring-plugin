namespace Monitoring.Pn.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using Abstractions;
    using Helpers;
    using Infrastructure.Models;
    using Microsoft.AspNetCore.Http;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;
    using Microting.eForm.Dto;
    using Microting.eForm.Infrastructure.Constants;
    using Microting.eForm.Infrastructure.Models;
    using Microting.eFormApi.BasePn.Infrastructure.Models.API;
    using Microting.EformMonitoringBase.Infrastructure.Data;
    using Microting.EformMonitoringBase.Infrastructure.Data.Entities;
    using Microting.EformMonitoringBase.Infrastructure.Enums;
    using Microting.EformMonitoringBase.Infrastructure.Models;
    using Microting.EformMonitoringBase.Infrastructure.Models.Blocks;
    using Newtonsoft.Json;

    public class RulesService : IRulesService
    {
        private readonly ILogger<RulesService> _logger;
        private readonly EformMonitoringPnDbContext _dbContext;
        private readonly IMonitoringLocalizationService _localizationService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public RulesService(
            EformMonitoringPnDbContext dbContext,
            IMonitoringLocalizationService localizationService,
            IHttpContextAccessor httpContextAccessor,
            ILogger<RulesService> logger)
        {
            _dbContext = dbContext;
            _localizationService = localizationService;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
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
                    rule.TemplateId = ruleModel.TemplateId;
                    rule.Text = ruleModel.Text;
                    rule.DataItemId = ruleModel.DataItemId;

                    await rule.Update(_dbContext);

                    // TODO update recipients

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

        public async Task<OperationResult> CreateNewRule(NotificationRuleCreateModel requestModel)
        {
            using (var transaction = await _dbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    var notificationRule = new NotificationRule()
                    {
                        Subject = requestModel.Subject,
                        Text = requestModel.Text,
                        AttachReport = requestModel.AttachReport,
                        DataItemId = requestModel.DataItemId,
                        Data = requestModel.Data.ToString(),
                        TemplateId = requestModel.TemplateId,
                        RuleType = requestModel.RuleType,
                        CreatedByUserId = UserId,
                        UpdatedByUserId = UserId,
                    };

                    await notificationRule.Save(_dbContext);

                    foreach (var recipientModel in requestModel.Recipients)
                    {
                        var recipient = new Recipient()
                        {
                            CreatedByUserId = UserId,
                            UpdatedByUserId = UserId,
                            Email = recipientModel,
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
                    Recipients = recipients,
                    AttachReport = rule.AttachReport,
                    RuleType = rule.RuleType,
                    Subject = rule.Subject,
                    TemplateId = rule.TemplateId,
                    Text = rule.Text,
                };

                switch (rule.RuleType)
                {
                    case RuleType.Select:
                        ruleModel.Data = JsonConvert.DeserializeObject<SelectBlock>(rule.Data);
                        break;
                    case RuleType.CheckBox:
                        ruleModel.Data = JsonConvert.DeserializeObject<CheckBoxBlock>(rule.Data);
                        break;
                    case RuleType.Number:
                        ruleModel.Data = JsonConvert.DeserializeObject<NumberBlock>(rule.Data);
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


        public async Task<OperationDataResult<NotificationRuleListsModel>> GetRules(NotificationListRequestModel requestModel)
        {
            try
            {
                var rules = await _dbContext.Rules
                    .AsNoTracking()
                    .Where(x => x.WorkflowState != Constants.WorkflowStates.Removed)
                    .Skip(requestModel.Offset)
                    .Take(requestModel.PageSize)
                    .ToListAsync();

                var result = new NotificationRuleListsModel();
                foreach (var rule in rules)
                {
                    var ruleModel = new NotificationRuleModel
                    {
                        Id = rule.Id,
                        AttachReport = rule.AttachReport,
                        DataItemId = rule.DataItemId,
                        RuleType = rule.RuleType,
                        Subject = rule.Subject,
                        TemplateId = rule.TemplateId,
                        Text = rule.Text,
                        Data = RulesBlockHelper.GetRuleTriggerString(rule),
                    };

                    result.Lists.Add(ruleModel);
                }

                result.Total = await _dbContext.Rules.CountAsync(x =>
                    x.WorkflowState != Constants.WorkflowStates.Removed);

                return new OperationDataResult<NotificationRuleListsModel>(true, result);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return new OperationDataResult<NotificationRuleListsModel>(
                    false,
                    _localizationService.GetString("ErrorWhileObtainingNotificationRulesInfo"));
            }
        }


        private void SetDefaultValue(IEnumerable<Element> elementLst, int itemId, string value)
        {
            foreach (var element in elementLst)
            {
                if (element is DataElement dataElement)
                {
                    foreach (var item in dataElement.DataItemList.Where(item => itemId == item.Id))
                    {
                        switch (item)
                        {
                            case NumberStepper numberStepper:
                                numberStepper.DefaultValue = int.Parse(value);
                                break;
                            case Number number:
                                number.DefaultValue = int.Parse(value);
                                break;
                            case Comment comment:
                                comment.Value = value;
                                break;
                            case Text text:
                                text.Value = value;
                                break;
                            case None none:
                                var cDataValue = new CDataValue();
                                cDataValue.InderValue = value;
                                none.Description = cDataValue;
                                break;
                            case EntitySearch entitySearch:
                                entitySearch.DefaultValue = int.Parse(value);
                                break;
                            case EntitySelect entitySelect:
                                entitySelect.DefaultValue = int.Parse(value);
                                break;

                        }
                    }
                }
                else
                {
                    var groupElement = (GroupElement)element;
                    SetDefaultValue(groupElement.ElementList, itemId, value);
                }
            }
        }

        public int UserId
        {
            get
            {
                var value = _httpContextAccessor?.HttpContext.User?.FindFirstValue(ClaimTypes.NameIdentifier);
                return value == null ? 0 : int.Parse(value);
            }
        }
    }
}
