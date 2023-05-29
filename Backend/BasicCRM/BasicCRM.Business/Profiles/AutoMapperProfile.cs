using AutoMapper;
using BasicCRM.Business.Dtos.AddressDto;
using BasicCRM.Business.Dtos.ClientDto;
using BasicCRM.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicCRM.Business.Profiles
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Address, GetAddressDto>().ReverseMap();
            CreateMap<AddressToCreateDto, Address>();
            CreateMap<AddressToUpdateDto, Address>();

            CreateMap<Client, GetClientDto>().ReverseMap();
            CreateMap<ClientToCreateDto, Client>();
            CreateMap<ClientToUpdateDto, Client>();

            CreateMap<ClientWithAddress, GetClientDto>();
        }
    }
}
