namespace Monitoring.Pn.Controllers
{
    using System.Threading.Tasks;
    using Abstractions;
    using Infrastructure.Models;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microting.eFormApi.BasePn.Infrastructure.Models.API;
    using Microting.EformMonitoringBase.Infrastructure.Models;
    using Microting.EformMonitoringBase.Infrastructure.Data.Const;

    [Authorize]
    public class RulesController : Controller
    {
        private readonly IRulesService _rulesService;

        public RulesController(IRulesService rulesService)
        {
            _rulesService = rulesService;
        }

        [HttpGet("api/monitoring-pn/rules")]
        [Authorize(Policy = MonitoringClaims.AccessMonitoringPlugin)]
        public async Task<OperationResult> GetRules(NotificationListRequestModel requestModel)
        {
            return await _rulesService.GetRules(requestModel);
        }

        [HttpGet("api/monitoring-pn/rules/{id}")]
        [Authorize(Policy = MonitoringClaims.AccessMonitoringPlugin)]
        public async Task<OperationResult> GetRuleById(int id)
        {
            return await _rulesService.GetRuleById(id);
        }

        [HttpPost("api/monitoring-pn/rules")]
        [Authorize(Policy = MonitoringClaims.CreateNotificationRules)]
        public async Task<OperationResult> CreateNewRule([FromBody] NotificationRuleModel model)
        {
            return await _rulesService.CreateNewRule(model);
        }

        [HttpPut("api/monitoring-pn/rules")]
        [Authorize(Policy = MonitoringClaims.UpdateNotificationRules)]
        public async Task<OperationResult> UpdateRule([FromBody] NotificationRuleModel model)
        {
            return await _rulesService.UpdateRule(model);
        }

        [HttpDelete("api/monitoring-pn/rules/{id}")]
        [Authorize(Policy = MonitoringClaims.DeleteNotificationRules)]
        public async Task<OperationResult> DeleteRule(int id)
        {
            return await _rulesService.DeleteRule(id);
        }
    }
}
