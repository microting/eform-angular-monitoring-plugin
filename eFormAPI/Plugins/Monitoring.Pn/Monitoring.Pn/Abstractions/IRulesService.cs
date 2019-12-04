
namespace Monitoring.Pn.Abstractions
{
    using System.Threading.Tasks;
    using Infrastructure.Models;
    using Microting.eFormApi.BasePn.Infrastructure.Models.API;
    using Microting.EformMonitoringBase.Infrastructure.Models;

    public interface IRulesService
    {
        Task<OperationDataResult<NotificationRulesListModel>> Index(NotificationListRequestModel requestModel);
        Task<OperationResult> Create(NotificationRuleModel ruleModel);
        Task<OperationDataResult<NotificationRuleModel>> Read(int id);
        Task<OperationResult> Delete(int id);
        Task<OperationResult> Update(NotificationRuleModel ruleModel);
    }
}
