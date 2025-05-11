using System;
using System.Collections.Generic;
using SGCI_app.domain.Entities;
using SGCI_app.domain.Ports;

namespace SGCI_app.application.Services
{
    public class CashMovementTypeService
    {
        private readonly ICashMovementTypeRepository _repository;

        public CashMovementTypeService(ICashMovementTypeRepository repository)
        {
            _repository = repository;
        }

        public void CrearCashMovementType(CashMovementType entity)
        {
            _repository.Crear(entity);
        }

        public void ActualizarCashMovementType(int id, CashMovementType entity)
        {
            entity.Id = id;
            _repository.Actualizar(entity);
        }

        public void EliminarCashMovementType(int id)
        {
            _repository.Eliminar(id);
        }

        public void MostrarTodos()
        {
            var lista = _repository.ObtenerTodos();
            foreach (var item in lista)
            {
                Console.WriteLine($"ID: {item.Id}, Nombre: {item.Nombre}, Tipo: {item.Tipo}");
            }
        }
    }
}