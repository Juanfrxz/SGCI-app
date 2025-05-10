
using SGCI_app.domain.Entities;

namespace SGCI_app.domain.Ports
{
    public interface IArlRepository : IGenericRepository<ARL>
    {
        void Actualizar(int id, ARL entity);
    }
}