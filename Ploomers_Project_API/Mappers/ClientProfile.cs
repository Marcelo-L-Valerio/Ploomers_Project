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

            CreateMap<ClientInputModel, Client>()
                 .ForMember(dest => dest.Type, opt =>
                 {
                     opt.MapFrom(src => TipoConverter.Convert(src.Type));
                 });
            CreateMap<ContactInputModel, Contact>();
        }
    }

    public static class TipoConverter
    {
        public static int Convert(string tipo)
        {
            if (tipo == "PF")
            {
                return 1;
            }
            else if (tipo == "PJ")
            {
                return 0;
            }

            // Caso não seja "PF" nem "PJ", você pode definir um valor padrão ou lançar uma exceção, dependendo da sua necessidade.
            throw new ArgumentException("Tipo inválido.");
        }
    }
}
