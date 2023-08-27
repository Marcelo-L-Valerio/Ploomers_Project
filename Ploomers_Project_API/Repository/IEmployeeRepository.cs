using Ploomers_Project_API.Models.Entities;

namespace Ploomers_Project_API.Repository
{
    public interface IEmployeeRepository
    {
        Employee Create(Employee employee);
        Employee FindById(Guid id);
        Employee Update(Employee employee);
        void Delete(Guid id);
        List<Employee> FindAll();
    }
}
