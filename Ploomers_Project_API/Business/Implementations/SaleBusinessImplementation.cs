using AutoMapper;
using Ploomers_Project_API.Mappers.DTOs.InputModels;
using Ploomers_Project_API.Mappers.DTOs.ViewModels;
using Ploomers_Project_API.Models.Context;
using Ploomers_Project_API.Models.Entities;
using Ploomers_Project_API.Repository;

namespace Ploomers_Project_API.Business.Implementations
{
    public class SaleBusinessImplementation : ISaleBusiness
    {
        private readonly ISaleRepository _saleRepository;
        private readonly IMapper _mapper;
        private readonly SqlServerContext _context;
        public SaleBusinessImplementation(ISaleRepository repository, IMapper mapper, SqlServerContext context)
        {
            _saleRepository = repository;
            _mapper = mapper;
            _context = context;
        }

        public SaleViewModel Create(SaleInputModel sale, Guid client_id, string employeeEmail)
        {
            var mappedSale = _mapper.Map<Sale>(sale);
            mappedSale.ClientId = client_id;

            var saleEntity = _saleRepository.Create(mappedSale,employeeEmail);

            if (mappedSale.Client == null || mappedSale.Employee == null) return null;
            var viewModel = _mapper.Map<SaleViewModel>(saleEntity);
            return viewModel;
        }

        public void Delete(Guid id)
        {
            _saleRepository.Delete(id);
        }

        public List<SaleViewModel> FindOneClientSales(Guid clientId)
        {
            return _mapper.Map<List<SaleViewModel>>
                (_saleRepository.FindOneClientSales(clientId));
        }

        public List<SaleViewModel> FindOneEmployeeSales(Guid employeeId)
        {
            return _mapper.Map<List<SaleViewModel>>
                (_saleRepository.FindOneEmployeeSales(employeeId));
        }

        public List<SaleViewModel> FindTodaySales(DateOnly today)
        {
            return _mapper.Map<List<SaleViewModel>>
                (_saleRepository.FindTodaySales(today));
        }

        public void Update(Guid id, SaleInputModel sale)
        {
            var mappedSale = _mapper.Map<Sale>(sale);
            mappedSale.Id = id;
            var saleEntity = _saleRepository.Update(mappedSale);
        }
    }
}
