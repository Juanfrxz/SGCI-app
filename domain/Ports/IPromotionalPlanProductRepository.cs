using System;
using SGCI_app.domain.Entities;

namespace SGCI_app.domain.Ports
{
    public interface IPromotionalPlanProductRepository : IGenericRepository<PromotionalPlanProduct>
    {
        void Eliminar(int planId, string productoId);
        void Actualizar(int planId, string productoId, PromotionalPlanProduct Entity);
    }
}