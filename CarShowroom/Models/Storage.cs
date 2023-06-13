using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CarShowroom.Entities;

public partial class Storage
{
    [Display(Name = "ИД на Склада")]
    public int StorageId { get; set; }

    [Display(Name = "Наличност")]
    public int? Availability { get; set; }

    [Display(Name = "Година на производство")]
    public DateTime? YearOfManufacture { get; set; }

    [Display(Name = "Дата на промяна")]
    public DateTime Modified19118133 { get; set; }

    [Display(Name = "Автомобил")]
    public virtual Car Car { get; set; }

    [Display(Name = "ИД на Автомобила")]
    public int CarId { get; set; }
}
