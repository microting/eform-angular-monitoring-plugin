using System.Threading.Tasks;
using Microting.eFormApi.BasePn.Infrastructure.Models.API;

namespace Monitoring.Pn.Abstractions
{
    using Microting.EformMonitoringBase.Infrastructure.Models.Settings;

    public interface IMonitoringPnSettingsService
    {
        Task<OperationDataResult<MonitoringBaseSettings>> GetSettings();
        Task<OperationResult> UpdateSettings(MonitoringBaseSettings monitoringBaseSettings);
    }
}