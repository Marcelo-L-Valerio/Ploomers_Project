using AutoMapper;
using Ploomers_Project_API.Mappers.DTOs.InputModels;
using Ploomers_Project_API.Mappers.DTOs.ViewModels;
using Ploomers_Project_API.Models.Entities;
using Ploomers_Project_API.Repository;

namespace Ploomers_Project_API.Business.Implementations
{
    public class SaleBusinessImplementation : ISaleBusiness
    {
        private readonly ISaleRepository _saleRepository;
        private readonly IMapper _mapper;
        public SaleBusinessImplementation(ISaleRepository repository, IMapper mapper)
        {
            _saleRepository = repository;
            _mapper = mapper;
        }

        public SaleViewModel Create(Guid clientId, SaleInputModel sale)
        {
            var mappedSale = _mapper.Map<Sale>(sale);
            mappedSale.Date = DateTime.Now;
            mappedSale.ClientId = clientId;
            var saleEntity = _saleRepository.Create(mappedSale);

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
