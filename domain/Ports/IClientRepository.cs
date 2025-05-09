using System;
using SGCI_app.domain.DTO;
using SGCI_app.domain.Entities;

namespace SGCI_app.domain.Ports
{
    public interface IClientRepository : IGenericRepository<DtoClient>
    {
        void Actualizar(int id, DtoClient entity);
    }
}