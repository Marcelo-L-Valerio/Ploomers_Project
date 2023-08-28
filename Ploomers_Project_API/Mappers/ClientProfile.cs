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
            // Groups the sales in a new field, and converts Type logics (0 or 1)
            // to user-friendly data
            CreateMap<Client, ClientViewModel>()
                .ForMember(dest => dest.LastWeekSales, opt =>
                {
                    opt.MapFrom(src => src.Sales);
                })
                .ForMember(dest => dest.Type, opt =>
                {
                    opt.MapFrom(src => TypeConverter.ConvertToApi(((int)src.Type)));
                });

            CreateMap<Contact, ContactViewModel>();

            // Converts the user-friendly input of type to logic (0 or 1)
            CreateMap<ClientInputModel, Client>()
                 .ForMember(dest => dest.Type, opt =>
                 {
                     opt.MapFrom(src => TypeConverter.ConvertToDb(src.Type));
                 });

            CreateMap<ContactInputModel, Contact>();
        }
    }

    //Helper class to convert the type data
    public static class TypeConverter
    {
        public static int ConvertToDb(string type)
        {
            if (type == "PF")
            {
                return 1;
            }
            else if (type == "PJ")
            {
                return 0;
            }

            throw new ArgumentException("Invalid Type. Must be either PF or PJ");
        }
        public static string ConvertToApi(int type)
        {
            if (type == 1)
            {
                return "Pessoa Física";
            }
            else if (type == 0)
            {
                return "Pessoa Jurídica";
            }

            return null;
        }
    }
}
