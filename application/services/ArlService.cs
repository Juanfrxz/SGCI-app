using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SGCI_app.domain.Entities;
using SGCI_app.domain.Ports;

namespace SGCI_app.application.services
{
    public class ArlService
    {
        private readonly IArlRepository _repo;
        public ArlService(IArlRepository repo)
        {
            _repo = repo;
        }
        public void MostrarTodos()
        {
            var lista = _repo.ObtenerTodos();
            foreach (var arl in lista)
            {
                Console.WriteLine($"ID: {arl.Id}, Nombre: {arl.Nombre}");
            }
        }
        public void CrearArl(ARL arl)
        {
            _repo.Crear(arl);
        }
        public void EliminarArl(int id)
        {
            _repo.Eliminar(id);
        }
        public void ActualizarArl(int id, ARL arl)
        {
            _repo.Actualizar(id, arl);
        }
    }
} 