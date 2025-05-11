using System;
using SGCI_app.application.Services;
using SGCI_app.domain.Entities;
using SGCI_app.domain.Factory;
using SGCI_app.infrastructure.postgres;

namespace SGCI_app.application.UI
{
    public class ProductSupplierMenu
    {
        private readonly ProductSupplierService _service;

        public ProductSupplierMenu()
        {
            string connStr = "Host=localhost;database=sgci;Port=5432;Username=postgres;Password=1219;Pooling=true";
            var factory = new ConexDBFactory(connStr);
            _service = new ProductSupplierService(factory.CrearProductSupplierRepository());
        }

        public void ShowMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== GESTIÓN PRODUCTOS_PROVEEDOR ===");
                Console.WriteLine("1. Crear Asociación Producto-Proveedor");
                Console.WriteLine("2. Listar Asociaciones");
                Console.WriteLine("3. Actualizar Asociación");
                Console.WriteLine("4. Eliminar Asociación");
                Console.WriteLine("0. Volver al menú principal");
                Console.Write("\nSeleccione una opción: ");

                string? input = Console.ReadLine();
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
            Console.WriteLine("=== CREAR ASOCIACIÓN PRODUCTO-PROVEEDOR ===");

            Console.Write("ID del proveedor: ");
            var terceroId = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(terceroId))
            {
                Console.WriteLine("ID de proveedor inválido."); Console.ReadKey(); return;
            }

            Console.Write("ID del producto: ");
            var productoId = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(productoId))
            {
                Console.WriteLine("ID de producto inválido."); Console.ReadKey(); return;
            }

            var entity = new ProductSupplier { Tercero_Id = terceroId, Producto_Id = productoId };
            try
            {
                _service.CrearProductSupplier(entity);
                Console.WriteLine("\nAsociación creada exitosamente.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nError al crear asociación: {ex.Message}");
            }
            Console.WriteLine("\nPresione cualquier tecla para continuar..."); Console.ReadKey();
        }

        private void ListarAsociaciones()
        {
            Console.Clear();
            Console.WriteLine("=== LISTA DE ASOCIACIONES ===\n");
            try
            {
                _service.MostrarTodos();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nError al listar asociaciones: {ex.Message}");
            }
            Console.WriteLine("\nPresione cualquier tecla para continuar..."); Console.ReadKey();
        }

        private void ActualizarAsociacion()
        {
            Console.Clear();
            Console.WriteLine("=== ACTUALIZAR ASOCIACIÓN ===");
            Console.Write("ID actual del proveedor: ");
            var oldTercero = Console.ReadLine();
            Console.Write("ID actual del producto: ");
            var oldProd = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(oldTercero) || string.IsNullOrWhiteSpace(oldProd))
            {
                Console.WriteLine("IDs inválidos."); Console.ReadKey(); return;
            }
            Console.Write("Nuevo ID del proveedor: ");
            var newTercero = Console.ReadLine();
            Console.Write("Nuevo ID del producto: ");
            var newProd = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(newTercero) || string.IsNullOrWhiteSpace(newProd))
            {
                Console.WriteLine("Nuevos IDs inválidos."); Console.ReadKey(); return;
            }
            var entity = new ProductSupplier { Tercero_Id = newTercero, Producto_Id = newProd };
            try
            {
                _service.ActualizarProductSupplier(oldTercero, oldProd, entity);
                Console.WriteLine("\nAsociación actualizada exitosamente.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nError al actualizar asociación: {ex.Message}");
            }
            Console.WriteLine("\nPresione cualquier tecla para continuar..."); Console.ReadKey();
        }

        private void EliminarAsociacion()
        {
            Console.Clear();
            Console.WriteLine("=== ELIMINAR ASOCIACIÓN ===");
            Console.Write("ID del proveedor: ");
            var terceroId = Console.ReadLine();
            Console.Write("ID del producto: ");
            var productoId = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(terceroId) || string.IsNullOrWhiteSpace(productoId))
            {
                Console.WriteLine("IDs inválidos."); Console.ReadKey(); return;
            }
            try
            {
                _service.EliminarProductSupplier(terceroId, productoId);
                Console.WriteLine("\nAsociación eliminada exitosamente.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nError al eliminar asociación: {ex.Message}");
            }
            Console.WriteLine("\nPresione cualquier tecla para continuar..."); Console.ReadKey();
        }
    }
}
