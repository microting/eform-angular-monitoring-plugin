namespace Monitoring.Pn.Helpers
{
    using System;
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
                    foreach (var item in multiSelectBlock.KeyValuePairList)
                    {
                        result += $"<p>{item.Key} ";

                        if (item.Selected)
                        {
                            result += "Checked";
                        }
                        else
                        {
                            result += "Not Checked";
                        }

                        result += "</p>";
                    }
                    break;
                case RuleType.CheckBox:
                    var checkBoxBlock = JsonConvert.DeserializeObject<CheckBoxBlock>(notificationRule.Data);
                    result = $"{checkBoxBlock.Label} = ";
                    if (checkBoxBlock.Selected)
                    {
                        result += "Checked";
                    }
                    else
                    {
                        result += "Not Checked";
                    }
                    break;
                case RuleType.Number:
                    var numberBlock = JsonConvert.DeserializeObject<NumberBlock>(notificationRule.Data);

                    if (numberBlock.GreaterThanValue != null)
                        result += $"{numberBlock.Label} > {numberBlock.GreaterThanValue}";

                    if (numberBlock.LessThanValue != null)
                        result += $"{numberBlock.Label} < {numberBlock.LessThanValue}";

                    if (numberBlock.EqualValue != null)
                        result += $"{numberBlock.Label} = {numberBlock.EqualValue}";
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return result;
        }
    }
}
