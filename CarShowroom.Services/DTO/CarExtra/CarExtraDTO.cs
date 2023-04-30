using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarShowroom.Entities;
using CarShowroom.Services.DTO.CarDTO;
using CarShowroom.Services.DTO.ExtraDTO;

namespace CarShowroom.Services.DTO.CarExtra
{
    public class CarExtraDTO
    {
        public int CarExtraId { get; set; } 
        public int CarId { get; set; }

        public int ExtraId { get; set; }

        public CarDTO.CarDTO Car { get; set; }

        public ExtraDTO.ExtraDTO Extra { get; set; }
    }
}
