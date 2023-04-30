using CarShowroom.Services.DTO.Order;
using CarShowroom.Services.DTO.TestDrive;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarShowroom.Services.DTO.CarDTO
{
    public class CarDTO
    {
        public int CarId { get; set; }

        public string? Model { get; set; }

        public int? Hp { get; set; }

        public int? MaxSpeed { get; set; }

        public int? MinSpeed { get; set; }

        public string? TypeFuel { get; set; }

        public int? Capacity { get; set; }

        public string? TypeEngine { get; set; }

        public int? NumberOfSeats { get; set; }

        public decimal? Height { get; set; }

        public decimal? Weight { get; set; }

        public decimal? AverageExpenseTown { get; set; }

        public decimal? AverageExpenseOnroad { get; set; }

        public decimal? AverageExpenseCombined { get; set; }

        public DateTime? YearOfManufacure { get; set; }

        public int? Doors { get; set; }

        public string? TypeCompartment { get; set; }

        public decimal? OriginalPrice { get; set; }

        public string? PictureUrl { get; set; }

        public virtual ICollection<OrderDTO> Orders { get; set; } = new List<OrderDTO>();

        public virtual ICollection<TestDriveDTO> TestDrives { get; set; } = new List<TestDriveDTO>();
    }
}
