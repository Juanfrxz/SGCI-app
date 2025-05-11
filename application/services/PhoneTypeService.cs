using System;
using System.Collections.Generic;
using SGCI_app.domain.Entities;
using SGCI_app.domain.Ports;

namespace SGCI_app.application.Services
{
    public class PhoneTypeService
    {
        private readonly IPhoneTypeRepository _repository;

        public PhoneTypeService(IPhoneTypeRepository repository)
        {
            _repository = repository;
        }

        public void CrearPhoneType(PhoneType entity)
        {
            _repository.Crear(entity);
        }

        public void ActualizarPhoneType(int id, PhoneType entity)
        {
            entity.Id = id;
            _repository.Actualizar(entity);
        }

        public void EliminarPhoneType(int id)
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