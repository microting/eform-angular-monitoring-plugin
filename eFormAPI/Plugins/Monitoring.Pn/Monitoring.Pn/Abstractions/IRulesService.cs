
namespace Monitoring.Pn.Abstractions
{
    using System.Threading.Tasks;
    using Microting.eFormApi.BasePn.Infrastructure.Models.API;
    using Microting.EformMonitoringBase.Infrastructure.Models;
    using Services;

    public interface IRulesService
    {
        Task<OperationResult> CreateNewRule(NotificationRuleCreateModel requestModel);
        Task<OperationDataResult<NotificationRuleModel>> GetRuleById(int id);
        Task<OperationResult> DeleteRule(int id);
    }
}
