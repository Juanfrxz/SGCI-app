using System;
using SGCI_app.domain.DTO;
using SGCI_app.domain.Entities;

namespace SGCI_app.domain.Ports
{
    public interface IProviderRepository : IGenericRepository<DtoProvider>
    {
        void Actualizar(int id, DtoProvider entity);
    }
}