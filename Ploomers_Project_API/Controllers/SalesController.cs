using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Ploomers_Project_API.Business;
using Ploomers_Project_API.Mappers.DTOs.InputModels;
using Ploomers_Project_API.Models.Context;

namespace Ploomers_Project_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalesController : ControllerBase
    {
        private readonly SqlServerContext _context;
        private ISaleBusiness _saleBusiness;

        public SalesController(SqlServerContext context, ISaleBusiness saleBusiness)
        {
            _context = context;
            _saleBusiness = saleBusiness;
        }


        [HttpGet("client/{client_id}")]
        public IActionResult GetClientSales(Guid client_id)
        {
            return Ok(_saleBusiness.FindOneClientSales(client_id));
        }

        [HttpGet("{date}")]
        public IActionResult GetDailySales(DateOnly date)
        {
            return Ok(_saleBusiness.FindTodaySales(date));
        }

        [HttpPost("client/{client_id}")]
        public IActionResult PostSale(Guid client_id, SaleInputModel payload)
        {
            var sale = _saleBusiness.Create(client_id, payload);
            return Ok(sale);
        }

        [HttpPut("{sale_id}")]
        public IActionResult Put(Guid sale_id, SaleInputModel payload)
        {
            _saleBusiness.Update(sale_id, payload);
            return NoContent();
        }

        [HttpDelete("{sale_id}")]
        public IActionResult Delete(Guid sale_id)
        {
            _saleBusiness.Delete(sale_id);
            return NoContent();
        }
    }
}
