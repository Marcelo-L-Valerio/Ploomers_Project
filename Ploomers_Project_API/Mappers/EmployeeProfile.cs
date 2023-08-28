using AutoMapper;
using Ploomers_Project_API.Mappers.DTOs.InputModels;
using Ploomers_Project_API.Mappers.DTOs.ViewModels;
using Ploomers_Project_API.Models.Entities;

namespace Ploomers_Project_API.Mappers
{
    public class EmployeeProfile : Profile
    {
        public EmployeeProfile()
        {
            // Creates the employee's company email
            CreateMap<EmployeeInputModel, Employee>()
                .ForMember(dest => dest.Email, opt =>
                {
                    opt.MapFrom(src => (src.FirstName + "." + src.LastName + "@ploomes.com").ToLower());
                });

            // Groups the sales in a new field, an groups the names fields
            CreateMap<Employee, EmployeeViewModel>()
                .ForMember(dest => dest.LastWeekSales, opt =>
                {
                    opt.MapFrom(src => src.Sales);
                })
                .ForMember(dest => dest.FullName, opt =>
                {
                    opt.MapFrom(src => (src.FirstName + " " + src.LastName));
                });
        }
    }
}
