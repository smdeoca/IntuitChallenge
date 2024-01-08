using IntuitBackend.Models;
using IntuitBackend.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace IntuitBackend.Controllers
{
    [ApiController]
    [Route("cliente")]
    public class ClienteController : ControllerBase
    {
        private readonly IServiceCliente serviceCliente;

        public ClienteController(IServiceCliente serviceCliente)
        {
            this.serviceCliente = serviceCliente;
        }

        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                return Ok(await serviceCliente.GetAll());
            }
            catch (Exception ex)
            {

                return StatusCode(500, "Error interno del servidor: " + ex.Message);
            }
        }

        [HttpGet]
        [Route("Get")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var client = await serviceCliente.Get(id);

                return client != null 
                    ? Ok(client)
                    : NotFound("El cliente no fue encontrado.");
            }
            catch (Exception ex)
            {

                return StatusCode(500, "Error interno del servidor: " + ex.Message);
            }
        }

        [HttpGet]
        [Route("Search")]
        public async Task<IActionResult> Search(string valor)
        {
            try
            {
                List<Cliente> clientList = await serviceCliente.Search(valor);

                return clientList.Count() > 0
                    ? Ok(clientList)
                    : NotFound("No se encontraron clientes.");
            }
            catch (Exception ex)
            {

                return StatusCode(500, "Error interno del servidor: " + ex.Message);
            }
        }

        [HttpPost]
        [Route("Insert")]
        public async Task<IActionResult> Insert(Cliente client)
        {
            try
            {
                if(client == null || !ModelState.IsValid)
                {
                    return BadRequest("Datos de cliente no válidos.");
                }
                
                await serviceCliente.Insert(client);
                
                return Ok("El registro se ha creado con éxito.");
            }
            catch (Exception ex)
            {

                return StatusCode(500, "Error interno del servidor: " + ex.Message);
            }
        }

        [HttpPut]
        [Route("Update")]
        public async Task<IActionResult> Update(int id, Cliente updatedClient)
        {
            try
            {
                if (updatedClient == null || !ModelState.IsValid)
                {
                    return BadRequest("Datos de cliente no válidos.");
                }

                Cliente client = null;

                client = await serviceCliente.Get(id);
                    
                

                if(client == null)
                {
                    return NotFound("El cliente no fue encontrado.");
                }

                await serviceCliente.Update(client, updatedClient);
               
                return Ok("El registro se ha actualizado con éxito.");
            }
            catch (Exception ex)
            {

                return StatusCode(500, "Error interno del servidor: " + ex.Message);
            }
        }
    }
}
