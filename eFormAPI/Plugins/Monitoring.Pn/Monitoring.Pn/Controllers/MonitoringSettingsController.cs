using System.Threading.Tasks;
using Monitoring.Pn.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microting.eFormApi.BasePn.Infrastructure.Database.Entities;
using Microting.eFormApi.BasePn.Infrastructure.Models.API;

namespace Monitoring.Pn.Controllers
{
    using Microting.EformMonitoringBase.Infrastructure.Models.Settings;

    public class MonitoringSettingsController : Controller
    {
        private readonly IMonitoringPnSettingsService _monitoringPnSettingsService;

        public MonitoringSettingsController(IMonitoringPnSettingsService monitoringPnSettingsService)
        {
            _monitoringPnSettingsService = monitoringPnSettingsService;
        }

        [HttpGet]
        [Authorize(Roles = EformRole.Admin)]
        [Route("api/monitoring-pn/settings")]
        public async Task<OperationDataResult<MonitoringBaseSettings>> GetSettings()
        {
            return await _monitoringPnSettingsService.GetSettings();
        }

        [HttpPost]
        [Authorize(Roles = EformRole.Admin)]
        [Route("api/monitoring-pn/settings")]
        public async Task<OperationResult> UpdateSettings([FromBody] MonitoringBaseSettings monitoringBaseSettings)
        {
            return await _monitoringPnSettingsService.UpdateSettings(monitoringBaseSettings);
        }

        
    }
}