using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IntuitBackend.Services
{

    public interface IServiceCliente
    {
        Task<List<Cliente>> GetAll();
        Task<Cliente?> Get(int id);
        Task<List<Cliente>> Search(string valor);
        Task Insert(Cliente cliente);
        Task Update(Cliente oldClient, Cliente updatedClient);
    }

    public class ServiceCliente : IServiceCliente
    {
        private readonly IntuitDBContext _intuitDBContext;

        public ServiceCliente(IntuitDBContext intuitDBContext)
        {
            _intuitDBContext = intuitDBContext;
        }

        [HttpGet]
        [Route("GetAll")]
        public async Task<List<Cliente>> GetAll()
        {
            
            List<Cliente> clientList = new List<Cliente>();
            
            clientList = await _intuitDBContext.Clientes.ToListAsync();
            
            return clientList;
            
            
        }

        [HttpGet]
        [Route("Get")]
        public async Task<Cliente?> Get(int id)
        {
            Cliente? client;
            
            client = await _intuitDBContext.Clientes.FindAsync(id);
            
            return client;
        }

        [HttpGet]
        [Route("Search")]
        public async Task<List<Cliente>> Search(string valor)
        {
            List<Cliente> clientList = new List<Cliente>();

            clientList = await _intuitDBContext.Clientes
                .Where(c => (c.Nombres + " " + c.Apellidos).Contains(valor) || (c.Apellidos + " " + c.Nombres).Contains(valor))
                .ToListAsync();
            
            return clientList;
        }

        [HttpPost]
        [Route("Insert")]
        public async Task Insert(Cliente cliente)
        {
            await _intuitDBContext.Clientes.AddAsync(cliente);
            await _intuitDBContext.SaveChangesAsync();   
        }

        [HttpPut]
        [Route("Update")]
        public async Task Update(Cliente oldClient, Cliente updatedClient)
        {
            oldClient.Apellidos = updatedClient.Apellidos ?? oldClient.Apellidos;
            oldClient.Nombres = updatedClient.Nombres ?? oldClient.Nombres;
            oldClient.Email = updatedClient.Email ?? oldClient.Email;
            oldClient.TelefonoCelular = updatedClient.TelefonoCelular ?? oldClient.TelefonoCelular;
            oldClient.Cuit = updatedClient.Cuit ?? oldClient.Cuit;
            oldClient.Domicilio = updatedClient.Domicilio ?? oldClient.Domicilio;
            oldClient.FechaNacimiento = updatedClient.FechaNacimiento ?? oldClient.FechaNacimiento;

            await _intuitDBContext.SaveChangesAsync();
        }

    }
}
