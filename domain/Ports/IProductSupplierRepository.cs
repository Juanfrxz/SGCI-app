using System;
using SGCI_app.domain.Entities;

namespace SGCI_app.domain.Ports
{
    public interface IProductSupplierRepository : IGenericRepository<ProductSupplier>
    {
        void Eliminar(string tercero_id, string producto_id);
        void Actualizar(string tercero_id, string producto_id, ProductSupplier Entity);
    }
}