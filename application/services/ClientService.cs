using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SGCI_app.domain.DTO;
using SGCI_app.domain.Ports;

namespace SGCI_app.application.services
{
    public class ClientService
    {
        private readonly IClientRepository _repo;
        public ClientService(IClientRepository repo)
        {
            _repo = repo;
        }
        public void MostrarTodos()
        {
            var lista = _repo.ObtenerTodos();
            foreach (var c in lista)
            {
                Console.WriteLine($"ID: {c.Id}, Nombre: {c.Nombre}, ID Tercero: {c.Tercero_Id}");
            }
        }
        public void CrearClienteDto(DtoClient dtocliente)
        {
            _repo.Crear(dtocliente);
        }
        public void EliminarCliente(int id)
        {
            _repo.Eliminar(id);
        }
        public void ActualizarDatosPersonalesCliente (int id, DtoClient dtocliente)
        {
            _repo.Actualizar(id, dtocliente);
        }
    }
}