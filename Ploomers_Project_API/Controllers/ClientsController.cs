using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ploomers_Project_API.Business;
using Ploomers_Project_API.DTOs.ViewModels;
using Ploomers_Project_API.Mappers.DTOs.InputModels;
using Ploomers_Project_API.Models.Context;

namespace Ploomers_Project_API.Controllers
{
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/clients")]
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

        // GET api/v1/clients - Retrieves all clients' data
        [ProducesResponseType((200), Type = typeof(List<ClientViewModel>))]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [HttpGet]
        public IActionResult GetAll()
        {
            try
            {
                return Ok(_clientBusiness.FindAll());
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

        // GET api/v1/clients/{ID} - Retrieves an specific's client data
        [ProducesResponseType((200), Type = typeof(ClientViewModel))]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        [HttpGet("{id}")]
        public IActionResult GetById(Guid id)
        {
            try
            {
                var client = _clientBusiness.FindById(id);
                if (client == null) return NotFound();

                return Ok(client);
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

        // POST api/v1/clients - Creates a new client in the database, if valid
        [ProducesResponseType((201), Type = typeof(ClientViewModel))]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [HttpPost]
        public IActionResult Post(ClientInputModel payload)
        {
            try
            {
                var client = _clientBusiness.Create(payload);
                return CreatedAtAction(nameof(GetById), new { id = client.Id }, client);
            }
            catch (Exception e)
            {
                if(e.InnerException != null)
                {
                    return BadRequest(e.InnerException.Message);
                }
                else
                {
                    return BadRequest(e.Message);
                }
            }
        }

        // PUT api/v1/clients/{ID} - Updates an existing client's data, if valid
        [ProducesResponseType((204))]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [HttpPut("{id}")]
        public IActionResult Put(Guid id, ClientInputModel payload)
        {
            try
            {
                _clientBusiness.Update(id, payload);
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

        // DELETE api/v1/clients - Deletes a client in the database
        [ProducesResponseType((204))]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            try
            {
                _clientBusiness.Delete(id);
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

        // POST api/v1/clients/{ID}/contacts - Creates a new contact attached to the
        // user with the URL ID in the database
        [ProducesResponseType((204))]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [HttpPost("{id}/contacts")]
        public IActionResult PostContact(Guid id, ContactInputModel payload)
        {
            try
            {
                _clientBusiness.AddContact(id, payload);
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

        // DELETE api/v1/clients/{ID}/contacts/{ID} - Deletes an existing contact info attached
        // to the user with the URL ID in the database
        [ProducesResponseType((204))]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [HttpDelete("{idcli}/contacts/{idcont}")]
        public IActionResult DeleteContact(Guid idcli, Guid idcont)
        {
            try
            {
                _clientBusiness.DeleteContact(idcli, idcont);
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
