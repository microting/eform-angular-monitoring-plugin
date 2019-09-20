using System.Threading.Tasks;
using Monitoring.Pn.Infrastructure.Models;
using Microting.eFormApi.BasePn.Infrastructure.Models.API;

namespace Monitoring.Pn.Abstractions
{
    public interface IMonitoringPnSettingsService
    {
        Task<OperationDataResult<MonitoringBaseSettings>> GetSettings();
        Task<OperationResult> UpdateSettings(MonitoringBaseSettings monitoringBaseSettings);
    }
}