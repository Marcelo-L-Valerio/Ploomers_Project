using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ploomers_Project_API.Business;
using Ploomers_Project_API.Mappers.DTOs.InputModels;
using Ploomers_Project_API.Models.Context;

namespace Ploomers_Project_API.Controllers
{
    [ApiVersion("1")]
    [Route("api/clients/v{version:apiVersion}")]
    [ApiController]
    [Authorize("Bearer")]
    public class ClientsController : ControllerBase
    {
        private readonly SqlServerContext _context;
        private IClientBusiness _clientBusiness;

        public ClientsController(SqlServerContext context, IClientBusiness clientBusiness)
        {
            _context = context;
            _clientBusiness = clientBusiness;
        }


        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_clientBusiness.FindAll());
        }

        [HttpGet("{id}")]
        public IActionResult GetById(Guid id)
        {
            var client = _clientBusiness.FindById(id);
            if (client == null) return NotFound();

            return Ok(client);
        }

        [HttpPost]
        public IActionResult Post(ClientInputModel payload)
        {
            var client = _clientBusiness.Create(payload);
            return CreatedAtAction(nameof(GetById), new { id = client.Id }, client);
        }

        [HttpPut("{id}")]
        public IActionResult Put(Guid id, ClientInputModel payload)
        {
            _clientBusiness.Update(id, payload);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            _clientBusiness.Delete(id);
            return NoContent();
        }

        [HttpPost("{id}/contacts")]
        public IActionResult PostContact(Guid id, ContactInputModel payload)
        {
            _clientBusiness.AddContact(id, payload);
            return NoContent();
        }

        [HttpDelete("{idcli}/contacts/{idcont}")]
        public IActionResult DeleteContact(Guid idcli, Guid idcont)
        {
            _clientBusiness.DeleteContact(idcli, idcont);
            return NoContent();
        }
    }
}
