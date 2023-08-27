using Ploomers_Project_API.Models.Context;
using Ploomers_Project_API.Models.Entities;
using System.Security.Cryptography;
using System.Text;

namespace Ploomers_Project_API.Repository.Implementations
{
    public class LoginRepositoryImplementation : ILoginRepository
    {
        private readonly SqlServerContext _context;
        public LoginRepositoryImplementation(SqlServerContext context)
        {
            _context = context;
        }
        public Employee ValidateCredentials(Employee employee)
        {
            using var alg = SHA256.Create();
            var pass = ComputeHash(employee.Password, alg);
            var queryset = _context.Employees.FirstOrDefault(e =>
                            (e.Email == employee.Email) && (e.Password == pass));
            return queryset;
        }

        public Employee ValidateCredentials(string email)
        {
            return _context.Employees.SingleOrDefault(e => e.Email == email);
        }

        public bool RevokeToken(string email)
        {
            var user = _context.Employees.SingleOrDefault(e => e.Email == email);
            if (user is null) return false;

            user.RefreshToken = null;
            _context.SaveChanges();
            return true;
        }

        public Employee RefreshUserInfo(Employee employee)
        {
            var queryset = _context.Employees.SingleOrDefault(p => p.Id.Equals(employee.Id));
            if (queryset != null)
            {
                try
                {
                    _context.Employees.Entry(queryset).CurrentValues.SetValues(employee);
                    _context.SaveChanges();
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return queryset;
        }

        private string ComputeHash(string input, SHA256 alg)
        {
            Byte[] inputBytes = Encoding.UTF8.GetBytes(input);
            Byte[] hashedBytes = alg.ComputeHash(inputBytes);

            return BitConverter.ToString(hashedBytes);
        }
    }
}
