using Ploomers_Project_API.Models.Entities;

namespace Ploomers_Project_API.Repository
{
    public interface ISaleRepository
    {
        Sale Create(Sale sale, string employeeEmail);
        Sale Update(Sale sale);
        void Delete(Guid id);
        List<Sale> FindTodaySales(DateOnly today);
        List<Sale> FindOneClientSales(Guid clientId);
        List<Sale> FindOneEmployeeSales(Guid employeeId);
    }
}
