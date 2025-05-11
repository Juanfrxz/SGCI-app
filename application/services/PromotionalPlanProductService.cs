using System;
using System.Collections.Generic;
using SGCI_app.domain.Entities;
using SGCI_app.domain.Ports;

namespace SGCI_app.application.services
{
    public class PromotionalPlanProductService
    {
        private readonly IPromotionalPlanProductRepository _repository;

        public PromotionalPlanProductService(IPromotionalPlanProductRepository repository)
        {
            _repository = repository;
        }

        public void CreatePlanProduct(PromotionalPlanProduct entity)
        {
            _repository.Crear(entity);
        }

        public void UpdatePlanProduct(int planId, string productoId, PromotionalPlanProduct entity)
        {
            _repository.Actualizar(planId, productoId, entity);
        }

        public void DeletePlanProduct(int planId, string productoId)
        {
            _repository.Eliminar(planId, productoId);
        }

        public void GetAllPlanProducts()
        {
            var list = _repository.ObtenerTodos();
            foreach (var item in list)
            {
                Console.WriteLine($"Plan ID: {item.Plan_Id}, Producto ID: {item.Producto_Id}");
            }
        }
    }
}
