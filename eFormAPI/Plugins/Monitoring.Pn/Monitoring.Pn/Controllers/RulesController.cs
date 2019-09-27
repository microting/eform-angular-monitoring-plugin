namespace Monitoring.Pn.Controllers
{
    using System.Threading.Tasks;
    using Abstractions;
    using Infrastructure.Models;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microting.eFormApi.BasePn.Infrastructure.Database.Entities;
    using Microting.eFormApi.BasePn.Infrastructure.Models.API;
    using Microting.EformMonitoringBase.Infrastructure.Models;
    using Services;

    [Authorize(Roles = EformRole.Admin)]
    public class RulesController : Controller
    {
        private readonly IRulesService _rulesService;

        public RulesController(IRulesService rulesService)
        {
            _rulesService = rulesService;
        }

        [HttpPut]
        [Route("api/monitoring-pn/rules")]
        public async Task<OperationResult> CreateNewRule([FromBody] NotificationRuleCreateModel model)
        {
            return await _rulesService.CreateNewRule(model);
        }

        [HttpPost]
        [Route("api/monitoring-pn/rules")]
        public async Task<OperationResult> UpdateRule([FromBody] NotificationRuleModel model)
        {
            return await _rulesService.UpdateRule(model);
        }

        [HttpGet]
        [Route("api/monitoring-pn/rules/{id}")]
        public async Task<OperationResult> GetRuleById(int id)
        {
            return await _rulesService.GetRuleById(id);
        }

        [HttpGet]
        [Route("api/monitoring-pn/rules")]
        public async Task<OperationResult> GetRules(NotificationListRequestModel requestModel)
        {
            return await _rulesService.GetRules(requestModel);
        }

        [HttpDelete]
        [Route("api/monitoring-pn/rules/{id}")]
        public async Task<OperationResult> DeleteRule(int id)
        {
            return await _rulesService.DeleteRule(id);
        }
    }
}
