namespace Monitoring.Pn.Infrastructure.Models
{
    public class NotificationListRequestModel
    {
        public int PageIndex { get; set; }
        public int Offset { get; set; }
        public int PageSize { get; set; }
    }
}
