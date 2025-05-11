using System;
using SGCI_app.application.services;
using SGCI_app.domain.Entities;
using SGCI_app.infrastructure.postgres;

namespace SGCI_app.application.UI
{
    public class PurchaseMenu
    {
        private readonly PurchaseService _purchaseService;
        private readonly PurchaseDetailService _purchaseDetailService;

        public PurchaseMenu()
        {
            string connStr = "Host=localhost;database=sgci;Port=5432;Username=postgres;Password=juan1374;Pooling=true";
            var factory = new ConexDBFactory(connStr);
            _purchaseService = new PurchaseService(factory.CrearPurchaseRepository());
            _purchaseDetailService = new PurchaseDetailService(factory.CrearPurchaseDetailRepository());
        }

        public void ShowMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== GESTIÓN DE COMPRAS ===");
                Console.WriteLine("1. Listar Compras");
                Console.WriteLine("2. Crear Nueva Compra");
                Console.WriteLine("3. Actualizar Compra");
                Console.WriteLine("4. Eliminar Compra");
                Console.WriteLine("5. Gestionar Detalles de Compra");
                Console.WriteLine("0. Volver al Menú Principal");
                Console.Write("\nSeleccione una opción: ");

                string? input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        ListarCompras();
                        break;
                    case "2":
                        CrearCompra();
                        break;
                    case "3":
                        ActualizarCompra();
                        break;
                    case "4":
                        EliminarCompra();
                        break;
                    case "5":
                        GestionarDetallesCompra();
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Opción no válida. Presione cualquier tecla para continuar...");
                        Console.ReadKey();
                        break;
                }
            }
        }

        private void ListarCompras()
        {
            Console.Clear();
            Console.WriteLine("=== LISTADO DE COMPRAS ===");
            _purchaseService.MostrarTodos();
            Console.WriteLine("\nPresione cualquier tecla para continuar...");
            Console.ReadKey();
        }

        private void CrearCompra()
        {
            Console.Clear();
            Console.WriteLine("=== CREAR NUEVA COMPRA ===");
            
            try
            {
                var compra = new Purchase();
                
                Console.Write("ID del Proveedor: ");
                compra.TerceroProveedor_Id = Console.ReadLine();
                
                Console.Write("Fecha (YYYY-MM-DD): ");
                if (DateTime.TryParse(Console.ReadLine(), out DateTime fecha))
                {
                    compra.Fecha = fecha;
                }
                
                Console.Write("ID del Empleado: ");
                compra.TerceroEmpleado_Id = Console.ReadLine();
                
                Console.Write("Documento de Compra: ");
                compra.DocCompra = Console.ReadLine();

                _purchaseService.CrearCompra(compra);
                Console.WriteLine("Compra creada exitosamente.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            Console.ReadKey();
        }

        private void ActualizarCompra()
        {
            Console.Clear();
            Console.WriteLine("=== ACTUALIZAR COMPRA ===");
            
            try
            {
                Console.Write("Ingrese el ID de la compra a actualizar: ");
                if (int.TryParse(Console.ReadLine(), out int id))
                {
                    var compra = new Purchase { Id = id };
                    
                    Console.Write("Nuevo ID del Proveedor: ");
                    compra.TerceroProveedor_Id = Console.ReadLine();
                    
                    Console.Write("Nueva Fecha (YYYY-MM-DD): ");
                    if (DateTime.TryParse(Console.ReadLine(), out DateTime fecha))
                    {
                        compra.Fecha = fecha;
                    }
                    
                    Console.Write("Nuevo ID del Empleado: ");
                    compra.TerceroEmpleado_Id = Console.ReadLine();
                    
                    Console.Write("Nuevo Documento de Compra: ");
                    compra.DocCompra = Console.ReadLine();

                    _purchaseService.ActualizarCompra(compra);
                    Console.WriteLine("Compra actualizada exitosamente.");
                }
                else
                {
                    Console.WriteLine("ID inválido.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            Console.ReadKey();
        }

        private void EliminarCompra()
        {
            Console.Clear();
            Console.WriteLine("=== ELIMINAR COMPRA ===");
            
            Console.Write("Ingrese el ID de la compra a eliminar: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                _purchaseService.EliminarCompra(id);
                Console.WriteLine("Compra eliminada exitosamente.");
            }
            else
            {
                Console.WriteLine("ID inválido.");
            }
            Console.ReadKey();
        }

        private void GestionarDetallesCompra()
        {
            Console.Clear();
            Console.WriteLine("=== GESTIÓN DE DETALLES DE COMPRA ===");
            Console.WriteLine("1. Listar Detalles");
            Console.WriteLine("2. Agregar Detalle");
            Console.WriteLine("3. Actualizar Detalle");
            Console.WriteLine("4. Eliminar Detalle");
            Console.WriteLine("0. Volver");
            Console.Write("\nSeleccione una opción: ");

            string? input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    ListarDetalles();
                    break;
                case "2":
                    AgregarDetalle();
                    break;
                case "3":
                    ActualizarDetalle();
                    break;
                case "4":
                    EliminarDetalle();
                    break;
            }
        }

        private void ListarDetalles()
        {
            Console.Clear();
            Console.WriteLine("=== LISTADO DE DETALLES DE COMPRA ===");
            _purchaseDetailService.MostrarTodos();
            Console.WriteLine("\nPresione cualquier tecla para continuar...");
            Console.ReadKey();
        }

        private void AgregarDetalle()
        {
            Console.Clear();
            Console.WriteLine("=== AGREGAR DETALLE DE COMPRA ===");
            
            var detalle = new PurchaseDetail();
            
            // Validar ID de la Compra
            bool compraValida = false;
            do
            {
                Console.Write("ID de la Compra: ");
                string? compraInput = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(compraInput))
                {
                    Console.WriteLine("Error: El ID de la compra no puede estar vacío.");
                    continue;
                }
                if (!int.TryParse(compraInput, out int compraId))
                {
                    Console.WriteLine("Error: El ID de la compra debe ser un número válido.");
                    continue;
                }
                detalle.Compra_Id = compraId;
                compraValida = true;
            } while (!compraValida);
            
            // Validar ID del Producto
            bool productoValido = false;
            do
            {
                Console.Write("ID del Producto: ");
                string? productoInput = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(productoInput))
                {
                    Console.WriteLine("Error: El ID del producto no puede estar vacío.");
                    continue;
                }
                detalle.Producto_Id = productoInput;
                productoValido = true;
            } while (!productoValido);
            
            // Validar cantidad
            bool cantidadValida = false;
            do
            {
                Console.Write("Cantidad: ");
                string? cantidadInput = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(cantidadInput))
                {
                    Console.WriteLine("Error: La cantidad no puede estar vacía.");
                    continue;
                }
                if (!int.TryParse(cantidadInput, out int cantidad) || cantidad <= 0)
                {
                    Console.WriteLine("Error: La cantidad debe ser un número positivo.");
                    continue;
                }
                detalle.Cantidad = cantidad;
                cantidadValida = true;
            } while (!cantidadValida);
            
            // Validar valor
            bool valorValido = false;
            do
            {
                Console.Write("Valor: ");
                string? valorInput = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(valorInput))
                {
                    Console.WriteLine("Error: El valor no puede estar vacío.");
                    continue;
                }
                if (!int.TryParse(valorInput, out int valor) || valor < 0)
                {
                    Console.WriteLine("Error: El valor debe ser un número no negativo.");
                    continue;
                }
                detalle.Valor = valor;
                valorValido = true;
            } while (!valorValido);

            detalle.Fecha = DateTime.Now;

            try
            {
                _purchaseDetailService.CrearDetalleCompra(detalle);
                Console.WriteLine("Detalle de compra creado exitosamente.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al crear el detalle de compra: {ex.Message}");
            }
            Console.WriteLine("\nPresione cualquier tecla para continuar...");
            Console.ReadKey();
        }

        private void ActualizarDetalle()
        {
            Console.Clear();
            Console.WriteLine("=== ACTUALIZAR DETALLE DE COMPRA ===");
            
            Console.Write("Ingrese el ID del detalle a actualizar: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                var detalle = new PurchaseDetail { Id = id };
                
                Console.Write("Nuevo ID de la Compra: ");
                if (int.TryParse(Console.ReadLine(), out int compraId))
                {
                    detalle.Compra_Id = compraId;
                }
                
                Console.Write("Nuevo ID del Producto: ");
                detalle.Producto_Id = Console.ReadLine();
                
                Console.Write("Nueva Cantidad: ");
                if (int.TryParse(Console.ReadLine(), out int cantidad))
                {
                    detalle.Cantidad = cantidad;
                }
                
                Console.Write("Nuevo Valor: ");
                if (int.TryParse(Console.ReadLine(), out int valor))
                {
                    detalle.Valor = valor;
                }

                detalle.Fecha = DateTime.Now;

                _purchaseDetailService.ActualizarDetalleCompra(detalle);
                Console.WriteLine("Detalle de compra actualizado exitosamente.");
            }
            else
            {
                Console.WriteLine("ID inválido.");
            }
            Console.ReadKey();
        }

        private void EliminarDetalle()
        {
            Console.Clear();
            Console.WriteLine("=== ELIMINAR DETALLE DE COMPRA ===");
            
            Console.Write("Ingrese el ID del detalle a eliminar: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                _purchaseDetailService.EliminarDetalleCompra(id);
                Console.WriteLine("Detalle de compra eliminado exitosamente.");
            }
            else
            {
                Console.WriteLine("ID inválido.");
            }
            Console.ReadKey();
        }
    }
}