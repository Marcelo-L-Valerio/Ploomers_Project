using Microsoft.EntityFrameworkCore;
using Ploomers_Project_API.Models.Context;
using Ploomers_Project_API.Models.Entities;
using System.Data;

namespace Ploomers_Project_API.Repository.Implementations
{
    public class ClientRepositoryImplementation : IClientRepository
    {
        private SqlServerContext _context;
        private DbSet<Client> dataset;
        public ClientRepositoryImplementation(SqlServerContext context)
        {
            _context = context;
            dataset = _context.Set<Client>();
        }

        public Client Create(Client client)
        {
            dataset.Add(client);
            _context.SaveChanges();
            return client;
        }

        public List<Client> FindAll()
        {
            DateTime lastWeek = DateTime.Now.Date.AddDays(-7);
            var queryset = dataset
                        .Include(co => co.Contacts)
                        .Include(co => co.Sales
                            .Where(s => s.Date >= lastWeek))
                        .ToList();
            foreach( var client in queryset )
            {
                foreach (var sale in client.Sales)
                {
                    sale.Employee = _context.Employees.SingleOrDefault(c =>
                        c.Id.Equals(sale.EmployeeId));
                }
            }

            return queryset;
        }

        public Client FindById(Guid id)
        {
            DateTime lastWeek = DateTime.Now.Date.AddDays(-7);
            var queryset = dataset
                .Include(co => co.Contacts)
                .Include(co => co.Sales
                    .Where(s => s.Date >= lastWeek))
                .SingleOrDefault(c => c.Id.Equals(id));

            foreach (var sale in queryset.Sales)
            {
                sale.Employee = _context.Employees.SingleOrDefault(c =>
                    c.Id.Equals(sale.EmployeeId));
            }

            return queryset;
        }

        public Client Update(Client client)
        {
            var queryset = dataset.SingleOrDefault(p => p.Id.Equals(client.Id));
            if (queryset != null)
            {
                dataset.Entry(queryset).CurrentValues.SetValues(client);
                _context.SaveChanges();
                return queryset;
            }
            else
            {
                throw new Exception("Client not found!");
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

        public void AddContact(Guid id, Contact contact)
        {
            var queryset = dataset.SingleOrDefault(p => p.Id.Equals(id));
            if (queryset != null)
            {
                contact.ClientId = id;
                contact.Client = queryset;
                _context.Contacts.Add(contact);
                _context.SaveChanges();
            }
        }

        public void DeleteContact(Guid idcli, Guid idcont)
        {
            var contact = _context.Contacts.SingleOrDefault(
                co => co.Id.Equals(idcont) && co.ClientId.Equals(idcli));
            if (contact != null)
            {
                _context.Contacts.Remove(contact);
                _context.SaveChanges();
            }
        }
    }
}
