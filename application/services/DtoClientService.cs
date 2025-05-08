using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Org.BouncyCastle.Asn1.Misc;
using SGCI_app.domain.DTO;
using SGCI_app.domain.Ports;

namespace SGCI_app.application.services
{
    public class DtoClientService
    {
        private readonly IDtoClientRepository _repo;
        public DtoClientService(IDtoClientRepository repo)
        {
            _repo = repo;
        }
        public void MostrarTodos()
        {
            var lista = _repo.ObtenerTodos();
            foreach (var c in lista)
            {
                Console.WriteLine($"ID: {c.Id}, Nombre: {c.Nombre}");
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