namespace Monitoring.Pn.Abstractions
{
    public interface IMonitoringLocalizationService
    {
        string GetString(string key);
        string GetString(string format, params object[] args);
    }
}