using System;
using SGCI_app.domain.DTO;
using SGCI_app.domain.Entities;

namespace SGCI_app.domain.Ports
{
    public interface IEmployeeRepository : IGenericRepository<DtoEmployee>
    {
        void Actualizar(int id, DtoEmployee entity);
    }
}