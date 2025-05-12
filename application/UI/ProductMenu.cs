using System;
using SGCI_app.application.services;
using SGCI_app.domain.Entities;
using SGCI_app.domain.Factory;
using SGCI_app.infrastructure.postgres;

namespace SGCI_app.application.UI
{
    public class ProductMenu : BaseMenu
    {
        private readonly ProductService _service;

        public ProductMenu() : base(showIntro: false)
        {
            string connStr = "Host=localhost;database=sgci;Port=5432;Username=postgres;Password=1219;Pooling=true";
            var factory = new ConexDBFactory(connStr);
            _service = new ProductService(factory.CrearProductRepository());
        }

        public override void ShowMenu()
        {
            while (true)
            {
                ShowHeader("GESTIÓN DE PRODUCTOS");
                Console.WriteLine("1. Crear Producto");
                Console.WriteLine("2. Listar Productos");
                Console.WriteLine("3. Actualizar Producto");
                Console.WriteLine("4. Eliminar Producto");
                Console.WriteLine("0. Volver al menú principal");
                DrawSeparator();

                int option = GetValidatedIntInput("Seleccione una opción: ", 0, 4);
                switch (option)
                {
                    case 1:
                        CrearProducto();
                        break;
                    case 2:
                        ListarProductos();
                        break;
                    case 3:
                        ActualizarProducto();
                        break;
                    case 4:
                        EliminarProducto();
                        break;
                    case 0:
                        return;
                }
            }
        }

        private void CrearProducto()
        {
            ShowHeader("CREAR PRODUCTO");
            var product = new Product();

            product.Id = GetValidatedInput("ID del producto: ");
            product.Nombre = GetValidatedInput("Nombre del producto: ");
            product.Stock = GetValidatedIntInput("Stock: ");
            product.StockMin = GetValidatedIntInput("Stock mínimo: ");
            product.StockMax = GetValidatedIntInput("Stock máximo: ");
            product.FechaCreacion = DateTime.Now;
            product.FechaActualizacion = DateTime.Now;
            product.CodigoBarras = GetValidatedInput("Código de barras: ");

            try
            {
                _service.CreateProduct(product);
                ShowSuccessMessage("Producto creado exitosamente.");
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Error al crear el producto: {ex.Message}");
            }
        }

        private void ListarProductos()
        {
            ShowHeader("LISTA DE PRODUCTOS");
            try
            {
                _service.GetAllProducts();
                ShowInfoMessage("Listado de productos completado.");
                Console.WriteLine();
                Console.Write("Presione cualquier tecla para continuar...");
                Console.ReadKey(true);
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Error al obtener los productos: {ex.Message}");
            }
        }

        private void ActualizarProducto()
        {
            ShowHeader("ACTUALIZAR PRODUCTO");
            string id = GetValidatedInput("ID del producto a actualizar: ");
            var product = new Product { Id = id };

            product.Nombre = GetValidatedInput("Nuevo nombre (dejar en blanco para mantener el actual): ", allowEmpty: true);
            
            string stockInput = GetValidatedInput("Nuevo stock (dejar en blanco para mantener el actual): ", allowEmpty: true);
            if (int.TryParse(stockInput, out int stock)) product.Stock = stock;

            string minInput = GetValidatedInput("Nuevo stock mínimo (dejar en blanco para mantener el actual): ", allowEmpty: true);
            if (int.TryParse(minInput, out int min)) product.StockMin = min;

            string maxInput = GetValidatedInput("Nuevo stock máximo (dejar en blanco para mantener el actual): ", allowEmpty: true);
            if (int.TryParse(maxInput, out int max)) product.StockMax = max;

            string dateInput = GetValidatedInput("Nueva fecha de actualización (YYYY-MM-DD, dejar en blanco para fecha actual): ", allowEmpty: true);
            if (!string.IsNullOrEmpty(dateInput) && DateTime.TryParse(dateInput, out DateTime updated))
                product.FechaActualizacion = updated;
            else
                product.FechaActualizacion = DateTime.Now;

            product.CodigoBarras = GetValidatedInput("Nuevo código de barras (dejar en blanco para mantener el actual): ", allowEmpty: true);

            try
            {
                _service.UpdateProduct(product);
                ShowSuccessMessage("Producto actualizado exitosamente.");
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Error al actualizar el producto: {ex.Message}");
            }
        }

        private void EliminarProducto()
        {
            ShowHeader("ELIMINAR PRODUCTO");
            string id = GetValidatedInput("ID del producto a eliminar: ");

            try
            {
                _service.DeleteProduct(id);
                ShowSuccessMessage("Producto eliminado exitosamente.");
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Error al eliminar el producto: {ex.Message}");
            }
        }
    }
}
