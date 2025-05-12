using System;
using SGCI_app.application.services;
using SGCI_app.domain.Entities;
using Npgsql;

namespace SGCI_app.application.UI
{
    public class SaleDetailMenu : BaseMenu
    {
        private readonly SaleDetailService _saleDetailService;

        public SaleDetailMenu(SaleDetailService saleDetailService) : base(showIntro: false)
        {
            _saleDetailService = saleDetailService;
        }

        public override void ShowMenu()
        {
            while (true)
            {
                ShowHeader("GESTIÓN DE DETALLES DE VENTA");
                Console.WriteLine("1. Listar Detalles de Venta");
                Console.WriteLine("2. Agregar Detalle de Venta");
                Console.WriteLine("3. Actualizar Detalle de Venta");
                Console.WriteLine("4. Eliminar Detalle de Venta");
                Console.WriteLine("0. Volver al Menú de Ventas");
                DrawSeparator();

                int option = GetValidatedIntInput("Seleccione una opción: ", 0, 4);
                switch (option)
                {
                    case 1:
                        ListarDetallesVenta();
                        break;
                    case 2:
                        AgregarDetalleVenta();
                        break;
                    case 3:
                        ActualizarDetalleVenta();
                        break;
                    case 4:
                        EliminarDetalleVenta();
                        break;
                    case 0:
                        return;
                }
            }
        }

        private void ListarDetallesVenta()
        {
            ShowHeader("LISTADO DE DETALLES DE VENTA");
            try
            {
                _saleDetailService.MostrarTodos();
                ShowInfoMessage("Listado de detalles de venta completado.");
                Console.WriteLine();
                Console.Write("Presione cualquier tecla para continuar...");
                Console.ReadKey(true);
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Error al listar detalles de venta: {ex.Message}");
                if (ex.InnerException != null)
                {
                    ShowErrorMessage($"Error interno: {ex.InnerException.Message}");
                }
            }
        }

        private void AgregarDetalleVenta()
        {
            ShowHeader("AGREGAR DETALLE DE VENTA");
            
            try
            {
                var detalle = new SaleDetail();
                
                detalle.FactId = GetValidatedIntInput("ID de la Venta: ");
                detalle.Producto_Id = GetValidatedInput("ID del Producto: ");
                detalle.Cantidad = GetValidatedIntInput("Cantidad: ");
                detalle.Valor = GetValidatedDoubleInput("Valor Unitario: ");

                Console.WriteLine("\nResumen del detalle de venta:");
                Console.WriteLine($"ID de Venta: {detalle.FactId}");
                Console.WriteLine($"ID de Producto: {detalle.Producto_Id}");
                Console.WriteLine($"Cantidad: {detalle.Cantidad}");
                Console.WriteLine($"Valor Unitario: {detalle.Valor:C}");
                Console.WriteLine($"Total: {detalle.Cantidad * detalle.Valor:C}");
                
                if (GetValidatedInput("¿Desea confirmar la operación? (S/N): ").ToUpper() != "S")
                {
                    ShowInfoMessage("Operación cancelada.");
                    return;
                }

                _saleDetailService.CrearDetalleVenta(detalle);
                ShowSuccessMessage("Detalle de venta agregado exitosamente.");
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Error al agregar detalle de venta: {ex.Message}");
                if (ex.InnerException != null)
                {
                    ShowErrorMessage($"Error interno: {ex.InnerException.Message}");
                }
            }
        }

        private void ActualizarDetalleVenta()
        {
            ShowHeader("ACTUALIZAR DETALLE DE VENTA");
            
            try
            {
                int id = GetValidatedIntInput("Ingrese el ID del detalle de venta a actualizar: ");
                var detalle = new SaleDetail { Id = id };
                
                string cantidadInput = GetValidatedInput("Nueva Cantidad (dejar en blanco para mantener la actual): ", allowEmpty: true);
                if (!string.IsNullOrEmpty(cantidadInput))
                {
                    if (!int.TryParse(cantidadInput, out int cantidad) || cantidad <= 0)
                    {
                        throw new Exception("La cantidad debe ser un número positivo");
                    }
                    detalle.Cantidad = cantidad;
                }
                
                string valorInput = GetValidatedInput("Nuevo Valor Unitario (dejar en blanco para mantener el actual): ", allowEmpty: true);
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
                
                if (GetValidatedInput("¿Desea confirmar la operación? (S/N): ").ToUpper() != "S")
                {
                    ShowInfoMessage("Operación cancelada.");
                    return;
                }

                _saleDetailService.ActualizarDetalleVenta(detalle);
                ShowSuccessMessage("Detalle de venta actualizado exitosamente.");
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Error al actualizar detalle de venta: {ex.Message}");
                if (ex.InnerException != null)
                {
                    ShowErrorMessage($"Error interno: {ex.InnerException.Message}");
                }
            }
        }

        private void EliminarDetalleVenta()
        {
            ShowHeader("ELIMINAR DETALLE DE VENTA");
            
            try
            {
                int id = GetValidatedIntInput("Ingrese el ID del detalle de venta a eliminar: ");

                if (GetValidatedInput("¿Está seguro que desea eliminar este detalle de venta? (S/N): ").ToUpper() != "S")
                {
                    ShowInfoMessage("Operación cancelada.");
                    return;
                }

                _saleDetailService.EliminarDetalleVenta(id);
                ShowSuccessMessage("Detalle de venta eliminado exitosamente.");
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Error al eliminar detalle de venta: {ex.Message}");
                if (ex.InnerException != null)
                {
                    ShowErrorMessage($"Error interno: {ex.InnerException.Message}");
                }
            }
        }

        private double GetValidatedDoubleInput(string prompt)
        {
            while (true)
            {
                string input = GetValidatedInput(prompt);
                if (double.TryParse(input, out double value) && value > 0)
                    return value;
                ShowErrorMessage("El valor debe ser un número positivo.");
            }
        }
    }
} 
 