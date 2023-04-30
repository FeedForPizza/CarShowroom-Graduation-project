using System;
using System.Collections.Generic;

namespace CarShowroom.Entities;

public partial class Customer
{
    public int CustomerId { get; set; }

    public string FirstName { get; set; } = null!;

    public string MiddleName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Address { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<TestDrive> TestDrives { get; set; } = new List<TestDrive>();
}
