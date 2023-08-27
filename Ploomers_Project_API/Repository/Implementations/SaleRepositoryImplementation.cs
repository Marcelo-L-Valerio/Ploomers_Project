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
            sale.Date = DateTime.Now;

            var employee = _context.Employees
                .SingleOrDefault(e => e.Email.Equals(employeeEmail));
            var client = _context.Clients
                .SingleOrDefault(e => e.Id.Equals(sale.ClientId));

            sale.Employee = employee;
            sale.Client = client;

            dataset.Add(sale);
            _context.SaveChanges();
            return sale;
        }

        public void Delete(Guid id)
        {
            var queryset = dataset.SingleOrDefault(s => s.Id.Equals(id));
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

        public Sale Update(Sale sale)
        {
            var queryset = dataset.SingleOrDefault(p => p.Id.Equals(sale.Id));
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

        public List<Sale> FindOneClientSales(Guid clientId)
        {
            var queryset = dataset.Where(s => s.ClientId.Equals(clientId)).ToList();

            var clientObject = _context.Clients.FirstOrDefault(c => c.Id.Equals(clientId));

            foreach(var sale in queryset)
            {
                sale.Client = clientObject;
                sale.Employee = _context.Employees.SingleOrDefault(c => c.Id.Equals(sale.EmployeeId));
            }

            return queryset;
        }

        public List<Sale> FindOneEmployeeSales(Guid employeeId)
        {
            var queryset = dataset.Where(s => s.EmployeeId.Equals(employeeId)).ToList();

            var employeeObject = _context.Employees.FirstOrDefault(c => c.Id.Equals(employeeId));

            foreach (var sale in queryset)
            {
                sale.Employee = employeeObject;
                sale.Client = _context.Clients.SingleOrDefault(c => c.Id.Equals(sale.ClientId));
            }

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
                sale.Employee = _context.Employees.SingleOrDefault(c => 
                    c.Id.Equals(sale.EmployeeId));
            }
            return queryset;
        }
    }
}
