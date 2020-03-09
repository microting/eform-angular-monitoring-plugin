using Microting.eForm.Infrastructure;

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
        public static string GetRuleTriggerString(NotificationRule notificationRule, MicrotingDbContext dbContext)
        {
            var result = "";
            var jsonSettings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Include
            };
            switch (notificationRule.RuleType)
            {
                case RuleType.Select:
                    var multiSelectBlock = JsonConvert.DeserializeObject<SelectBlock>(notificationRule.Data, jsonSettings);
                    result = multiSelectBlock.KeyValuePairList
                        .Where(i => i.Selected)
                        .Aggregate(result, (current, item) => current + $"<p>{multiSelectBlock.Label} = {item.Value}</p>");
                    break;
                case RuleType.CheckBox:
                    var checkBoxBlock = JsonConvert.DeserializeObject<CheckBoxBlock>(notificationRule.Data, jsonSettings);
                    var checkboxState = checkBoxBlock.Selected ? "Checked" : "Not Checked";
                    result = $"<p>{checkBoxBlock.Label} = {checkboxState}</p>";
                    break;
                case RuleType.Number:
                    var numberBlock = JsonConvert.DeserializeObject<NumberBlock>(notificationRule.Data, jsonSettings);

                    if (numberBlock.GreaterThanValue != null)
                        result += $"<p>{numberBlock.Label} > {numberBlock.GreaterThanValue}</p>";

                    if (numberBlock.LessThanValue != null)
                        result += $"<p>{numberBlock.Label} < {numberBlock.LessThanValue}</p>";

                    if (numberBlock.EqualValue != null)
                        result += $"<p>{numberBlock.Label} = {numberBlock.EqualValue}</p>";
                    break;
                case null:
                    if (notificationRule.DeviceUsers.Any())
                    {
                        foreach (DeviceUser deviceUser in notificationRule.DeviceUsers)
                        {
                            result += dbContext.sites.Single(x => x.MicrotingUid == deviceUser.DeviceUserId).Name;
                        }
                    }
                    break;
            }

            return result;
        }
    }
}
