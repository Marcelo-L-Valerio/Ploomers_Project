using AutoMapper;
using Ploomers_Project_API.Mappers.DTOs.InputModels;
using Ploomers_Project_API.Mappers.DTOs.ViewModels;
using Ploomers_Project_API.Models.Entities;

namespace Ploomers_Project_API.Mappers
{
    public class SaleProfile : Profile
    {
        public SaleProfile()
        {
            CreateMap<Sale, SaleViewModel>()
                .ForMember(dest => dest.Total, opt =>
                {
                    opt.MapFrom(src => src.Value * src.Amount);
                })
                .ForMember(dest => dest.ClientName, opt =>
                {
                    opt.MapFrom(src => src.Client.Name);
                })
                .ForMember(dest => dest.EmployeeName, opt =>
                {
                    opt.MapFrom(src => src.Employee.FirstName + " " + src.Employee.LastName);
                });

            CreateMap<SaleInputModel, Sale>();
        }
    }
}
