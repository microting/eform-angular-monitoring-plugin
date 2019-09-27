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
                case RuleType.SingleList:

                    break;
                case RuleType.MultiSelect:
                    var multiSelectBlock = JsonConvert.DeserializeObject<MultiSelectBlock>(notificationRule.Data);
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
                    result = $"{numberBlock.Label} {NumberOperatorTypeToString(numberBlock.Type)} {numberBlock.Value}";
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return result;
        }


        public static string NumberOperatorTypeToString(OperatorType type)
        {
            switch (type)
            {
                case OperatorType.EqualTo:
                    return "=";
                case OperatorType.NotEqualTo:
                    return "!=";
                case OperatorType.GreaterThan:
                    return ">";
                case OperatorType.LessThan:
                    return "<";
                case OperatorType.GreaterOrEqualThan:
                    return ">=";
                case OperatorType.LessOrEqualThan:
                    return "<=";
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }
    }
}
