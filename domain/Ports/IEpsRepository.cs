using System;
using SGCI_app.domain.Entities;

namespace SGCI_app.domain.Ports
{
    public interface IEpsRepository : IGenericRepository<EPS>
    {
        void Actualizar(int id, EPS entity);
    }
}