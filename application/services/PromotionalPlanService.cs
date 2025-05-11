using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SGCI_app.domain.Entities;
using SGCI_app.domain.Ports;

namespace SGCI_app.application.services
{
    public class PromotionalPlanService
{
    private readonly IPromotionalPlanRepository _promotionalPlanRepository;

    public PromotionalPlanService(IPromotionalPlanRepository promotionalPlanRepository)
    {
        _promotionalPlanRepository = promotionalPlanRepository;
    }

    public void CreatePromotionalPlan(PromotionalPlan plan)
    {
        _promotionalPlanRepository.Crear(plan);
    }

    public void UpdatePromotionalPlan(PromotionalPlan plan)
    {
        _promotionalPlanRepository.Actualizar(plan);
    }

    public void DeletePromotionalPlan(int id)
    {
        _promotionalPlanRepository.Eliminar(id);
    }

    public void GetAllPromotionalPlans()
    {
        var lista = _promotionalPlanRepository.ObtenerTodos();
            foreach (var c in lista)
            {
                Console.WriteLine($"ID: {c.Id}, Nombre: {c.Nombre}, Descuento: {c.Descuento}%, Fecha_Inicio: {c.Inicio}, Fecha_Fin: {c.Fin}");
            }
    }
}

}