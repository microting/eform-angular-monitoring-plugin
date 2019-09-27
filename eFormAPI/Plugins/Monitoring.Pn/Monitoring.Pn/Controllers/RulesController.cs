namespace Monitoring.Pn.Controllers
{
    using System.Threading.Tasks;
    using Abstractions;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microting.eFormApi.BasePn.Infrastructure.Database.Entities;
    using Microting.eFormApi.BasePn.Infrastructure.Models.API;
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

        [HttpGet]
        [Route("api/monitoring-pn/rules/{id}")]
        public async Task<OperationResult> GetRuleById(int id)
        {
            return await _rulesService.GetRuleById(id);
        }

        [HttpDelete]
        [Route("api/monitoring-pn/rules/{id}")]
        public async Task<OperationResult> DeleteRule(int id)
        {
            return await _rulesService.DeleteRule(id);
        }
    }
}
