namespace Monitoring.Pn.Controllers
{
    using System.Threading.Tasks;
    using Abstractions;
    using Infrastructure.Models;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microting.eFormApi.BasePn.Infrastructure.Models.API;
    using Microting.EformMonitoringBase.Infrastructure.Const;
    using Microting.EformMonitoringBase.Infrastructure.Models;
    using Microting.EformMonitoringBase.Infrastructure.Const;
 
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
        public async Task<OperationResult> Index(NotificationListRequestModel requestModel)
        {
            return await _rulesService.Index(requestModel);
        }
        
        [HttpPost("api/monitoring-pn/rules")]
        [Authorize(Policy = MonitoringClaims.CreateNotificationRules)]
        public async Task<OperationResult> Create([FromBody] NotificationRuleModel model)
        {
            return await _rulesService.Create(model);
        }

        [HttpGet("api/monitoring-pn/rules/{id}")]
        [Authorize(Policy = MonitoringClaims.AccessMonitoringPlugin)]
        public async Task<OperationResult> Read(int id)
        {
            return await _rulesService.Read(id);
        }

        
        [HttpPut("api/monitoring-pn/rules")]
        [Authorize(Policy = MonitoringClaims.UpdateNotificationRules)]
        public async Task<OperationResult> Update([FromBody] NotificationRuleModel model)
        {
            return await _rulesService.Update(model);
        }

        [HttpDelete("api/monitoring-pn/rules/{id}")]
        [Authorize(Policy = MonitoringClaims.DeleteNotificationRules)]
        public async Task<OperationResult> Delete(int id)
        {
            return await _rulesService.Delete(id);
        }
    }
}
