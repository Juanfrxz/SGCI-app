using System;
using SGCI_app.application.services;
using SGCI_app.domain.Entities;
using SGCI_app.infrastructure.postgres;
using Npgsql;

namespace SGCI_app.application.UI
{
    public class SaleMenu : BaseMenu
    {
        private readonly SaleService _saleService;
        private readonly SaleDetailService _saleDetailService;

        public SaleMenu() : base(showIntro: false)
        {
            string connStr = "Host=localhost;database=sgci;Port=5432;Username=postgres;Password=juan1374;Pooling=true";
            var factory = new ConexDBFactory(connStr);
            _saleService = new SaleService(factory.CrearSaleRepository());
            _saleDetailService = new SaleDetailService(factory.CrearSaleDetailRepository());
        }

        public override void ShowMenu()
        {
            while (true)
            {
                ShowHeader("GESTIÓN DE VENTAS");
                Console.WriteLine("1. Listar Ventas");
                Console.WriteLine("2. Crear Nueva Venta");
                Console.WriteLine("3. Actualizar Venta");
                Console.WriteLine("4. Eliminar Venta");
                Console.WriteLine("5. Gestionar Detalles de Venta");
                Console.WriteLine("0. Volver al Menú Principal");
                DrawSeparator();

                int option = GetValidatedIntInput("Seleccione una opción: ", 0, 5);
                switch (option)
                {
                    case 1:
                        ListarVentas();
                        break;
                    case 2:
                        CrearVenta();
                        break;
                    case 3:
                        ActualizarVenta();
                        break;
                    case 4:
                        EliminarVenta();
                        break;
                    case 5:
                        GestionarDetallesVenta();
                        break;
                    case 0:
                        return;
                }
            }
        }

        private void ListarVentas()
        {
            ShowHeader("LISTADO DE VENTAS");
            try
            {
                _saleService.MostrarTodos();
                ShowInfoMessage("Listado de ventas completado.");
                Console.WriteLine();
                Console.Write("Presione cualquier tecla para continuar...");
                Console.ReadKey(true);
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Error al listar ventas: {ex.Message}");
                if (ex.InnerException != null)
                {
                    ShowErrorMessage($"Error interno: {ex.InnerException.Message}");
                }
            }
        }

        private void CrearVenta()
        {
            ShowHeader("CREAR NUEVA VENTA");
            
            try
            {
                var venta = new Sale();
                
                venta.Fecha = GetValidatedDateInput("Fecha (YYYY-MM-DD): ");
                venta.TerceroEmpleado_Id = GetValidatedInput("ID del Empleado (tercero_id): ");
                venta.TerceroCliente_Id = GetValidatedInput("ID del Cliente (tercero_id): ");

                _saleService.CrearVenta(venta);
                ShowSuccessMessage("Venta creada exitosamente.");
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Error al crear la venta: {ex.Message}");
                if (ex.InnerException != null)
                {
                    ShowErrorMessage($"Error interno: {ex.InnerException.Message}");
                }
            }
        }

        private void ActualizarVenta()
        {
            ShowHeader("ACTUALIZAR VENTA");
            
            try
            {
                int id = GetValidatedIntInput("Ingrese el ID de la venta a actualizar: ");
                var venta = new Sale { FactId = id };
                
                string fechaInput = GetValidatedInput("Nueva Fecha (YYYY-MM-DD, dejar en blanco para mantener la actual): ", allowEmpty: true);
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
                
                string empleadoInput = GetValidatedInput("Nuevo ID del Empleado (tercero_id, dejar en blanco para mantener el actual): ", allowEmpty: true);
                if (!string.IsNullOrEmpty(empleadoInput)) venta.TerceroEmpleado_Id = empleadoInput;
                
                string clienteInput = GetValidatedInput("Nuevo ID del Cliente (tercero_id, dejar en blanco para mantener el actual): ", allowEmpty: true);
                if (!string.IsNullOrEmpty(clienteInput)) venta.TerceroCliente_Id = clienteInput;

                _saleService.ActualizarVenta(venta);
                ShowSuccessMessage("Venta actualizada exitosamente.");
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Error al actualizar la venta: {ex.Message}");
                if (ex.InnerException != null)
                {
                    ShowErrorMessage($"Error interno: {ex.InnerException.Message}");
                }
            }
        }

        private void EliminarVenta()
        {
            ShowHeader("ELIMINAR VENTA");
            
            try
            {
                int id = GetValidatedIntInput("Ingrese el ID de la venta a eliminar: ");
                _saleService.EliminarVenta(id);
                ShowSuccessMessage("Venta eliminada exitosamente.");
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Error al eliminar la venta: {ex.Message}");
                if (ex.InnerException != null)
                {
                    ShowErrorMessage($"Error interno: {ex.InnerException.Message}");
                }
            }
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
                ShowErrorMessage($"Error al gestionar detalles de venta: {ex.Message}");
                if (ex.InnerException != null)
                {
                    ShowErrorMessage($"Error interno: {ex.InnerException.Message}");
                }
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