namespace CarShowroom.Models
{
    public class OrderReportModel
    {
        public int OrderId { get; set; }
        public decimal? OriginalPrice { get; set; }
        public decimal? TotalSum { get; set; }
        public int? Quantity { get; set; }
        public string CarModel { get; set; }
        public string CustomerName { get; set; }
        // Additional aggregated metrics properties
    }
}
