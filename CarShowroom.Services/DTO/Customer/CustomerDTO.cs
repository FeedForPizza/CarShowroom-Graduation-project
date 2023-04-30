using CarShowroom.Services.DTO.Order;
using CarShowroom.Services.DTO.TestDrive;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarShowroom.Services.DTO.Customer
{
    public class CustomerDTO
    {
        public int CustomerId { get; set; }

        public string FirstName { get; set; } = null!;

        public string MiddleName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public string Address { get; set; } = null!;

        public string Phone { get; set; } = null!;

        public virtual ICollection<OrderDTO> Orders { get; set; } = new List<OrderDTO>();

        public virtual ICollection<TestDriveDTO> TestDrives { get; set; } = new List<TestDriveDTO>();
    }
}
