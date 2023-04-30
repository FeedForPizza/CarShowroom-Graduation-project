using System;
using System.Collections.Generic;

namespace CarShowroom.Entities;

public partial class Order
{
    public int OrderId { get; set; }

    public decimal? OriginalPice { get; set; }

    public decimal? TotalSum { get; set; }

    public int? Quantity { get; set; }

    public int? CarId { get; set; }

    public int CustomerId { get; set; }

    public virtual Car? Car { get; set; }

    public virtual Customer Customer { get; set; } = null!;
}
