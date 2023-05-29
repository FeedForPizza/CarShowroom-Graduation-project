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

    public int CarId { get; set; }

    public DateTime? DateOfTestDrive { get; set; }

    public DateTime? DateOfQuery { get; set; }

    public int CustomerId { get; set; }

    public virtual Car Car { get; set; } = null!;

    public virtual Customer Customer { get; set; } = null!;
}
