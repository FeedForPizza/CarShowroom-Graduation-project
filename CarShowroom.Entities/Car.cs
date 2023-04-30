using System;
using System.Collections.Generic;

namespace CarShowroom.Entities;

public class Car
{
    public int CarId { get; set; }

    public string? Model { get; set; }

    public int? Hp { get; set; }

    public int? MaxSpeed { get; set; }

    public int? MinSpeed { get; set; }

    public string? TypeFuel { get; set; }

    public int? Capacity { get; set; }

    public string? TypeEngine { get; set; }

    public int? NumberOfSeats { get; set; }

    public decimal? Height { get; set; }

    public decimal? Weight { get; set; }

    public decimal? AverageExpenseTown { get; set; }

    public decimal? AverageExpenseOnroad { get; set; }

    public decimal? AverageExpenseCombined { get; set; }

    public DateTime? YearOfManufacure { get; set; }

    public int? Doors { get; set; }

    public string? TypeCompartment { get; set; }

    public decimal? OriginalPrice { get; set; }

    public string? PictureUrl { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<TestDrive> TestDrives { get; set; } = new List<TestDrive>();
}
