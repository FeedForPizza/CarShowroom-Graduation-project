using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarShowroom.Entities
{
    public partial class OrderExtra
    {
        public int OrderExtraId { get; set; }
        [Display(Name = "ИД на Поръчка")]
        public int OrderId { get; set; }

        [Display(Name = "ИД на Екстра")]
        public int ExtraId { get; set; }

        [Display(Name = "Поръчка")]
        public virtual Order Order { get; set; } = null!;

        [Display(Name = "Екстра")]
        public virtual Extra Extra { get; set; } = null!;
        public DateTime Modified19118133 { get; set; }
    }
}
