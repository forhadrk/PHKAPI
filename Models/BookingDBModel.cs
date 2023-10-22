namespace PHKAPI.Models
{
    public class BookingDBModel
    {
        public int OtherServicesID { get; set; }
        public int BookingMasterID { get; set; }
        public string? BookingID { get; set; } = string.Empty;
        public string? ServiceName { get; set; } = string.Empty;
        public int Price { get; set; }
        public int TotalPrice { get; set; }
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
        public string? PriceInfo { get; set; } = string.Empty;
        public string? TransactionID { get; set; } = string.Empty;
        public string? PostCode { get; set; } = string.Empty;
        public string? Suburb { get; set; } = string.Empty;
        public string? SpecialNotes { get; set; } = string.Empty;
        public string? OtherServicesList { get; set; } = string.Empty;
        public string? CardName { get; set; } = string.Empty;
        public string? CardNumber { get; set; } = string.Empty;
        public string? ExpiryMonth { get; set; } = string.Empty;
        public string? ExpiryYear { get; set; } = string.Empty;
        public string? CVCNo { get; set; } = string.Empty;
        public bool IsBookingWithPayment { get; set; }

    }
}