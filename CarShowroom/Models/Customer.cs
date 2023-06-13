using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CarShowroom.Entities;

public partial class Customer
{
    public int CustomerId { get; set; }

    [Display(Name = "Име")]
    [Required(ErrorMessage = "Полето за име е задължително.")]
    public string FirstName { get; set; }

    [Display(Name = "Презиме")]
    [Required(ErrorMessage = "Полето за презиме е задължително.")]
    public string MiddleName { get; set; }

    [Display(Name = "Фамилия")]
    [Required(ErrorMessage = "Полето за фамилия е задължително.")]
    public string LastName { get; set; }

    [Display(Name = "Адрес")]
    [Required(ErrorMessage = "Полето за адрес е задължително.")]
    public string Address { get; set; }

    [Display(Name = "Телефон")]
    [Required(ErrorMessage = "Полето за телефон е задължително.")]
    [RegularExpression(@"^[0-9]+$", ErrorMessage = "Телефонът трябва да съдържа само цифри.")]
    public string Phone { get; set; }
    public DateTime Modified19118133 { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<TestDrive> TestDrives { get; set; } = new List<TestDrive>();
}
