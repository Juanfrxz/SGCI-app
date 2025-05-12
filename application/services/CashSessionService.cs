// CashSessionService.cs
using System;
using System.Collections.Generic;
using SGCI_app.domain.Entities;
using SGCI_app.domain.Ports;

namespace SGCI_app.application.Services
{
    public class CashSessionService
    {
        private readonly ICashSessionRepository _repository;

        public CashSessionService(ICashSessionRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Crea una nueva sesión de caja con la fecha y balance de apertura actuales.
        /// </summary>
        public void CrearCashSession(int balanceApertura)
        {
            var session = new CashSession
            {
                AperturaCaja = DateTime.Now,
                BalanceApertura = balanceApertura,
                // balance_cierre quedará en null hasta que se cierre la sesión
            };
            _repository.Crear(session);
        }

        /// <summary>
        /// Cierra la sesión de caja estableciendo la columna 'cerrado' con NOW().
        /// </summary>
        public void CerrarCashSession(int id)
        {
            _repository.Cerrar(id);
        }

        /// <summary>
        /// Actualiza el balance de cierre de una sesión ya cerrada.
        /// </summary>
        public void ActualizarBalanceCierre(int id, int balanceCierre)
        {
            var session = new CashSession
            {
                Id = id,
                BalaceCierre = balanceCierre,
                // opcionalmente podríamos actualizar CierreCaja si queremos sobreescribirla
                CierreCaja = DateTime.Now
            };
            _repository.Actualizar(session);
        }

        /// <summary>
        /// Elimina una sesión de caja por su ID.
        /// </summary>
        public void EliminarCashSession(int id)
        {
            _repository.Eliminar(id);
        }

        /// <summary>
        /// Devuelve todas las sesiones de caja.
        /// </summary>
        public IEnumerable<CashSession> ObtenerTodasLasSessions()
        {
            return _repository.ObtenerTodos();
        }
    }
}
