using System;
using System.Collections.Generic;
using SGCI_app.domain.Entities;
using SGCI_app.domain.Ports;

namespace SGCI_app.application.Services
{
    public class ThirdPartyTypeService
    {
        private readonly IThirdPartyTypeRepository _repository;

        public ThirdPartyTypeService(IThirdPartyTypeRepository repository)
        {
            _repository = repository;
        }

        public void CrearThirdPartyType(ThirdPartyType entity)
        {
            _repository.Crear(entity);
        }

        public void ActualizarThirdPartyType(int id, ThirdPartyType entity)
        {
            entity.Id = id;
            _repository.Actualizar(entity);
        }

        public void EliminarThirdPartyType(int id)
        {
            _repository.Eliminar(id);
        }

        public void MostrarTodos()
        {
            var lista = _repository.ObtenerTodos();
            foreach (var c in lista)
            {
                Console.WriteLine($"ID: {c.Id}, Descripción: {c.Descripcion}");
            }
        }
    }
}