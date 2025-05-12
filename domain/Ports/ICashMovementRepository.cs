using System;
using System.Collections.Generic;
using SGCI_app.domain.Entities;

namespace SGCI_app.domain.Ports
{
    public interface ICashMovementRepository : IGenericRepository<CashMovement>
    {
        IEnumerable<CashMovement> ObtenerPorSesion(int sesionId);
        IEnumerable<CashMovement> ObtenerPorFecha(DateTime fecha);
        IEnumerable<CashMovement> ObtenerPorTipo(int tipoId);
        IEnumerable<CashMovement> ObtenerPorTercero(string terceroId);
    }
}