using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Ploomers_Project_API.Business;
using Ploomers_Project_API.Mappers.DTOs.InputModels;
using Ploomers_Project_API.Models.Context;

namespace Ploomers_Project_API.Controllers
{
    [ApiVersion("1")]
    [Route("api/employees/v{version:apiVersion}")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly SqlServerContext _context;
        private IEmployeeBusiness _employeeBusiness;

        public EmployeesController(SqlServerContext context, IEmployeeBusiness employeeBusiness)
        {
            _context = context;
            _employeeBusiness = employeeBusiness;
        }

        [Authorize("Bearer")]
        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_employeeBusiness.FindAll());
        }

        [Authorize("Bearer")]
        [HttpGet("{id}")]
        public IActionResult GetById(Guid id)
        {
            var client = _employeeBusiness.FindById(id);
            if (client == null) return NotFound();

            return Ok(client);
        }

        [HttpPost]
        public IActionResult Post(EmployeeInputModel payload)
        {
            var employee = _employeeBusiness.Create(payload);
            return CreatedAtAction(nameof(GetById), new { id = employee.Id }, employee);
        }

        [Authorize("Bearer")]
        [HttpPut("{id}")]
        public IActionResult Put(Guid id, EmployeeInputModel payload)
        {
            _employeeBusiness.Update(id, payload);
            return NoContent();
        }

        [Authorize("Bearer")]
        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            _employeeBusiness.Delete(id);
            return NoContent();
        }
    }
}
