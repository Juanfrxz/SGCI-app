using System;
using System.Collections.Generic;
using SGCI_app.domain.Entities;
using SGCI_app.domain.Ports;

namespace SGCI_app.application.Services
{
    public class CashMovementService
    {
        private readonly ICashMovementRepository _repository;

        public CashMovementService(ICashMovementRepository repository)
        {
            _repository = repository;
        }

        public void CrearMovimiento(CashMovement movimiento)
        {
            if (movimiento.Valor <= 0)
                throw new ArgumentException("El valor del movimiento debe ser mayor que 0");

            _repository.Crear(movimiento);
        }

        public void ActualizarMovimiento(CashMovement movimiento)
        {
            if (movimiento.Valor <= 0)
                throw new ArgumentException("El valor del movimiento debe ser mayor que 0");

            _repository.Actualizar(movimiento);
        }

        public void EliminarMovimiento(int id)
        {
            _repository.Eliminar(id);
        }

        public IEnumerable<CashMovement> ObtenerTodos()
        {
            return _repository.ObtenerTodos();
        }

        public IEnumerable<CashMovement> ObtenerPorSesion(int sesionId)
        {
            return _repository.ObtenerPorSesion(sesionId);
        }

        public IEnumerable<CashMovement> ObtenerPorFecha(DateTime fecha)
        {
            return _repository.ObtenerPorFecha(fecha);
        }

        public IEnumerable<CashMovement> ObtenerPorTipo(int tipoId)
        {
            return _repository.ObtenerPorTipo(tipoId);
        }

        public IEnumerable<CashMovement> ObtenerPorTercero(string terceroId)
        {
            return _repository.ObtenerPorTercero(terceroId);
        }

        /// <summary>
        /// Muestra todos los movimientos de caja en consola.
        /// </summary>
        public void MostrarTodos()
        {
            var lista = _repository.ObtenerTodos();
            foreach (var m in lista)
            {
                Console.WriteLine(
                    $"ID: {m.Id}, Fecha: {m.Fecha.ToShortDateString()}, Tipo ID: {m.TipoMovimiento_Id}, " +
                    $"Valor: {m.Valor}, Concepto: {(string.IsNullOrEmpty(m.Concepto) ? "--" : m.Concepto)}, " +
                    $"Tercero ID: {(string.IsNullOrEmpty(m.Tercero_Id) ? "--" : m.Tercero_Id)}, Sesión ID: {m.Sesion_Id}");
            }
        }
    }
}