using System;
using SGCI_app.application.services;
using SGCI_app.domain.Entities;
using SGCI_app.infrastructure.postgres;

namespace SGCI_app.application.UI
{
    public class PurchaseMenu : BaseMenu
    {
        private readonly PurchaseService _purchaseService;
        private readonly PurchaseDetailService _purchaseDetailService;

        public PurchaseMenu() : base(showIntro: false)
        {
            string connStr = "Host=localhost;database=sgci;Port=5432;Username=postgres;Password=juan1374;Pooling=true";
            var factory = new ConexDBFactory(connStr);
            _purchaseService = new PurchaseService(factory.CrearPurchaseRepository());
            _purchaseDetailService = new PurchaseDetailService(factory.CrearPurchaseDetailRepository());
        }

        public override void ShowMenu()
        {
            while (true)
            {
                ShowHeader("GESTIÓN DE COMPRAS");
                Console.WriteLine("1. Listar Compras");
                Console.WriteLine("2. Crear Nueva Compra");
                Console.WriteLine("3. Actualizar Compra");
                Console.WriteLine("4. Eliminar Compra");
                Console.WriteLine("5. Gestionar Detalles de Compra");
                Console.WriteLine("0. Volver al Menú Principal");
                DrawSeparator();

                int option = GetValidatedIntInput("Seleccione una opción: ", 0, 5);
                switch (option)
                {
                    case 1:
                        ListarCompras();
                        break;
                    case 2:
                        CrearCompra();
                        break;
                    case 3:
                        ActualizarCompra();
                        break;
                    case 4:
                        EliminarCompra();
                        break;
                    case 5:
                        GestionarDetallesCompra();
                        break;
                    case 0:
                        return;
                }
            }
        }

        private void ListarCompras()
        {
            ShowHeader("LISTADO DE COMPRAS");
            try
            {
                _purchaseService.MostrarTodos();
                ShowInfoMessage("Listado de compras completado.");
                Console.WriteLine();
                Console.Write("Presione cualquier tecla para continuar...");
                Console.ReadKey(true);
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Error al listar compras: {ex.Message}");
            }
        }

        private void CrearCompra()
        {
            ShowHeader("CREAR NUEVA COMPRA");
            
            try
            {
                var compra = new Purchase();
                
                compra.TerceroProveedor_Id = GetValidatedInput("ID del Proveedor: ");
                compra.Fecha = GetValidatedDateInput("Fecha (YYYY-MM-DD): ");
                compra.TerceroEmpleado_Id = GetValidatedInput("ID del Empleado: ");
                compra.DocCompra = GetValidatedInput("Documento de Compra: ");

                _purchaseService.CrearCompra(compra);
                ShowSuccessMessage("Compra creada exitosamente.");
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Error al crear la compra: {ex.Message}");
            }
        }

        private void ActualizarCompra()
        {
            ShowHeader("ACTUALIZAR COMPRA");
            
            try
            {
                int id = GetValidatedIntInput("Ingrese el ID de la compra a actualizar: ");
                var compra = new Purchase { Id = id };
                
                string proveedorInput = GetValidatedInput("Nuevo ID del Proveedor (dejar en blanco para mantener el actual): ", allowEmpty: true);
                if (!string.IsNullOrEmpty(proveedorInput)) compra.TerceroProveedor_Id = proveedorInput;
                
                string fechaInput = GetValidatedInput("Nueva Fecha (YYYY-MM-DD, dejar en blanco para mantener la actual): ", allowEmpty: true);
                if (!string.IsNullOrEmpty(fechaInput) && DateTime.TryParse(fechaInput, out DateTime fecha))
                {
                    compra.Fecha = fecha;
                }
                
                string empleadoInput = GetValidatedInput("Nuevo ID del Empleado (dejar en blanco para mantener el actual): ", allowEmpty: true);
                if (!string.IsNullOrEmpty(empleadoInput)) compra.TerceroEmpleado_Id = empleadoInput;
                
                string docInput = GetValidatedInput("Nuevo Documento de Compra (dejar en blanco para mantener el actual): ", allowEmpty: true);
                if (!string.IsNullOrEmpty(docInput)) compra.DocCompra = docInput;

                _purchaseService.ActualizarCompra(compra);
                ShowSuccessMessage("Compra actualizada exitosamente.");
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Error al actualizar la compra: {ex.Message}");
            }
        }

        private void EliminarCompra()
        {
            ShowHeader("ELIMINAR COMPRA");
            
            try
            {
                int id = GetValidatedIntInput("Ingrese el ID de la compra a eliminar: ");
                _purchaseService.EliminarCompra(id);
                ShowSuccessMessage("Compra eliminada exitosamente.");
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Error al eliminar la compra: {ex.Message}");
            }
        }

        private void GestionarDetallesCompra()
        {
            while (true)
            {
                ShowHeader("GESTIÓN DE DETALLES DE COMPRA");
                Console.WriteLine("1. Listar Detalles");
                Console.WriteLine("2. Agregar Detalle");
                Console.WriteLine("3. Actualizar Detalle");
                Console.WriteLine("4. Eliminar Detalle");
                Console.WriteLine("0. Volver");
                DrawSeparator();

                int option = GetValidatedIntInput("Seleccione una opción: ", 0, 4);
                switch (option)
                {
                    case 1:
                        ListarDetalles();
                        break;
                    case 2:
                        AgregarDetalle();
                        break;
                    case 3:
                        ActualizarDetalle();
                        break;
                    case 4:
                        EliminarDetalle();
                        break;
                    case 0:
                        return;
                }
            }
        }

        private void ListarDetalles()
        {
            ShowHeader("LISTADO DE DETALLES DE COMPRA");
            try
            {
                _purchaseDetailService.MostrarTodos();
                ShowInfoMessage("Listado de detalles completado.");
                Console.WriteLine();
                Console.Write("Presione cualquier tecla para continuar...");
                Console.ReadKey(true);
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Error al listar detalles: {ex.Message}");
            }
        }

        private void AgregarDetalle()
        {
            ShowHeader("AGREGAR DETALLE DE COMPRA");
            
            try
            {
                var detalle = new PurchaseDetail();
                
                detalle.Compra_Id = GetValidatedIntInput("ID de la Compra: ");
                detalle.Producto_Id = GetValidatedInput("ID del Producto: ");
                detalle.Cantidad = GetValidatedIntInput("Cantidad: ");
                detalle.Valor = GetValidatedIntInput("Valor: ");
                detalle.Fecha = DateTime.Now;

                _purchaseDetailService.CrearDetalleCompra(detalle);
                ShowSuccessMessage("Detalle de compra creado exitosamente.");
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Error al crear el detalle de compra: {ex.Message}");
            }
        }

        private void ActualizarDetalle()
        {
            ShowHeader("ACTUALIZAR DETALLE DE COMPRA");
            
            try
            {
                int id = GetValidatedIntInput("Ingrese el ID del detalle a actualizar: ");
                var detalle = new PurchaseDetail { Id = id };
                
                string compraInput = GetValidatedInput("Nuevo ID de la Compra (dejar en blanco para mantener el actual): ", allowEmpty: true);
                if (!string.IsNullOrEmpty(compraInput) && int.TryParse(compraInput, out int compraId))
                    detalle.Compra_Id = compraId;
                
                string productoInput = GetValidatedInput("Nuevo ID del Producto (dejar en blanco para mantener el actual): ", allowEmpty: true);
                if (!string.IsNullOrEmpty(productoInput)) detalle.Producto_Id = productoInput;
                
                string cantidadInput = GetValidatedInput("Nueva Cantidad (dejar en blanco para mantener la actual): ", allowEmpty: true);
                if (!string.IsNullOrEmpty(cantidadInput) && int.TryParse(cantidadInput, out int cantidad))
                    detalle.Cantidad = cantidad;
                
                string valorInput = GetValidatedInput("Nuevo Valor (dejar en blanco para mantener el actual): ", allowEmpty: true);
                if (!string.IsNullOrEmpty(valorInput) && int.TryParse(valorInput, out int valor))
                    detalle.Valor = valor;
                
                detalle.Fecha = DateTime.Now;

                _purchaseDetailService.ActualizarDetalleCompra(detalle);
                ShowSuccessMessage("Detalle de compra actualizado exitosamente.");
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Error al actualizar el detalle de compra: {ex.Message}");
            }
        }

        private void EliminarDetalle()
        {
            ShowHeader("ELIMINAR DETALLE DE COMPRA");
            
            try
            {
                int id = GetValidatedIntInput("Ingrese el ID del detalle a eliminar: ");
                _purchaseDetailService.EliminarDetalleCompra(id);
                ShowSuccessMessage("Detalle de compra eliminado exitosamente.");
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Error al eliminar el detalle de compra: {ex.Message}");
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