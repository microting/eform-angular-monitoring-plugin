
namespace Monitoring.Pn.Abstractions
{
    using System.Threading.Tasks;
    using Infrastructure.Models;
    using Microting.eFormApi.BasePn.Infrastructure.Models.API;
    using Microting.EformMonitoringBase.Infrastructure.Models;

    public interface IRulesService
    {
        Task<OperationResult> CreateNewRule(NotificationRuleModel ruleModel);
        Task<OperationDataResult<NotificationRuleModel>> GetRuleById(int id);
        Task<OperationResult> DeleteRule(int id);
        Task<OperationResult> UpdateRule(NotificationRuleModel ruleModel);
        Task<OperationDataResult<NotificationRulesListModel>> GetRules(NotificationListRequestModel requestModel);
    }
}
