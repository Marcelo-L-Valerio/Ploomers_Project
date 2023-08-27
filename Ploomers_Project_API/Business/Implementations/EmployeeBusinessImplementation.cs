using AutoMapper;
using Ploomers_Project_API.Mappers.DTOs.InputModels;
using Ploomers_Project_API.Mappers.DTOs.ViewModels;
using Ploomers_Project_API.Models.Entities;
using Ploomers_Project_API.Repository;
using Ploomers_Project_API.TokenService;
using Ploomers_Project_API.TokenService.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.RegularExpressions;

namespace Ploomers_Project_API.Business.Implementations
{
    public class EmployeeBusinessImplementation : IEmployeeBusiness
    {
        private TokenConfiguration _configuration;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IMapper _mapper;
        private ITokenService _tokenService;
        public EmployeeBusinessImplementation(
            IEmployeeRepository repository, IMapper mapper,
            ITokenService tokenService, TokenConfiguration configuration)
        {
            _employeeRepository = repository;
            _mapper = mapper;
            _tokenService = tokenService;
            _configuration = configuration;
        }
        public EmployeeViewModel Create(EmployeeInputModel employee)
        {
            var mappedEmployee = _mapper.Map<Employee>(employee);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
                new Claim(JwtRegisteredClaimNames.UniqueName, mappedEmployee.Email)
            };

            var accessToken = _tokenService.GenerateAcessToken(claims);
            var refreshToken = _tokenService.GenerateRefreshToken();

            mappedEmployee.RefreshToken = refreshToken;
            mappedEmployee.RefreshTokenExpiryTime = DateTime.Now.AddDays(_configuration.DaysToExpiry);

            if (IsValid(employee))
            {
                var employeeEntity = _employeeRepository.Create(mappedEmployee);
                var viewModel = _mapper.Map<EmployeeViewModel>(employeeEntity);
                return viewModel;
            }
            return null;
        }

        public void Delete(Guid id)
        {
            _employeeRepository.Delete(id);
        }

        public List<EmployeeViewModel> FindAll()
        {
            var employees = _employeeRepository.FindAll();

            var viewModel = _mapper.Map<List<EmployeeViewModel>>(employees);

            viewModel.
                ForEach(employee =>
                {
                    employee.LastWeekTotal = employee.LastWeekSales
                    .Sum(sale => sale.Total);
                });
            return viewModel;
        }

        public EmployeeViewModel FindById(Guid id)
        {
            var result = _employeeRepository.FindById(id);

            var viewModel = _mapper.Map<EmployeeViewModel>(result);

            viewModel.LastWeekTotal = viewModel.LastWeekSales.Sum(sale => sale.Total);

            return viewModel;
        }

        public void Update(Guid id, EmployeeInputModel employeeData)
        {
            var mappedEmployee = _mapper.Map<Employee>(employeeData);
            mappedEmployee.Id = id;
            var employeeEntity = _employeeRepository.Update(mappedEmployee);
        }

        private bool IsValid(EmployeeInputModel employee)
        {
            if (employee.Password.Length < 8)
            {
                throw new Exception("Password minimum lenght is 8 digits!");
            }
            return true;
        }
    }
}
