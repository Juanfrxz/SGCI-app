using System;
using SGCI_app.domain.DTO;
using SGCI_app.domain.Entities;

namespace SGCI_app.domain.Ports
{
    public interface ICompanyRepository : IGenericRepository<DtoCompany>
    {
        public void Eliminar(string id);
    }
}