using System;
using System.Collections.Generic;

namespace CarShowroom.Entities;

public partial class TestDrive
{
    public int TestDriveId { get; set; }

    public int CarId { get; set; }

    public DateTime? DateOfTestDrive { get; set; }

    public DateTime? DateOfQuery { get; set; }

    public int CustomerId { get; set; }

    public virtual Car Car { get; set; } = null!;

    public virtual Customer Customer { get; set; } = null!;
}
