using System;
using SGCI_app.domain.Entities;

namespace SGCI_app.domain.Ports
{
    public interface IDocTypeRepository : IGenericRepository<DocType>
    {
        void Actualizar(int id, DocType entity);
    }
}