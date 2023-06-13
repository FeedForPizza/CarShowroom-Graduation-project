using System.ComponentModel.DataAnnotations;

namespace CarShowroom.Models
{
    public class OrderReportModel
    {
        [Display(Name = "ИД на Поръчката")]
        public int OrderId { get; set; }

        [Display(Name = "Оригинална цена")]
        public decimal? OriginalPrice { get; set; }

        [Display(Name = "Обща сума")]
        public decimal? TotalSum { get; set; }

        [Display(Name = "Количество")]
        public int? Quantity { get; set; }

        [Display(Name = "Модел на автомобила")]
        public string CarModel { get; set; }

        [Display(Name = "Име на клиента")]

        public string CustomerName { get; set; }
        // Additional aggregated metrics properties
    }
}
