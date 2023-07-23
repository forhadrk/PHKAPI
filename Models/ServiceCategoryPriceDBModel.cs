namespace PHKAPI.Models
{
    public class ServiceCategoryPriceDBModel
    {
        public int CategoryPriceID { get; set; }
        public int ServicesID { get; set; }
        public string? Title { get; set; }
        public int? Price { get; set; }
        public string? PriceInfo { get; set; }
        public string? ServiceName { get; set; }
        public string? ServiceInfo { get; set; }        
    }
}
