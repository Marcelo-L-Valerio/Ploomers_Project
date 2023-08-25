using AutoMapper;
using Ploomers_Project_API.DTOs.ViewModels;
using Ploomers_Project_API.Mappers.DTOs.InputModels;
using Ploomers_Project_API.Mappers.DTOs.ViewModels;
using Ploomers_Project_API.Models.Entities;

namespace Ploomers_Project_API.Mappers
{
    public class ClientProfile : Profile
    {
        public ClientProfile()
        {
            CreateMap<Client, ClientViewModel>()
                .ForMember(dest => dest.LastWeekSales, opt =>
                {
                    opt.MapFrom(src => src.Sales);
                });

            CreateMap<Contact, ContactViewModel>();

            CreateMap<ClientInputModel, Client>();
            CreateMap<ContactInputModel, Contact>();
        }
    }
}
