using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarShowroom.Entities
{
    public partial class OrderExtra
    {
        public int OrderExtraId { get; set; }
        public int OrderId { get; set; }

        public int ExtraId { get; set; }

        public virtual Order Order { get; set; } = null!;

        public virtual Extra Extra { get; set; } = null!;
    }
}
