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
            var data = dataset
                        .Include(co => co.Contacts)
                        .Include(co => co.Sales
                            .Where(s => s.Date >= lastWeek))
                        .ToList();
            return data;
        }

        public Client FindById(Guid id)
        {
            DateTime lastWeek = DateTime.Now.Date.AddDays(-7);
            return dataset
                .Include(co => co.Contacts)
                .Include(co => co.Sales
                    .Where(s => s.Date >= lastWeek))
                .SingleOrDefault(c => c.Id.Equals(id));
        }

        public Client Update(Client client)
        {
            var result = dataset.SingleOrDefault(p => p.Id.Equals(client.Id));
            if (result != null)
            {
                dataset.Entry(result).CurrentValues.SetValues(client);
                _context.SaveChanges();
                return result;
            }
            else
            {
                throw new Exception("Client not found!");
            }
        }

        public void Delete(Guid id)
        {
            var result = dataset.SingleOrDefault(c => c.Id.Equals(id));
            if (result != null)
            {
                dataset.Remove(result);
                _context.SaveChanges();
            }
        }

        public void AddContact(Guid id, Contact contact)
        {
            var result = dataset.SingleOrDefault(p => p.Id.Equals(id));
            if (result != null)
            {
                contact.ClientId = id;
                contact.Client = result;
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
