using AutoMapper;
using Ploomers_Project_API.Mappers.DTOs.InputModels;
using Ploomers_Project_API.Mappers.DTOs.ViewModels;
using Ploomers_Project_API.Models.Entities;
using Ploomers_Project_API.Repository;
using Ploomers_Project_API.Services.TokenService;
using Ploomers_Project_API.Services.TokenService.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Ploomers_Project_API.Business.Implementations
{
    public class EmployeeBusinessImplementation : IEmployeeBusiness
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IMapper _mapper;
        public EmployeeBusinessImplementation(IEmployeeRepository repository, IMapper mapper)
        {
            _employeeRepository = repository;
            _mapper = mapper;
        }

        public EmployeeViewModel Create(EmployeeInputModel employee)
        {
            var mappedEmployee = _mapper.Map<Employee>(employee);
            if (!IsValid(employee)) return null;

            var employeeEntity = _employeeRepository.Create(mappedEmployee);
            var viewModel = _mapper.Map<EmployeeViewModel>(employeeEntity);
            return viewModel;
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
            _employeeRepository.Update(mappedEmployee);
        }

        public void Delete(Guid id)
        {
            _employeeRepository.Delete(id);
        }

        // Employee Input data extra validation
        private bool IsValid(EmployeeInputModel employee)
        {
            // Password validation
            if (employee.Password.Length < 8)
            {
                throw new Exception("Password minimum lenght is 8 digits!");
            }
            return true;
        }
    }
}
