using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SGCI_app.domain.DTO;
using SGCI_app.domain.Ports;

namespace SGCI_app.application.services
{
    public class EmployeeService
    {
        private readonly IEmployeeRepository _repo;
        public EmployeeService(IEmployeeRepository repo)
        {
            _repo = repo;
        }
        public void MostrarTodos()
        {
            var lista = _repo.ObtenerTodos();
            foreach (var c in lista)
            {
                Console.WriteLine($"ID: {c.Id}, Nombre: {c.Nombre}, ID Tercero  : {c.Tercero_Id}");
            }
        }
        public void CrearEmpleado(DtoEmployee demployee)
        {
            _repo.Crear(demployee);
        }
        public void EliminarEmpleado(int id)
        {
            _repo.Eliminar(id);
        }
        public void ActualizarDatosEmpleado (int id, DtoEmployee demployee)
        {
            _repo.Actualizar(id, demployee);
        }
    }
}