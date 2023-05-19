using AutoMapper;
using CarShowroom.Data;
using CarShowroom.Entities;
using CarShowroom.Services.DTO.CarDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarShowroom.Services
{

    public class CarService
    {
        private readonly IMapper _mapper;
        private readonly CarShowroomContext _dbContext;


        public CarService(IMapper mapper, CarShowroomContext dbContext)
        {
            _mapper = mapper;
            _dbContext = dbContext;

        }

    } 
}

