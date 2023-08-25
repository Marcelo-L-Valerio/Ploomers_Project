using Ploomers_Project_API.Mappers.DTOs.InputModels;
using Ploomers_Project_API.Mappers.DTOs.ViewModels;

namespace Ploomers_Project_API.Business
{
    public interface ISaleBusiness
    {
        SaleViewModel Create(Guid clientId, SaleInputModel sale);
        void Update(Guid id, SaleInputModel sale);
        void Delete(Guid id);
        List<SaleViewModel> FindTodaySales(DateOnly today);
        List<SaleViewModel> FindOneClientSales(Guid clientId);
    }
}
