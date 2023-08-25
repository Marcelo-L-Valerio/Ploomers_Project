using Microsoft.EntityFrameworkCore;
using Ploomers_Project_API.Models.Context;
using Ploomers_Project_API.Models.Entities;
using System.Data;

namespace Ploomers_Project_API.Repository.Implementations
{
    public class SaleRepositoryImplementation : ISaleRepository
    {
        private SqlServerContext _context;
        private DbSet<Sale> dataset;
        public SaleRepositoryImplementation(SqlServerContext context)
        {
            _context = context;
            dataset = _context.Set<Sale>();
        }

        public Sale Create(Sale sale)
        {
            var result = _context.Clients
                .FirstOrDefault(c => c.Id.Equals(sale.ClientId));

            if (result == null) return null;

            sale.Client = result;
            dataset.Add(sale);
            _context.SaveChanges();
            return sale;
        }

        public void Delete(Guid id)
        {
            var result = dataset.SingleOrDefault(s => s.Id.Equals(id));
            if (result != null)
            {
                dataset.Remove(result);
                _context.SaveChanges();
            }
            else
            {
                throw new Exception("Sale not found!");
            }
        }

        public Sale Update(Sale sale)
        {
            var result = dataset.SingleOrDefault(p => p.Id.Equals(sale.Id));
            if (result != null)
            {
                result.Amount = sale.Amount;
                result.Value = sale.Value;
                result.Product = sale.Product;
                dataset.Update(result);
                _context.SaveChanges();
                return result;
            }
            else
            {
                throw new Exception("Sale not found!");
            }
        }

        public List<Sale> FindOneClientSales(Guid clientId)
        {
            var queryset = dataset.Where(s => s.ClientId.Equals(clientId)).ToList();

            var clientObject = _context.Clients.SingleOrDefault(c => c.Id.Equals(clientId));
            queryset.ForEach(s => s.Client = clientObject);

            return queryset;
        }

        public List<Sale> FindTodaySales(DateOnly today)
        {
            DateTime start = today.ToDateTime(TimeOnly.MinValue);
            DateTime end = today.ToDateTime(TimeOnly.MaxValue);

            var queryset = dataset.Where(s => s.Date <= end && s.Date >= start).ToList();

            foreach (var sale in queryset)
            {
                sale.Client = _context.Clients.SingleOrDefault(c => 
                    c.Id.Equals(sale.ClientId));
            }
            return queryset;
        }
    }
}
