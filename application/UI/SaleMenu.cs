using System;
using SGCI_app.application.services;
using SGCI_app.domain.Entities;
using SGCI_app.infrastructure.postgres;
using Npgsql;

namespace SGCI_app.application.UI
{
    public class SaleMenu
    {
        private readonly SaleService _saleService;
        private readonly SaleDetailService _saleDetailService;

        public SaleMenu()
        {
            string connStr = "Host=localhost;database=sgci;Port=5432;Username=postgres;Password=juan1374;Pooling=true";
            var factory = new ConexDBFactory(connStr);
            _saleService = new SaleService(factory.CrearSaleRepository());
            _saleDetailService = new SaleDetailService(factory.CrearSaleDetailRepository());
        }

        public void ShowMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== GESTIÓN DE VENTAS ===");
                Console.WriteLine("1. Listar Ventas");
                Console.WriteLine("2. Crear Nueva Venta");
                Console.WriteLine("3. Actualizar Venta");
                Console.WriteLine("4. Eliminar Venta");
                Console.WriteLine("5. Gestionar Detalles de Venta");
                Console.WriteLine("0. Volver al Menú Principal");
                Console.Write("\nSeleccione una opción: ");

                string? input = Console.ReadLine();

                try
                {
                    switch (input)
                    {
                        case "1":
                            ListarVentas();
                            break;
                        case "2":
                            CrearVenta();
                            break;
                        case "3":
                            ActualizarVenta();
                            break;
                        case "4":
                            EliminarVenta();
                            break;
                        case "5":
                            GestionarDetallesVenta();
                            break;
                        case "0":
                            return;
                        default:
                            Console.WriteLine("Opción no válida. Presione cualquier tecla para continuar...");
                            Console.ReadKey();
                            break;
                    }
                }
                catch (PostgresException ex)
                {
                    Console.WriteLine($"\nError de base de datos: {ex.Message}");
                    Console.WriteLine("Detalles: " + ex.Detail);
                    Console.WriteLine("\nPresione cualquier tecla para continuar...");
                    Console.ReadKey();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"\nError inesperado: {ex.Message}");
                    Console.WriteLine("\nPresione cualquier tecla para continuar...");
                    Console.ReadKey();
                }
            }
        }

        private void ListarVentas()
        {
            Console.Clear();
            Console.WriteLine("=== LISTADO DE VENTAS ===");
            try
            {
                _saleService.MostrarTodos();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nError al listar ventas: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Error interno: {ex.InnerException.Message}");
                }
            }
            Console.WriteLine("\nPresione cualquier tecla para continuar...");
            Console.ReadKey();
        }

        private void CrearVenta()
        {
            Console.Clear();
            Console.WriteLine("=== CREAR NUEVA VENTA ===");
            
            try
            {
                var venta = new Sale();
                
                Console.Write("Fecha (YYYY-MM-DD): ");
                if (DateTime.TryParse(Console.ReadLine(), out DateTime fecha))
                {
                    venta.Fecha = fecha;
                }
                else
                {
                    throw new Exception("Formato de fecha inválido. Use YYYY-MM-DD");
                }
                
                Console.Write("ID del Empleado (tercero_id): ");
                venta.TerceroEmpleado_Id = Console.ReadLine();
                if (string.IsNullOrEmpty(venta.TerceroEmpleado_Id))
                {
                    throw new Exception("El ID del empleado no puede estar vacío");
                }
                
                Console.Write("ID del Cliente (tercero_id): ");
                venta.TerceroCliente_Id = Console.ReadLine();
                if (string.IsNullOrEmpty(venta.TerceroCliente_Id))
                {
                    throw new Exception("El ID del cliente no puede estar vacío");
                }

                _saleService.CrearVenta(venta);
                Console.WriteLine("Venta creada exitosamente.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nError al crear la venta: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Error interno: {ex.InnerException.Message}");
                }
            }
            Console.WriteLine("\nPresione cualquier tecla para continuar...");
            Console.ReadKey();
        }

        private void ActualizarVenta()
        {
            Console.Clear();
            Console.WriteLine("=== ACTUALIZAR VENTA ===");
            
            try
            {
                Console.Write("Ingrese el ID de la venta a actualizar: ");
                if (!int.TryParse(Console.ReadLine(), out int id))
                {
                    throw new Exception("ID inválido. Debe ser un número.");
                }

                var venta = new Sale { FactId = id };
                
                Console.Write("Nueva Fecha (YYYY-MM-DD, dejar en blanco para mantener la actual): ");
                var fechaInput = Console.ReadLine();
                if (!string.IsNullOrEmpty(fechaInput))
                {
                    if (DateTime.TryParse(fechaInput, out DateTime fecha))
                    {
                        venta.Fecha = fecha;
                    }
                    else
                    {
                        throw new Exception("Formato de fecha inválido. Use YYYY-MM-DD");
                    }
                }
                
                Console.Write("Nuevo ID del Empleado (tercero_id, dejar en blanco para mantener el actual): ");
                venta.TerceroEmpleado_Id = Console.ReadLine();
                
                Console.Write("Nuevo ID del Cliente (tercero_id, dejar en blanco para mantener el actual): ");
                venta.TerceroCliente_Id = Console.ReadLine();

                _saleService.ActualizarVenta(venta);
                Console.WriteLine("Venta actualizada exitosamente.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nError al actualizar la venta: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Error interno: {ex.InnerException.Message}");
                }
            }
            Console.WriteLine("\nPresione cualquier tecla para continuar...");
            Console.ReadKey();
        }

        private void EliminarVenta()
        {
            Console.Clear();
            Console.WriteLine("=== ELIMINAR VENTA ===");
            
            try
            {
                Console.Write("Ingrese el ID de la venta a eliminar: ");
                if (!int.TryParse(Console.ReadLine(), out int id))
                {
                    throw new Exception("ID inválido. Debe ser un número.");
                }

                _saleService.EliminarVenta(id);
                Console.WriteLine("Venta eliminada exitosamente.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nError al eliminar la venta: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Error interno: {ex.InnerException.Message}");
                }
            }
            Console.WriteLine("\nPresione cualquier tecla para continuar...");
            Console.ReadKey();
        }

        private void GestionarDetallesVenta()
        {
            try
            {
                var saleDetailMenu = new SaleDetailMenu(_saleDetailService);
                saleDetailMenu.ShowMenu();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nError al gestionar detalles de venta: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Error interno: {ex.InnerException.Message}");
                }
                Console.WriteLine("\nPresione cualquier tecla para continuar...");
                Console.ReadKey();
            }
        }
    }
} 