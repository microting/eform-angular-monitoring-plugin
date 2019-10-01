namespace Monitoring.Pn.Helpers
{
    using System;
    using System.Linq;
    using Microting.EformMonitoringBase.Infrastructure.Data.Entities;
    using Microting.EformMonitoringBase.Infrastructure.Enums;
    using Microting.EformMonitoringBase.Infrastructure.Models.Blocks;
    using Newtonsoft.Json;

    public static class RulesBlockHelper
    {
        public static string GetRuleTriggerString(NotificationRule notificationRule)
        {
            var result = "";
            switch (notificationRule.RuleType)
            {
                case RuleType.Select:
                    var multiSelectBlock = JsonConvert.DeserializeObject<SelectBlock>(notificationRule.Data);
                    result = multiSelectBlock.KeyValuePairList
                        .Where(i => i.Selected)
                        .Aggregate(result, (current, item) => current + $"<p>{multiSelectBlock.Label} = {item.Value}</p>");
                    break;
                case RuleType.CheckBox:
                    var checkBoxBlock = JsonConvert.DeserializeObject<CheckBoxBlock>(notificationRule.Data);
                    var checkboxState = checkBoxBlock.Selected ? "Checked" : "Not Checked";
                    result = $"<p>{checkBoxBlock.Label} = {checkboxState}</p>";
                    break;
                case RuleType.Number:
                    var numberBlock = JsonConvert.DeserializeObject<NumberBlock>(notificationRule.Data);

                    if (numberBlock.GreaterThanValue != null)
                        result += $"<p>{numberBlock.Label} > {numberBlock.GreaterThanValue}</p>";

                    if (numberBlock.LessThanValue != null)
                        result += $"<p>{numberBlock.Label} < {numberBlock.LessThanValue}</p>";

                    if (numberBlock.EqualValue != null)
                        result += $"<p>{numberBlock.Label} = {numberBlock.EqualValue}</p>";
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return result;
        }
    }
}
