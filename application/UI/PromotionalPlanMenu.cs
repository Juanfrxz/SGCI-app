using System;
using SGCI_app.application.services;
using SGCI_app.domain.Entities;
using SGCI_app.domain.Factory;
using SGCI_app.infrastructure.postgres;

namespace SGCI_app.application.UI
{
    public class PromotionalPlanMenu : BaseMenu
    {
        private readonly PromotionalPlanService _service;

        public PromotionalPlanMenu() : base(showIntro: false)
        {
            string connStr = "Host=localhost;database=sgci;Port=5432;Username=postgres;Password=1219;Pooling=true";
            var factory = new ConexDBFactory(connStr);
            _service = new PromotionalPlanService(factory.CrearPromoPlanRepository());
        }

        public override void ShowMenu()
        {
            while (true)
            {
                ShowHeader("GESTIÓN DE PLANES PROMOCIONALES");
                Console.WriteLine("1. Crear Plan Promocional");
                Console.WriteLine("2. Listar Planes Promocionales");
                Console.WriteLine("3. Actualizar Plan Promocional");
                Console.WriteLine("4. Eliminar Plan Promocional");
                Console.WriteLine("0. Volver al menú principal");
                DrawSeparator();

                int option = GetValidatedIntInput("Seleccione una opción: ", 0, 4);
                switch (option)
                {
                    case 1:
                        CrearPromotionalPlan();
                        break;
                    case 2:
                        ListarPromotionalPlans();
                        break;
                    case 3:
                        ActualizarPromotionalPlan();
                        break;
                    case 4:
                        EliminarPromotionalPlan();
                        break;
                    case 0:
                        return;
                }
            }
        }

        private void CrearPromotionalPlan()
        {
            ShowHeader("CREAR PLAN PROMOCIONAL");
            var plan = new PromotionalPlan();

            plan.Id = GetValidatedIntInput("ID del plan: ");
            plan.Nombre = GetValidatedInput("Nombre del plan: ");
            plan.Datos_Extra = GetValidatedInput("Descripción: ");
            plan.Descuento = GetValidatedIntInput("Descuento (%): ");
            plan.Inicio = GetValidatedDateInput("Fecha de inicio (YYYY-MM-DD): ");
            plan.Fin = GetValidatedDateInput("Fecha de fin (YYYY-MM-DD): ");

            try
            {
                _service.CreatePromotionalPlan(plan);
                ShowSuccessMessage("Plan promocional creado exitosamente.");
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Error al crear el plan promocional: {ex.Message}");
            }
        }

        private void ListarPromotionalPlans()
        {
            ShowHeader("LISTA DE PLANES PROMOCIONALES");
            try
            {
                _service.GetAllPromotionalPlans();
                ShowInfoMessage("Listado de planes promocionales completado.");
                Console.WriteLine();
                Console.Write("Presione cualquier tecla para continuar...");
                Console.ReadKey(true);
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Error al obtener los planes promocionales: {ex.Message}");
            }
        }

        private void ActualizarPromotionalPlan()
        {
            ShowHeader("ACTUALIZAR PLAN PROMOCIONAL");
            int id = GetValidatedIntInput("ID del plan a actualizar: ");
            var plan = new PromotionalPlan { Id = id };

            plan.Nombre = GetValidatedInput("Nuevo nombre (dejar en blanco para mantener el actual): ", allowEmpty: true);
            plan.Datos_Extra = GetValidatedInput("Nueva descripción (dejar en blanco para mantener la actual): ", allowEmpty: true);
            
            string descuentoInput = GetValidatedInput("Nuevo descuento % (dejar en blanco para mantener el actual): ", allowEmpty: true);
            if (int.TryParse(descuentoInput, out int descuento)) plan.Descuento = descuento;

            string fechaInicioInput = GetValidatedInput("Nueva fecha de inicio (YYYY-MM-DD, dejar en blanco para mantener la actual): ", allowEmpty: true);
            if (!string.IsNullOrEmpty(fechaInicioInput) && DateTime.TryParse(fechaInicioInput, out DateTime fechaInicio))
                plan.Inicio = fechaInicio;

            string fechaFinInput = GetValidatedInput("Nueva fecha de fin (YYYY-MM-DD, dejar en blanco para mantener la actual): ", allowEmpty: true);
            if (!string.IsNullOrEmpty(fechaFinInput) && DateTime.TryParse(fechaFinInput, out DateTime fechaFin))
                plan.Fin = fechaFin;

            try
            {
                _service.UpdatePromotionalPlan(plan);
                ShowSuccessMessage("Plan promocional actualizado exitosamente.");
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Error al actualizar el plan promocional: {ex.Message}");
            }
        }

        private void EliminarPromotionalPlan()
        {
            ShowHeader("ELIMINAR PLAN PROMOCIONAL");
            int id = GetValidatedIntInput("ID del plan a eliminar: ");

            try
            {
                _service.DeletePromotionalPlan(id);
                ShowSuccessMessage("Plan promocional eliminado exitosamente.");
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Error al eliminar el plan promocional: {ex.Message}");
            }
        }

        private DateTime GetValidatedDateInput(string prompt)
        {
            while (true)
            {
                string input = GetValidatedInput(prompt);
                if (DateTime.TryParse(input, out DateTime date))
                    return date;
                ShowErrorMessage("Formato de fecha inválido. Use YYYY-MM-DD.");
            }
        }
    }
}