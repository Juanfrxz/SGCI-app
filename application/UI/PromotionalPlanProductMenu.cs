using System;
using SGCI_app.application.services;
using SGCI_app.domain.Entities;
using SGCI_app.domain.Factory;
using SGCI_app.infrastructure.postgres;

namespace SGCI_app.application.UI
{
    public class PromotionalPlanProductMenu
    {
        private readonly PromotionalPlanProductService _service;

        public PromotionalPlanProductMenu()
        {
            string connStr = "Host=localhost;database=sgci;Port=5432;Username=postgres;Password=1219;Pooling=true";
            var factory = new ConexDBFactory(connStr);
            _service = new PromotionalPlanProductService(factory.CrearPromoPlanProdRepository());
        }

        public void ShowMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== GESTIÓN DE ASOCIACIONES PLAN-PRODUCTO ===");
                Console.WriteLine("1. Crear Asociación");
                Console.WriteLine("2. Listar Asociaciones");
                Console.WriteLine("3. Actualizar Asociación");
                Console.WriteLine("4. Eliminar Asociación");
                Console.WriteLine("0. Volver al Menú Principal");
                Console.Write("\nSeleccione una opción: ");

                var input = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(input))
                {
                    Console.WriteLine("Por favor, ingrese una opción válida.");
                    Console.ReadKey();
                    continue;
                }

                switch (input)
                {
                    case "1": CrearAsociacion(); break;
                    case "2": ListarAsociaciones(); break;
                    case "3": ActualizarAsociacion(); break;
                    case "4": EliminarAsociacion(); break;
                    case "0": return;
                    default:
                        Console.WriteLine("Opción no válida. Presione cualquier tecla para continuar...");
                        Console.ReadKey();
                        break;
                }
            }
        }

        private void CrearAsociacion()
        {
            Console.Clear();
            Console.WriteLine("=== CREAR ASOCIACIÓN PLAN-PRODUCTO ===");

            var entity = new PromotionalPlanProduct();
            Console.Write("ID del plan: ");
            var planInput = Console.ReadLine();
            if (!int.TryParse(planInput, out int planId))
            {
                Console.WriteLine("ID de plan inválido.");
                Console.ReadKey();
                return;
            }
            entity.Plan_Id = planId;

            Console.Write("ID del producto: ");
            var productoInput = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(productoInput))
            {
                Console.WriteLine("ID de producto inválido.");
                Console.ReadKey();
                return;
            }
            entity.Producto_Id = productoInput;

            try
            {
                _service.CreatePlanProduct(entity);
                Console.WriteLine("\nAsociación creada exitosamente.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nError al crear asociación: {ex.Message}");
            }

            Console.WriteLine("\nPresione cualquier tecla para continuar...");
            Console.ReadKey();
        }

        private void ListarAsociaciones()
        {
            Console.Clear();
            Console.WriteLine("=== LISTA DE ASOCIACIONES ===\n");

            try
            {
                _service.GetAllPlanProducts();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nError al listar asociaciones: {ex.Message}");
            }

            Console.WriteLine("\nPresione cualquier tecla para continuar...");
            Console.ReadKey();
        }

        private void ActualizarAsociacion()
        {
            Console.Clear();
            Console.WriteLine("=== ACTUALIZAR ASOCIACIÓN ===");

            Console.Write("ID actual del plan: ");
            var oldPlanInput = Console.ReadLine();
            if (!int.TryParse(oldPlanInput, out int oldPlanId))
            {
                Console.WriteLine("ID de plan inválido.");
                Console.ReadKey();
                return;
            }

            Console.Write("ID actual del producto: ");
            var oldProductoId = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(oldProductoId))
            {
                Console.WriteLine("ID de producto inválido.");
                Console.ReadKey();
                return;
            }

            var entity = new PromotionalPlanProduct();
            Console.Write("Nuevo ID del plan: ");
            var newPlanInput = Console.ReadLine();
            if (!int.TryParse(newPlanInput, out int newPlanId))
            {
                Console.WriteLine("ID de plan inválido.");
                Console.ReadKey();
                return;
            }
            entity.Plan_Id = newPlanId;

            Console.Write("Nuevo ID del producto: ");
            var newProductoId = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(newProductoId))
            {
                Console.WriteLine("ID de producto inválido.");
                Console.ReadKey();
                return;
            }
            entity.Producto_Id = newProductoId;

            try
            {
                _service.UpdatePlanProduct(oldPlanId, oldProductoId, entity);
                Console.WriteLine("\nAsociación actualizada exitosamente.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nError al actualizar asociación: {ex.Message}");
            }

            Console.WriteLine("\nPresione cualquier tecla para continuar...");
            Console.ReadKey();
        }

        private void EliminarAsociacion()
        {
            Console.Clear();
            Console.WriteLine("=== ELIMINAR ASOCIACIÓN ===");

            Console.Write("ID del plan: ");
            var planInput = Console.ReadLine();
            if (!int.TryParse(planInput, out int planId))
            {
                Console.WriteLine("ID de plan inválido.");
                Console.ReadKey();
                return;
            }

            Console.Write("ID del producto: ");
            var productoId = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(productoId))
            {
                Console.WriteLine("ID de producto inválido.");
                Console.ReadKey();
                return;
            }

            try
            {
                _service.DeletePlanProduct(planId, productoId);
                Console.WriteLine("\nAsociación eliminada exitosamente.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nError al eliminar asociación: {ex.Message}");
            }

            Console.WriteLine("\nPresione cualquier tecla para continuar...");
            Console.ReadKey();
        }
    }
}
