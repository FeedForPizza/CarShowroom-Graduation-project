using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarShowroom.Entities;

public partial class TestDrive
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int TestDriveId { get; set; }

    [Display(Name = "ИД на Автомобила")]
    public int CarId { get; set; }

    [Display(Name = "Дата на тестово шофиране")]
    [Required(ErrorMessage = "Полето за дата на тестово шофиране е задължително.")]
    public DateTime? DateOfTestDrive { get; set; }

    [Display(Name = "Дата на заявка")]
    public DateTime? DateOfQuery { get; set; }

    [Display(Name = "ИД на Клиента")]
    public int CustomerId { get; set; }

    [Display(Name = "Автомобил")]
    public virtual Car Car { get; set; }

    [Display(Name = "Клиент")]
    public virtual Customer Customer { get; set; } = null!;

    [Display(Name = "Дата на Промяна")]
    public DateTime Modified19118133 { get; set; }
}
