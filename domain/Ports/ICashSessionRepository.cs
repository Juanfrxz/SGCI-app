using System;
using SGCI_app.domain.Entities;

namespace SGCI_app.domain.Ports
{
    public interface ICashSessionRepository : IGenericRepository<CashSession>
    {
        void Cerrar (int id);
    }
}