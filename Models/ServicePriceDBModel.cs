namespace PHKAPI.Models
{
    public class ServicePriceDBModel
    {
        public int ServiceListID { get; set; }
        public string? ServiceTitle { get; set; }
        public string PriceFor { get; set; } = string.Empty;
        public int Price { get; set; } = 0;
        public bool? Active { get; set; } = true;
    }
}
