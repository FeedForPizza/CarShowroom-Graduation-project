using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CarShowroom.Entities;

public class Car
{
    public int CarId { get; set; }
    [Display(Name = "Модел")]
    [Required(ErrorMessage = "Името на моделът е задължително поле.")]
    public string? Model { get; set; }

    [Display(Name = "Мощност (к.с.)")]
    [Range(1, int.MaxValue, ErrorMessage = "Мощността трябва да бъде положително число.")]
    [Required(ErrorMessage = "Мощността (к.с.) е задължително поле.")]
    public int? Hp { get; set; }

    [Display(Name = "Максимална скорост")]
    [Range(1, int.MaxValue, ErrorMessage = "Максималната скорост трябва да бъде положително число.")]
    [Required(ErrorMessage = "Максималната скорост е задължително поле.")]
    public int? MaxSpeed { get; set; }

    [Display(Name = "Минимална скорост")]
    [Range(1, int.MaxValue, ErrorMessage = "Минималната скорост трябва да бъде положително число.")]
    [Required(ErrorMessage = "Минималната скорост е задължително поле.")]
    public int? MinSpeed { get; set; }

    [Display(Name = "Тип гориво")]
    [Required(ErrorMessage = "Типът гориво е задължително поле.")]
    public string? TypeFuel { get; set; }

    [Display(Name = "Капацитет")]
    [Required(ErrorMessage = "Капацитетът е задължително поле.")]
    [Range(1, int.MaxValue, ErrorMessage = "Капацитетът трябва да бъде положително число.")]
    public int? Capacity { get; set; }

    [Display(Name = "Тип двигател")]
    [Required(ErrorMessage = "Типът двигател е задължително поле.")]
    public string? TypeEngine { get; set; }

    [Display(Name = "Брой седалки")]
    [Range(1, int.MaxValue, ErrorMessage = "Броят на седалките трябва да бъде положително число.")]
    [Required(ErrorMessage = "Броят на дедалките е задължително поле.")]
    public int? NumberOfSeats { get; set; }

    [Display(Name = "Височина")]
    [Range(1, int.MaxValue, ErrorMessage = "Височината трябва да бъде положително число.")]
    [Required(ErrorMessage = "Височината е задължително поле.")]
    public decimal? Height { get; set; }

    [Display(Name = "Тегло")]
    [Range(1, int.MaxValue, ErrorMessage = "Теглото трябва да бъде положително число.")]
    [Required(ErrorMessage = "Теглото е задължително поле.")]
    public decimal? Weight { get; set; }

    [Display(Name = "Среден разход в градско")]
    [Range(1, int.MaxValue, ErrorMessage = "Среден разход в градско трябва да бъде положително число.")]
    [Required(ErrorMessage = "Средният разход в градско е задължително поле.")]
    public decimal? AverageExpenseTown { get; set; }

    [Display(Name = "Среден разход по път")]
    [Range(1, int.MaxValue, ErrorMessage = "Среден разход по път трябва да бъде положително число.")]
    [Required(ErrorMessage = "Средният разход по път е задължително поле.")]
    public decimal? AverageExpenseOnroad { get; set; }

    [Display(Name = "Среден разход общ")]
    [Range(1, int.MaxValue, ErrorMessage = "Среден разход общ трябва да бъде положително число.")]
    [Required(ErrorMessage = "Средният разход общ е задължително поле.")]
    public decimal? AverageExpenseCombined { get; set; }

    [Display(Name = "Година на Производство")]
    [Required(ErrorMessage = "Годината на производство е задължително поле.")]
    public DateTime? YearOfManufacure { get; set; }

    [Display(Name = "Брой врати")]
    [Range(1, int.MaxValue, ErrorMessage = "Брой врати трябва да бъде положително число.")]
    [Required(ErrorMessage = "Броят на вратите е задължително поле.")]
    public int? Doors { get; set; }

    [Display(Name = "Купе")]
    [Required(ErrorMessage = "Типът на купето е задължително поле.")]
    public string? TypeCompartment { get; set; }

    [Display(Name = "Цена")]
    [Range(1, int.MaxValue, ErrorMessage = "Цената трябва да бъде положително число.")]
    [Required(ErrorMessage = "Цената е задължително поле.")]
    public decimal? OriginalPrice { get; set; }

    [Display(Name = "Снимка")]
    [Required(ErrorMessage = "URL-адресът на снимката е задължително поле.")]
    public string? PictureUrl { get; set; }
    public DateTime Modified19118133 { get; set; }


public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<TestDrive> TestDrives { get; set; } = new List<TestDrive>();
    public ICollection<Storage> Storages { get; set; }
}
