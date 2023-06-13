using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CarShowroom.Entities;

public partial class Order
{
    public int OrderId { get; set; }

    [Display(Name = "Оригинална цена")]
    public decimal? OriginalPrice { get; set; }

    [Display(Name = "Обща сума")]
    [Required(ErrorMessage = "Полето за  обща сума е  задължително.")]
    public decimal? TotalSum { get; set; }

    [Display(Name = "Количество")]
    [Required(ErrorMessage = "Полето за количество е задължително.")]
    [Range(0, int.MaxValue, ErrorMessage = "Количеството трябва да бъде положително число.")]
    public int? Quantity { get; set; }

    [Display(Name = "ИД на Автомобила")]
    public int? CarId { get; set; }

    [Display(Name = "ИД на Клиента")]
    public int CustomerId { get; set; }

    public virtual Car? Car { get; set; }
    public DateTime Modified19118133 { get; set; }

    public virtual Customer Customer { get; set; } = null!;
}
