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