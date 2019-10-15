using Monitoring.Pn.Abstractions;
using Microsoft.Extensions.Localization;
using Microting.eFormApi.BasePn.Localization.Abstractions;

namespace Monitoring.Pn.Services
{
    public class MonitoringLocalizationService :IMonitoringLocalizationService
    {
        private readonly IStringLocalizer _localizer;
        
        // ReSharper disable once SuggestBaseTypeForParameter
        public MonitoringLocalizationService(IEformLocalizerFactory factory)
        {
            _localizer = factory.Create(typeof(EformMonitoringPlugin));
        }
        
        public string GetString(string key)
        {
            var str = _localizer[key];
            return str.Value;
        }

        public string GetString(string format, params object[] args)
        {
            var message = _localizer[format];

            return message?.Value == null ? null : string.Format(message.Value, args);
        }
    }
}