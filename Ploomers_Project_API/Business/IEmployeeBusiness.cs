using Ploomers_Project_API.DTOs.ViewModels;
using Ploomers_Project_API.Mappers.DTOs.InputModels;
using Ploomers_Project_API.Mappers.DTOs.ViewModels;

namespace Ploomers_Project_API.Business
{
    public interface IEmployeeBusiness
    {
        EmployeeViewModel Create(EmployeeInputModel employee);
        EmployeeViewModel FindById(Guid id);
        void Update(Guid id, EmployeeInputModel employee);
        void Delete(Guid id);
        List<EmployeeViewModel> FindAll();
    }
}
