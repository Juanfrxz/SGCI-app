using System;
using SGCI_app.application.Services;
using SGCI_app.domain.Entities;
using SGCI_app.domain.Factory;
using SGCI_app.infrastructure.postgres;

namespace SGCI_app.application.UI
{
    public class ProductSupplierMenu : BaseMenu
    {
        private readonly ProductSupplierService _service;

        public ProductSupplierMenu() : base(showIntro: false)
        {
            string connStr = "Host=localhost;database=sgci;Port=5432;Username=postgres;Password=1219;Pooling=true";
            var factory = new ConexDBFactory(connStr);
            _service = new ProductSupplierService(factory.CrearProductSupplierRepository());
        }

        public override void ShowMenu()
        {
            while (true)
            {
                ShowHeader("GESTIÓN DE PROVEEDORES DE PRODUCTOS");
                Console.WriteLine("1. Crear Proveedor de Producto");
                Console.WriteLine("2. Listar Proveedores de Productos");
                Console.WriteLine("3. Actualizar Proveedor de Producto");
                Console.WriteLine("4. Eliminar Proveedor de Producto");
                Console.WriteLine("0. Volver al menú principal");
                DrawSeparator();

                int option = GetValidatedIntInput("Seleccione una opción: ", 0, 4);
                switch (option)
                {
                    case 1:
                        CrearProductSupplier();
                        break;
                    case 2:
                        ListarProductSuppliers();
                        break;
                    case 3:
                        ActualizarProductSupplier();
                        break;
                    case 4:
                        EliminarProductSupplier();
                        break;
                    case 0:
                        return;
                }
            }
        }

        private void CrearProductSupplier()
        {
            ShowHeader("CREAR PROVEEDOR DE PRODUCTO");
            var productSupplier = new ProductSupplier();

            productSupplier.Tercero_Id = GetValidatedInput("ID del proveedor: ");
            productSupplier.Producto_Id = GetValidatedInput("ID del producto: ");

            try
            {
                _service.CrearProductSupplier(productSupplier);
                ShowSuccessMessage("Proveedor de producto creado exitosamente.");
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Error al crear el proveedor de producto: {ex.Message}");
            }
        }

        private void ListarProductSuppliers()
        {
            ShowHeader("LISTA DE PROVEEDORES DE PRODUCTOS");
            try
            {
                _service.MostrarTodos();
                ShowInfoMessage("Listado de proveedores de productos completado.");
                Console.WriteLine();
                Console.Write("Presione cualquier tecla para continuar...");
                Console.ReadKey(true);
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Error al obtener los proveedores de productos: {ex.Message}");
            }
        }

        private void ActualizarProductSupplier()
        {
            ShowHeader("ACTUALIZAR PROVEEDOR DE PRODUCTO");
            string oldTerceroId = GetValidatedInput("ID del proveedor a actualizar: ");
            string oldProductoId = GetValidatedInput("ID del producto a actualizar: ");
            
            var productSupplier = new ProductSupplier();
            productSupplier.Tercero_Id = GetValidatedInput("Nuevo ID del proveedor (dejar en blanco para mantener el actual): ", allowEmpty: true);
            productSupplier.Producto_Id = GetValidatedInput("Nuevo ID del producto (dejar en blanco para mantener el actual): ", allowEmpty: true);

            try
            {
                _service.ActualizarProductSupplier(oldTerceroId, oldProductoId, productSupplier);
                ShowSuccessMessage("Proveedor de producto actualizado exitosamente.");
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Error al actualizar el proveedor de producto: {ex.Message}");
            }
        }

        private void EliminarProductSupplier()
        {
            ShowHeader("ELIMINAR PROVEEDOR DE PRODUCTO");
            string terceroId = GetValidatedInput("ID del proveedor a eliminar: ");
            string productoId = GetValidatedInput("ID del producto a eliminar: ");

            try
            {
                _service.EliminarProductSupplier(terceroId, productoId);
                ShowSuccessMessage("Proveedor de producto eliminado exitosamente.");
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Error al eliminar el proveedor de producto: {ex.Message}");
            }
        }
    }
}
