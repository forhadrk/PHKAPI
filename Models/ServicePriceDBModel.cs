namespace PHKAPI.Models
{
    public class ServicePriceDBModel
    {
        public int ServiceListID { get; set; }        
        public string? ServiceTitle { get; set; }
        public string PriceFor { get; set; } = string.Empty;
        public string DivID { get; set; } = string.Empty;
        public int Price { get; set; } = 0;
        public bool? Active { get; set; } = true;
        public bool? IsChecked { get; set; } = true;

        public int ServicesID { get; set; }
        public int CategoryPriceID { get; set; }
        public string? Title { get; set; }
        public string? PriceInfo { get; set; }
        public string ServicesListID { get; set; } = string.Empty;
    }
}
