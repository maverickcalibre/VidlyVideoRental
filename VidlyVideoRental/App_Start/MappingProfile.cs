using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VidlyVideoRental.Dtos;
using VidlyVideoRental.Models;

namespace VidlyVideoRental.App_Start
{
    /// <summary>
    /// This class defines how objects of different types map to each other
    /// </summary>
    public class MappingProfile: Profile
    {
        public MappingProfile()
        {
            Mapper.CreateMap<CustomerDto, Customer>();
            Mapper.CreateMap<Customer, CustomerDto>();
        }
    }
}