using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SGCI_app.domain.DTO;

namespace SGCI_app.domain.Ports
{
    public interface IDtoEmployeeRepository : IGenericRepository<DtoEmployee>
    {
        void Actualizar(int id, DtoEmployee entity);
    }
}