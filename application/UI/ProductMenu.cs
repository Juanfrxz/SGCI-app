using System;
using System.Collections.Generic;
using SGCI_app.application.services;
using SGCI_app.domain.Entities;
using SGCI_app.domain.Factory;
using SGCI_app.infrastructure.postgres;

namespace SGCI_app.application.UI
{
    public class ProductMenu
    {
        private readonly ProductService _service;

        public ProductMenu()
        {
            string connStr = "Host=localhost;database=sgci;Port=5432;Username=postgres;Password=1219;Pooling=true";
            var factory = new ConexDBFactory(connStr);
            _service = new ProductService(factory.CrearProductRepository());
        }

        public void ShowMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== GESTIÓN DE PRODUCTOS ===");
                Console.WriteLine("1. Crear Producto");
                Console.WriteLine("2. Listar Productos");
                Console.WriteLine("3. Actualizar Producto");
                Console.WriteLine("4. Eliminar Producto");
                Console.WriteLine("0. Volver al Menú Principal");
                Console.Write("\nSeleccione una opción: ");

                var input = Console.ReadLine();
                if (string.IsNullOrEmpty(input))
                {
                    Console.WriteLine("Por favor, ingrese una opción válida.");
                    Console.ReadKey();
                    continue;
                }

                switch (input)
                {
                    case "1": CrearProducto(); break;
                    case "2": ListarProductos(); break;
                    case "3": ActualizarProducto(); break;
                    case "4": EliminarProducto(); break;
                    case "0": return;
                    default:
                        Console.WriteLine("Opción no válida. Presione cualquier tecla para continuar...");
                        Console.ReadKey();
                        break;
                }
            }
        }

        private void CrearProducto()
        {
            Console.Clear();
            Console.WriteLine("=== CREAR PRODUCTO ===");

            var product = new Product();
            Console.Write("ID del producto: ");
            product.Id = Console.ReadLine();

            Console.Write("Nombre del producto: ");
            product.Nombre = Console.ReadLine();

            Console.Write("Stock: ");
            if (int.TryParse(Console.ReadLine(), out int stock)) product.Stock = stock;

            Console.Write("Stock mínimo: ");
            if (int.TryParse(Console.ReadLine(), out int stockMin)) product.StockMin = stockMin;

            Console.Write("Stock máximo: ");
            if (int.TryParse(Console.ReadLine(), out int stockMax)) product.StockMax = stockMax;

            // al crear, establecemos fechas
            product.FechaCreacion = DateTime.Now;
            product.FechaActualizacion = DateTime.Now;

            Console.Write("Código de barras: ");
            product.CodigoBarras = Console.ReadLine();

            try
            {
                _service.CreateProduct(product);
                Console.WriteLine("\nProducto creado exitosamente.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nError al crear el producto: {ex.Message}");
            }

            Console.WriteLine("\nPresione cualquier tecla para continuar...");
            Console.ReadKey();
        }

        private void ListarProductos()
        {
            Console.Clear();
            Console.WriteLine("=== LISTA DE PRODUCTOS ===\n");

            try
            {
                _service.GetAllProducts();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nError al obtener los productos: {ex.Message}");
            }

            Console.WriteLine("\nPresione cualquier tecla para continuar...");
            Console.ReadKey();
        }

        private void ActualizarProducto()
        {
            Console.Clear();
            Console.WriteLine("=== ACTUALIZAR PRODUCTO ===");

            Console.Write("Ingrese el ID del producto a actualizar: ");
            var id = Console.ReadLine();
            if (string.IsNullOrEmpty(id))
            {
                Console.WriteLine("ID inválido.");
                Console.ReadKey();
                return;
            }

            var product = new Product { Id = id };

            Console.Write("Nuevo nombre (dejar en blanco para mantener el actual): ");
            product.Nombre = Console.ReadLine();

            Console.Write("Nuevo stock (dejar en blanco para mantener el actual): ");
            var stockInput = Console.ReadLine();
            if (int.TryParse(stockInput, out int stock)) product.Stock = stock;

            Console.Write("Nuevo stock mínimo (dejar en blanco para mantener el actual): ");
            var minInput = Console.ReadLine();
            if (int.TryParse(minInput, out int min)) product.StockMin = min;

            Console.Write("Nuevo stock máximo (dejar en blanco para mantener el actual): ");
            var maxInput = Console.ReadLine();
            if (int.TryParse(maxInput, out int max)) product.StockMax = max;

            Console.Write("Nueva fecha de actualización (YYYY-MM-DD, dejar en blanco para fecha actual): ");
            var dateInput = Console.ReadLine();
            if (!string.IsNullOrEmpty(dateInput) && DateTime.TryParse(dateInput, out DateTime updated))
                product.FechaActualizacion = updated;
            else
                product.FechaActualizacion = DateTime.Now;

            Console.Write("Nuevo código de barras (dejar en blanco para mantener el actual): ");
            product.CodigoBarras = Console.ReadLine();

            try
            {
                _service.UpdateProduct(product);
                Console.WriteLine("\nProducto actualizado exitosamente.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nError al actualizar el producto: {ex.Message}");
            }

            Console.WriteLine("\nPresione cualquier tecla para continuar...");
            Console.ReadKey();
        }

        private void EliminarProducto()
        {
            Console.Clear();
            Console.WriteLine("=== ELIMINAR PRODUCTO ===");

            Console.Write("Ingrese el ID del producto a eliminar: ");
            var id = Console.ReadLine();
            if (string.IsNullOrEmpty(id))
            {
                Console.WriteLine("ID inválido.");
                Console.ReadKey();
                return;
            }

            try
            {
                _service.DeleteProduct(id);
                Console.WriteLine("\nProducto eliminado exitosamente.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nError al eliminar el producto: {ex.Message}");
            }

            Console.WriteLine("\nPresione cualquier tecla para continuar...");
            Console.ReadKey();
        }
    }
}
