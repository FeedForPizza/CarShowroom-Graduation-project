using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarShowroom.Services.DTO.Storage
{
    public class StorageDTO
    {
        public int StorageId { get; set; }

        public int? Availability { get; set; }

        public DateTime? YearOfManufacture { get; set; }

    }
}
