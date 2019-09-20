namespace Monitoring.Pn.Infrastructure.Models
{
    public class MonitoringBaseSettings
    {
        public string LogLevel { get; set; }
        
        public string LogLimit { get; set; }
        
        public string SdkConnectionString { get; set; }
    }
}