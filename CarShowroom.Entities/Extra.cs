using System;
using System.Collections.Generic;

namespace CarShowroom.Entities;

public partial class Extra
{
    public int ExtraId { get; set; }

    public string? ExtraName { get; set; }

    public decimal? Price { get; set; }
}
