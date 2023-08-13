namespace PHKAPI.Models
{
    public class ServiceCategoryPriceDetailsDBModel
    {
        public int CategoryPriceDetailsID { get; set; }
        public int CategoryPriceID { get; set; }
        public int ServicesID { get; set; }
        public string? ServiceName { get; set; }
        public string? ServiceCategory { get; set; }
        public string? Title { get; set; }
    }
}
