using System;
using System.Collections.Generic;

namespace CarShowroom.Entities;

public partial class Storage
{
    public int StorageId { get; set; }

    public int? Availability { get; set; }

    public DateTime? YearOfManufacture { get; set; }
}
