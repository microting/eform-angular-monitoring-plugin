namespace Monitoring.Pn.Infrastructure.Models
{
    using System.Collections.Generic;

    public class NotificationRulesListModel
    {
        public int Total { get; set; }
        public List<NotificationRuleSimpleModel> Rules { get; set; }

        public NotificationRulesListModel()
        {
            Rules = new List<NotificationRuleSimpleModel>();
        }
    }

    public class NotificationRuleSimpleModel
    {
        public int Id { get; set; }
        public string Trigger { get; set; }
        public string Event { get; set; }
        public string EFormName { get; set; }
    }
}