using CarShowroom.Services.DTO.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarShowroom.Services.DTO.OrderExtra
{
    internal class OrderExtraDTO
    {
        public int OrderExtraId { get; set; }
        public int OrderId { get; set; }

        public int ExtraId { get; set; }

        public virtual OrderDTO Order { get; set; } = null!;

        public virtual ExtraDTO.ExtraDTO Extra { get; set; } = null!;
    }
}
