using AutoMapper;
using Domain.Entities;
using Shared.AuthDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.MappingProfiles
{
    public class AutthProfile : Profile
    {
        public AutthProfile()
        {
            CreateMap<RegisterDto, User>();
        }
    }
}
