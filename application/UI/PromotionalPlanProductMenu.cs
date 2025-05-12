using System;
using SGCI_app.application.services;
using SGCI_app.domain.Entities;
using SGCI_app.domain.Factory;
using SGCI_app.infrastructure.postgres;

namespace SGCI_app.application.UI
{
    public class PromotionalPlanProductMenu : BaseMenu
    {
        private readonly PromotionalPlanProductService _service;

        public PromotionalPlanProductMenu() : base(showIntro: false)
        {
            string connStr = "Host=localhost;database=sgci;Port=5432;Username=postgres;Password=1219;Pooling=true";
            var factory = new ConexDBFactory(connStr);
            _service = new PromotionalPlanProductService(factory.CrearPromoPlanProdRepository());
        }

        public override void ShowMenu()
        {
            while (true)
            {
                ShowHeader("GESTIÓN DE ASOCIACIONES PLAN-PRODUCTO");
                Console.WriteLine("1. Crear Asociación");
                Console.WriteLine("2. Listar Asociaciones");
                Console.WriteLine("3. Actualizar Asociación");
                Console.WriteLine("4. Eliminar Asociación");
                Console.WriteLine("0. Volver al menú principal");
                DrawSeparator();

                int option = GetValidatedIntInput("Seleccione una opción: ", 0, 4);
                switch (option)
                {
                    case 1:
                        CrearAsociacion();
                        break;
                    case 2:
                        ListarAsociaciones();
                        break;
                    case 3:
                        ActualizarAsociacion();
                        break;
                    case 4:
                        EliminarAsociacion();
                        break;
                    case 0:
                        return;
                }
            }
        }

        private void CrearAsociacion()
        {
            ShowHeader("CREAR ASOCIACIÓN PLAN-PRODUCTO");
            var entity = new PromotionalPlanProduct();

            entity.Plan_Id = GetValidatedIntInput("ID del plan: ");
            entity.Producto_Id = GetValidatedInput("ID del producto: ");

            try
            {
                _service.CreatePlanProduct(entity);
                ShowSuccessMessage("Asociación creada exitosamente.");
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Error al crear asociación: {ex.Message}");
            }
        }

        private void ListarAsociaciones()
        {
            ShowHeader("LISTA DE ASOCIACIONES");
            try
            {
                _service.GetAllPlanProducts();
                ShowInfoMessage("Listado de asociaciones completado.");
                Console.WriteLine();
                Console.Write("Presione cualquier tecla para continuar...");
                Console.ReadKey(true);
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Error al listar asociaciones: {ex.Message}");
            }
        }

        private void ActualizarAsociacion()
        {
            ShowHeader("ACTUALIZAR ASOCIACIÓN");
            int oldPlanId = GetValidatedIntInput("ID actual del plan: ");
            string oldProductoId = GetValidatedInput("ID actual del producto: ");

            var entity = new PromotionalPlanProduct();
            entity.Plan_Id = GetValidatedIntInput("Nuevo ID del plan: ");
            entity.Producto_Id = GetValidatedInput("Nuevo ID del producto: ");

            try
            {
                _service.UpdatePlanProduct(oldPlanId, oldProductoId, entity);
                ShowSuccessMessage("Asociación actualizada exitosamente.");
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Error al actualizar asociación: {ex.Message}");
            }
        }

        private void EliminarAsociacion()
        {
            ShowHeader("ELIMINAR ASOCIACIÓN");
            int planId = GetValidatedIntInput("ID del plan: ");
            string productoId = GetValidatedInput("ID del producto: ");

            try
            {
                _service.DeletePlanProduct(planId, productoId);
                ShowSuccessMessage("Asociación eliminada exitosamente.");
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Error al eliminar asociación: {ex.Message}");
            }
        }
    }
}
