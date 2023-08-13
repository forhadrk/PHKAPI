namespace PHKAPI.Models
{
    public class BookingDBModel
    {
        public int OtherServicesID { get; set; }
        public int BookingMasterID { get; set; }
        public string? ServiceName { get; set; } = string.Empty;
        public int Price { get; set; }
        public bool Active { get; set; }

        public string? BookingName { get; set; } = string.Empty;
        public string? Address { get; set; } = string.Empty;
        public string? ServiceDate { get; set; } = string.Empty;
        public string? BookingHour { get; set; } = string.Empty;
        public int CategoryPriceID { get; set; } = 0;
        public int ServicesID { get; set; } = 0;
        public string? City { get; set; } = string.Empty;
        public string? Email { get; set; } = string.Empty;
        public string? Mobile { get; set; } = string.Empty;
        public string? PostCode { get; set; } = string.Empty;
        public string? Suburb { get; set; } = string.Empty;
        public string? SpecialNotes { get; set; } = string.Empty;
        public string? OtherServicesList { get; set; } = string.Empty;

    }
}