namespace Monitoring.Pn.Infrastructure.Models
{
    using System.Collections.Generic;
    using Microting.EformMonitoringBase.Infrastructure.Models;

    public class NotificationRuleListsModel
    {
        public int Total { get; set; }
        public List<NotificationRuleModel> Lists { get; set; }

        public NotificationRuleListsModel()
        {
            Lists = new List<NotificationRuleModel>();
        }
    }
}