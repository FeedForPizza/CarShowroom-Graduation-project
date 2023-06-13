using System;
using System.Collections.Generic;

namespace CarShowroom.Entities;

public partial class CarExtra
{
    public int CarExtraId { get; set; }
    public int CarId { get; set; }

    public int ExtraId { get; set; }
    public DateTime Modified19118133 { get; set; }

    public virtual Car Car { get; set; } = null!;

    public virtual Extra Extra { get; set; } = null!;
}
