using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ploomers_Project_API.Business;
using Ploomers_Project_API.Mappers.DTOs.InputModels;
using Ploomers_Project_API.Mappers.DTOs.ViewModels;
using Ploomers_Project_API.Models.Context;

namespace Ploomers_Project_API.Controllers
{
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/employees")]
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

        // GET api/v1/employees - Retrieves all employees' data
        [Authorize("Bearer")]
        [ProducesResponseType(200, Type = typeof(List<EmployeeViewModel>))]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [HttpGet]
        public IActionResult GetAll()
        {
            try
            {
                return Ok(_employeeBusiness.FindAll());
            }
            catch (Exception e)
            {
                if (e.InnerException != null)
                {
                    return BadRequest(e.InnerException.Message);
                }
                else
                {
                    return BadRequest(e.Message);
                }
            }
        }

        // GET api/v1/employees/{ID} - Retrieves an specific employees' data
        [Authorize("Bearer")]
        [ProducesResponseType(200, Type = typeof(EmployeeViewModel))]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [HttpGet("{id}")]
        public IActionResult GetById(Guid id)
        {
            try
            {
                var employee = _employeeBusiness.FindById(id);
                if (employee == null) return NotFound();

                return Ok(employee);
            }
            catch (Exception e)
            {
                if (e.InnerException != null)
                {
                    return BadRequest(e.InnerException.Message);
                }
                else
                {
                    return BadRequest(e.Message);
                }
            }
        }

        // POST api/v1/employees - Creates a new employee in the database, if valid
        // no authentication needed
        [ProducesResponseType((201), Type = typeof(EmployeeViewModel))]
        [ProducesResponseType(400)]
        [HttpPost]
        public IActionResult Post(EmployeeInputModel payload)
        {
            try
            {
                var employee = _employeeBusiness.Create(payload);
                return CreatedAtAction(nameof(GetById), new { id = employee.Id }, employee);
            }
            catch (Exception e)
            {
                if (e.InnerException != null)
                {
                    return BadRequest(e.InnerException.Message);
                }
                else
                {
                    return BadRequest(e.Message);
                }
            }
        }

        // PUT api/v1/employees/{ID} -  Updates an existing employee's data, if valid
        [Authorize("Bearer")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [HttpPut("{id}")]
        public IActionResult Put(Guid id, EmployeeInputModel payload)
        {
            try
            {
                _employeeBusiness.Update(id, payload);
                return NoContent();
            }
            catch (Exception e)
            {
                if (e.InnerException != null)
                {
                    return BadRequest(e.InnerException.Message);
                }
                else
                {
                    return BadRequest(e.Message);
                }
            }
        }

        // DELETE api/v1/employees/{ID} - Deletes an employee in the database
        [Authorize("Bearer")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            try
            {
                _employeeBusiness.Delete(id);
                return NoContent();
            }
            catch (Exception e)
            {
                if (e.InnerException != null)
                {
                    return BadRequest(e.InnerException.Message);
                }
                else
                {
                    return BadRequest(e.Message);
                }
            }
        }
    }
}
