using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CarShowroom.Entities;

public partial class Extra
{
    public int ExtraId { get; set; }

    [Display(Name = "Екстра")]
    [Required(ErrorMessage = "Полето за име на екстра е задължително.")]
    public string? ExtraName { get; set; }

    [Display(Name = "Цена")]
    [Required(ErrorMessage = "Полето за цена е задължително.")]
    public decimal? Price { get; set; }
    public DateTime Modified19118133 { get; set; }
}
