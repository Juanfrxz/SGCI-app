using System;
using System.Collections.Generic;
using SGCI_app.domain.DTO;
using SGCI_app.domain.Ports;

namespace SGCI_app.application.services
{
    public class CompanyService
    {
        private readonly ICompanyRepository _repo;
        public CompanyService(ICompanyRepository repo)
        {
            _repo = repo;
        }

        public void MostrarTodos()
        {
            var lista = _repo.ObtenerTodos();
            foreach (var c in lista)
            {
                Console.WriteLine($"ID: {c.Id}, Nombre: {c.Nombre}, Fecha Registro: {c.FechaRegistro}");
            }
        }

        public void CrearCompany(DtoCompany company)
        {
            _repo.Crear(company);
        }

        public void EliminarCompany(string id)
        {
            _repo.Eliminar(id);
        }

        public void ActualizarCompany(DtoCompany company)
        {
            _repo.Actualizar(company);
        }
    }
} 