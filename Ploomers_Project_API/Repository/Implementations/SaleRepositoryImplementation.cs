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

        public Sale Create(Sale sale, string employeeEmail)
        {
            // Add the missing data and saves in the database
            sale.Date = DateTime.Now;

            var employee = _context.Employees
                .SingleOrDefault(e => e.Email == employeeEmail);
            var client = _context.Clients
                .SingleOrDefault(e => e.Id == sale.ClientId);

            sale.Employee = employee;
            sale.Client = client;

            dataset.Add(sale);
            _context.SaveChanges();
            return sale;
        }

        public Sale Update(Sale sale)
        {
            var queryset = dataset.SingleOrDefault(p => p.Id == sale.Id);
            if (queryset != null)
            {
                queryset.Amount = sale.Amount;
                queryset.Value = sale.Value;
                queryset.Product = sale.Product;
                dataset.Update(queryset);
                _context.SaveChanges();
                return queryset;
            }
            else
            {
                throw new Exception("Sale not found!");
            }
        }

        public void Delete(Guid id)
        {
            var queryset = dataset.SingleOrDefault(s => s.Id == id);
            if (queryset != null)
            {
                dataset.Remove(queryset);
                _context.SaveChanges();
            }
            else
            {
                throw new Exception("Sale not found!");
            }
        }

        public List<Sale> FindOneClientSales(Guid clientId)
        {
            var queryset = dataset.Where(s => s.ClientId == clientId)
                .Include(s => s.Employee).Include(s => s.Client).ToList();

            return queryset;
        }

        public List<Sale> FindOneEmployeeSales(Guid employeeId)
        {
            var queryset = dataset.Where(s => s.EmployeeId == employeeId)
                .Include(s => s.Employee).Include(s => s.Client).ToList();

            return queryset;
        }

        public List<Sale> FindTodaySales(DateOnly today)
        {
            DateTime start = today.ToDateTime(TimeOnly.MinValue);
            DateTime end = today.ToDateTime(TimeOnly.MaxValue);

            var queryset = dataset.Where(s => s.Date <= end && s.Date >= start)
                .Include(s => s.Employee).Include(s => s.Client).ToList();

            return queryset;
        }
    }
}
