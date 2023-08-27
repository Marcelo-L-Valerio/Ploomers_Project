using Microsoft.EntityFrameworkCore;
using Ploomers_Project_API.Models.Context;
using Ploomers_Project_API.Models.Entities;
using Ploomers_Project_API.TokenService;
using System.Data;
using System.Security.Cryptography;
using System.Text;

namespace Ploomers_Project_API.Repository.Implementations
{
    public class EmployeeRepositoryImplementation : IEmployeeRepository
    {
        private SqlServerContext _context;
        private DbSet<Employee> dataset;
        public EmployeeRepositoryImplementation(SqlServerContext context)
        {
            _context = context;
            dataset = _context.Set<Employee>();
        }
        public Employee Create(Employee employee)
        {
            var pass = employee.Password;
            using var alg = SHA256.Create();
            employee.Password = ComputeHash(pass, alg);
            dataset.Add(employee);
            _context.SaveChanges();
            return employee;
        }

        public List<Employee> FindAll()
        {
            DateTime lastWeek = DateTime.Now.Date.AddDays(-7);
            var queryset = dataset
                            .Include(em => em.Sales
                                .Where(s => s.Date >= lastWeek))
                            .ToList();
            foreach (var employee in queryset)
            {
                foreach (var sale in employee.Sales)
                {
                    sale.Client = _context.Clients.SingleOrDefault(c =>
                    c.Id.Equals(sale.ClientId));
                }
            }

            return queryset;
        }

        public Employee FindById(Guid id)
        {
            DateTime lastWeek = DateTime.Now.Date.AddDays(-7);
            var queryset = dataset.Include(em => em.Sales
                                .Where(s => s.Date >= lastWeek))
                                .SingleOrDefault(e => e.Id.Equals(id));
            foreach (var sale in queryset.Sales)
            {
                sale.Client = _context.Clients.SingleOrDefault(c =>
                    c.Id.Equals(sale.ClientId));
            }

            return queryset;
        }

        public Employee Update(Employee employee)
        {
            var queryset = dataset.SingleOrDefault(p => p.Id.Equals(employee.Id));
            if (queryset != null)
            {
                dataset.Entry(queryset).CurrentValues.SetValues(employee);
                _context.SaveChanges();
                return queryset;
            }
            else
            {
                throw new Exception("Employee not found!");
            }
        }

        public void Delete(Guid id)
        {
            var queryset = dataset.SingleOrDefault(c => c.Id.Equals(id));
            if (queryset != null)
            {
                dataset.Remove(queryset);
                _context.SaveChanges();
            }
        }
        private string ComputeHash(string input, SHA256 alg)
        {
            Byte[] inputBytes = Encoding.UTF8.GetBytes(input);
            Byte[] hashedBytes = alg.ComputeHash(inputBytes);

            return BitConverter.ToString(hashedBytes);
        }
    }
}
