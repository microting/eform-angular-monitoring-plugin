﻿using System.Collections.Generic;
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
        
        public async Task<OperationDataResult<NotificationRulesListModel>> Index(NotificationListRequestModel requestModel)
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
                    .Include(x => x.DeviceUsers)
                    .ToListAsync();

                var result = new NotificationRulesListModel();
                foreach (var rule in rules)
                {
                    string eFormName;
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
                        Event = "Email"
                    };

                    if (rule.Data != null && !string.IsNullOrEmpty(rule.Data))
                    {
                        ruleModel.Trigger = RulesBlockHelper.GetRuleTriggerString(rule);
                    }

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

        public async Task<OperationResult> Create(NotificationRuleModel ruleModel)
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
                        CheckListId = ruleModel.CheckListId,
                        RuleType = ruleModel.RuleType,
                        CreatedByUserId = UserId,
                        UpdatedByUserId = UserId,
                    };

                    if (ruleModel.Data != null)
                    {
                        notificationRule.Data = ruleModel.Data?.ToString();
                    }

                    await notificationRule.Create(_dbContext);

                    foreach (var recipientModel in ruleModel.Recipients)
                    {
                        var recipient = new Recipient()
                        {
                            CreatedByUserId = UserId,
                            UpdatedByUserId = UserId,
                            Email = recipientModel.Email,
                            NotificationRuleId = notificationRule.Id,
                        };
                        await recipient.Create(_dbContext);
                    }

                    var deviceUsersGroupedIds = ruleModel.DeviceUsers
                        .Where(x=> x.Id != null)
                        .GroupBy(x => x.Id)
                        .Select(x => x.Key)
                        .ToList();

                    foreach (var deviceUserId in deviceUsersGroupedIds)
                    {
                        if (deviceUserId != null)
                        {
                            var deviceUser = new DeviceUser()
                            {
                                CreatedByUserId = UserId,
                                UpdatedByUserId = UserId,
                                NotificationRuleId = notificationRule.Id,
                                DeviceUserId = (int) deviceUserId,
                            };
                            await deviceUser.Create(_dbContext);
                        }
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

        public async Task<OperationDataResult<NotificationRuleModel>> Read(int id)
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

                var deviceUsers = await _dbContext.DeviceUsers
                    .Where(x => x.NotificationRuleId == rule.Id
                                && x.WorkflowState != Constants.WorkflowStates.Removed)
                    .Select(x => new DeviceUserModel()
                    {
                        Id = x.DeviceUserId,
                    }).ToListAsync();

                var core = await _coreHelper.GetCore();
                foreach (var deviceUserModel in deviceUsers)
                {
                    if (deviceUserModel.Id != null)
                    {
                        var sdkDeviceUser = await core.SiteRead((int) deviceUserModel.Id);
                        deviceUserModel.FirstName = sdkDeviceUser.FirstName;
                        deviceUserModel.LastName = sdkDeviceUser.LastName;
                    }
                }

                var ruleModel = new NotificationRuleModel()
                {
                    Id = rule.Id,
                    CheckListId = rule.CheckListId,
                    DataItemId = rule.DataItemId,
                    RuleType = rule.RuleType,
                    AttachReport = rule.AttachReport,
                    Subject = rule.Subject,
                    Text = rule.Text,
                    Recipients = recipients,
                    DeviceUsers = deviceUsers,
                };

                if (!string.IsNullOrEmpty(rule.Data) && rule.RuleType != null)
                {
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

        public async Task<OperationResult> Update(NotificationRuleModel ruleModel)
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
                    rule.RuleType = ruleModel.RuleType;
                    rule.Subject = ruleModel.Subject;
                    rule.CheckListId = ruleModel.CheckListId;
                    rule.Text = ruleModel.Text;
                    rule.DataItemId = ruleModel.DataItemId;

                    if (ruleModel.Data != null)
                    {
                        rule.Data = ruleModel.Data.ToString();
                    }

                    await rule.Update(_dbContext);

                    // work with recipients
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

                        await recipient.Create(_dbContext);
                    }

                    // work with device users
                    var deviceUsersDelete = await _dbContext.DeviceUsers
                        .Where(r => r.NotificationRuleId == rule.Id && ruleModel.DeviceUsers.All(rm => rm.Id != r.DeviceUserId))
                        .ToListAsync();

                    foreach (var dud in deviceUsersDelete)
                    {
                        await dud.Delete(_dbContext);
                    }

                    foreach (var deviceUserModel in ruleModel.DeviceUsers)
                    {
                        if (!await _dbContext.DeviceUsers.AnyAsync(
                            x => x.DeviceUserId == deviceUserModel.Id &&
                                 x.NotificationRuleId == rule.Id && 
                                 x.WorkflowState != Constants.WorkflowStates.Removed))
                        {
                            if (deviceUserModel.Id != null)
                            {
                                var deviceUser = new DeviceUser()
                                {
                                    NotificationRuleId = rule.Id,
                                    CreatedByUserId = UserId,
                                    UpdatedByUserId = UserId,
                                    DeviceUserId = (int)deviceUserModel.Id,
                                };

                                await deviceUser.Create(_dbContext);
                            }
                        }
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

        public async Task<OperationResult> Delete(int id)
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

                    // recipients
                    var recipients = await _dbContext.Recipients
                        .Where(x => x.NotificationRuleId == notificationRule.Id)
                        .ToListAsync();

                    foreach (var recipient in recipients)
                    {
                        await recipient.Delete(_dbContext);
                    }

                    // device users
                    var deviceUsers = await _dbContext.DeviceUsers
                        .Where(x => x.NotificationRuleId == notificationRule.Id)
                        .ToListAsync();

                    foreach (var deviceUser in deviceUsers)
                    {
                        await deviceUser.Delete(_dbContext);
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
