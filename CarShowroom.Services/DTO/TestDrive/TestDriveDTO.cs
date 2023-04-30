using CarShowroom.Services.DTO.Customer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarShowroom.Services.DTO.TestDrive
{
    public class TestDriveDTO
    {
        public int TestDriveId { get; set; }

        public int CarId { get; set; }

        public DateTime? DateOfTestDrive { get; set; }

        public DateTime? DateOfQuery { get; set; }

        public int CustomerId { get; set; }

        public virtual CarDTO.CarDTO Car { get; set; } = null!;

        public virtual CustomerDTO Customer { get; set; } = null!;
    }
}
