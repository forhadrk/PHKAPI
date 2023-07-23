namespace PHKAPI.Models
{
    public class ServiceNameDBModel
    {
        public int ServicesID { get; set; }
        public string? ServiceName { get; set; }
        public string ServiceInfo { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public string PageUrl { get; set; } = string.Empty;
        public bool? Active { get; set; } = true;
    }
}
