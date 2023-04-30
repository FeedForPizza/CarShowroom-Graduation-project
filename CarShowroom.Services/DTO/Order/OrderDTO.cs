using CarShowroom.Services.DTO.Customer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarShowroom.Services.DTO.Order
{
    public class OrderDTO
    {
        public int OrderId { get; set; }

        public decimal? OriginalPice { get; set; }

        public decimal? TotalSum { get; set; }

        public int? Quantity { get; set; }

        public int? CarId { get; set; }

        public int CustomerId { get; set; }

        public virtual CarDTO.CarDTO? Car { get; set; }

        public virtual CustomerDTO Customer { get; set; } = null!;
    }
}
