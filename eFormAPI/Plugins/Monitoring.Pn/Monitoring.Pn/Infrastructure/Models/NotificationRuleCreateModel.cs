namespace Monitoring.Pn.Infrastructure.Models
{
    using System.Collections.Generic;
    using Microting.EformMonitoringBase.Infrastructure.Enums;

    public class NotificationRuleCreateModel
    {
        public int TemplateId { get; set; }
        public RuleType RuleType { get; set; }
        public object Data { get; set; }
        public int DataItemId { get; set; }
        public string Subject { get; set; }
        public string Text { get; set; }
        public bool AttachReport { get; set; }
        public List<string> Recipients { get; set; }
            = new List<string>();
    }
}