using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ploomers_Project_API.Business;
using Ploomers_Project_API.Mappers.DTOs.InputModels;
using Ploomers_Project_API.Mappers.DTOs.ViewModels;
using Ploomers_Project_API.Models.Context;

namespace Ploomers_Project_API.Controllers
{
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/sales")]
    [ApiController]
    [Authorize("Bearer")]
    public class SalesController : ControllerBase
    {
        private readonly SqlServerContext _context;
        private ISaleBusiness _saleBusiness;

        public SalesController(SqlServerContext context, ISaleBusiness saleBusiness)
        {
            _context = context;
            _saleBusiness = saleBusiness;
        }

        // GET api/v1/sales/client/{Client ID} - Retrieves all Client's related sales
        // according to client's ID
        [ProducesResponseType((200), Type = typeof(List<SaleViewModel>))]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        [HttpGet("client/{client_id}")]
        public IActionResult GetClientSales(Guid client_id)
        {
            try
            {
                return Ok(_saleBusiness.FindOneClientSales(client_id));
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

        // GET api/v1/sales/employee/{Employee ID} - Retrieves all Employee's related sales
        // according to employee's ID
        [ProducesResponseType((200), Type = typeof(List<SaleViewModel>))]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        [HttpGet("employee/{employee_id}")]
        public IActionResult GetEmployeeSales(Guid employee_id)
        {
            try
            {
                return Ok(_saleBusiness.FindOneEmployeeSales(employee_id));
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

        // GET api/v1/sales/{YYYY-MM-DD} - Retrieves all sales of the specified date
        [ProducesResponseType((200), Type = typeof(List<SaleViewModel>))]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        [HttpGet("{date}")]
        public IActionResult GetDailySales(DateOnly date)
        {
            try
            {
                return Ok(_saleBusiness.FindTodaySales(date));
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

        // POST api/v1/sales/client/{Client ID} - Creates a new sale related to the client
        // specified, and the employee that made the request
        [ProducesResponseType((201), Type = typeof(SaleViewModel))]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [HttpPost("client/{client_id}")]
        public IActionResult Post(Guid client_id, SaleInputModel payload)
        {
            try
            {
                var employeeEmail = User.Identity.Name;
                var sale = _saleBusiness.Create(payload, client_id, employeeEmail);
                return Ok(sale);
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

        // PUT api/v1/sales/{Sale ID} - Updates a sale data. Uses the ID, so you have to 
        // reach it from the employee or client's view
        [ProducesResponseType((204))]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [HttpPut("{sale_id}")]
        public IActionResult Put(Guid sale_id, SaleInputModel payload)
        {
            try
            {
                _saleBusiness.Update(sale_id, payload);
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

        // DELETE api/v1/sales/{Sale ID} - Deletes a sale data. Uses the ID, so you have to 
        // reach it from the employee or client's view
        [ProducesResponseType((204))]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [HttpDelete("{sale_id}")]
        public IActionResult Delete(Guid sale_id)
        {
            try
            {
                _saleBusiness.Delete(sale_id);
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
