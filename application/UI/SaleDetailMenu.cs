using System;
using SGCI_app.application.services;
using SGCI_app.domain.Entities;
using Npgsql;

namespace SGCI_app.application.UI
{
    public class SaleDetailMenu
    {
        private readonly SaleDetailService _saleDetailService;

        public SaleDetailMenu(SaleDetailService saleDetailService)
        {
            _saleDetailService = saleDetailService;
        }

        public void ShowMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== GESTIÓN DE DETALLES DE VENTA ===");
                Console.WriteLine("1. Listar Detalles de Venta");
                Console.WriteLine("2. Agregar Detalle de Venta");
                Console.WriteLine("3. Actualizar Detalle de Venta");
                Console.WriteLine("4. Eliminar Detalle de Venta");
                Console.WriteLine("0. Volver al Menú de Ventas");
                Console.Write("\nSeleccione una opción: ");

                string? input = Console.ReadLine();

                try
                {
                    switch (input)
                    {
                        case "1":
                            ListarDetallesVenta();
                            break;
                        case "2":
                            AgregarDetalleVenta();
                            break;
                        case "3":
                            ActualizarDetalleVenta();
                            break;
                        case "4":
                            EliminarDetalleVenta();
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

        private void ListarDetallesVenta()
        {
            Console.Clear();
            Console.WriteLine("=== LISTADO DE DETALLES DE VENTA ===");
            try
            {
                _saleDetailService.MostrarTodos();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nError al listar detalles de venta: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Error interno: {ex.InnerException.Message}");
                }
            }
            Console.WriteLine("\nPresione cualquier tecla para continuar...");
            Console.ReadKey();
        }

        private void AgregarDetalleVenta()
        {
            Console.Clear();
            Console.WriteLine("=== AGREGAR DETALLE DE VENTA ===");
            
            try
            {
                var detalle = new SaleDetail();
                
                Console.Write("ID de la Venta: ");
                if (!int.TryParse(Console.ReadLine(), out int factId))
                {
                    throw new Exception("ID de venta inválido. Debe ser un número.");
                }
                detalle.FactId = factId;
                
                Console.Write("ID del Producto: ");
                detalle.Producto_Id = Console.ReadLine();
                if (string.IsNullOrEmpty(detalle.Producto_Id))
                {
                    throw new Exception("El ID del producto no puede estar vacío");
                }
                
                Console.Write("Cantidad: ");
                if (!int.TryParse(Console.ReadLine(), out int cantidad) || cantidad <= 0)
                {
                    throw new Exception("La cantidad debe ser un número positivo");
                }
                detalle.Cantidad = cantidad;
                
                Console.Write("Valor Unitario: ");
                if (!double.TryParse(Console.ReadLine(), out double valor) || valor <= 0)
                {
                    throw new Exception("El valor debe ser un número positivo");
                }
                detalle.Valor = valor;

                Console.WriteLine("\nResumen del detalle de venta:");
                Console.WriteLine($"ID de Venta: {detalle.FactId}");
                Console.WriteLine($"ID de Producto: {detalle.Producto_Id}");
                Console.WriteLine($"Cantidad: {detalle.Cantidad}");
                Console.WriteLine($"Valor Unitario: {detalle.Valor:C}");
                Console.WriteLine($"Total: {detalle.Cantidad * detalle.Valor:C}");
                
                Console.Write("\n¿Desea confirmar la operación? (S/N): ");
                if (Console.ReadLine()?.ToUpper() != "S")
                {
                    Console.WriteLine("Operación cancelada.");
                    return;
                }

                _saleDetailService.CrearDetalleVenta(detalle);
                Console.WriteLine("Detalle de venta agregado exitosamente.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nError al agregar detalle de venta: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Error interno: {ex.InnerException.Message}");
                }
            }
            Console.WriteLine("\nPresione cualquier tecla para continuar...");
            Console.ReadKey();
        }

        private void ActualizarDetalleVenta()
        {
            Console.Clear();
            Console.WriteLine("=== ACTUALIZAR DETALLE DE VENTA ===");
            
            try
            {
                Console.Write("Ingrese el ID del detalle de venta a actualizar: ");
                if (!int.TryParse(Console.ReadLine(), out int id))
                {
                    throw new Exception("ID inválido. Debe ser un número.");
                }

                var detalle = new SaleDetail { Id = id };
                
                Console.Write("Nueva Cantidad (dejar en blanco para mantener la actual): ");
                var cantidadInput = Console.ReadLine();
                if (!string.IsNullOrEmpty(cantidadInput))
                {
                    if (!int.TryParse(cantidadInput, out int cantidad) || cantidad <= 0)
                    {
                        throw new Exception("La cantidad debe ser un número positivo");
                    }
                    detalle.Cantidad = cantidad;
                }
                
                Console.Write("Nuevo Valor Unitario (dejar en blanco para mantener el actual): ");
                var valorInput = Console.ReadLine();
                if (!string.IsNullOrEmpty(valorInput))
                {
                    if (!double.TryParse(valorInput, out double valor) || valor <= 0)
                    {
                        throw new Exception("El valor debe ser un número positivo");
                    }
                    detalle.Valor = valor;
                }

                Console.WriteLine("\nResumen de la actualización:");
                if (detalle.Cantidad > 0)
                {
                    Console.WriteLine($"Nueva Cantidad: {detalle.Cantidad}");
                }
                if (detalle.Valor > 0)
                {
                    Console.WriteLine($"Nuevo Valor Unitario: {detalle.Valor:C}");
                }
                
                Console.Write("\n¿Desea confirmar la operación? (S/N): ");
                if (Console.ReadLine()?.ToUpper() != "S")
                {
                    Console.WriteLine("Operación cancelada.");
                    return;
                }

                _saleDetailService.ActualizarDetalleVenta(detalle);
                Console.WriteLine("Detalle de venta actualizado exitosamente.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nError al actualizar detalle de venta: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Error interno: {ex.InnerException.Message}");
                }
            }
            Console.WriteLine("\nPresione cualquier tecla para continuar...");
            Console.ReadKey();
        }

        private void EliminarDetalleVenta()
        {
            Console.Clear();
            Console.WriteLine("=== ELIMINAR DETALLE DE VENTA ===");
            
            try
            {
                Console.Write("Ingrese el ID del detalle de venta a eliminar: ");
                if (!int.TryParse(Console.ReadLine(), out int id))
                {
                    throw new Exception("ID inválido. Debe ser un número.");
                }

                Console.Write("\n¿Está seguro que desea eliminar este detalle de venta? (S/N): ");
                if (Console.ReadLine()?.ToUpper() != "S")
                {
                    Console.WriteLine("Operación cancelada.");
                    return;
                }

                _saleDetailService.EliminarDetalleVenta(id);
                Console.WriteLine("Detalle de venta eliminado exitosamente.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nError al eliminar detalle de venta: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Error interno: {ex.InnerException.Message}");
                }
            }
            Console.WriteLine("\nPresione cualquier tecla para continuar...");
            Console.ReadKey();
        }
    }
} 