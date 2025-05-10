using System;
using SGCI_app.application.services;
using SGCI_app.domain.DTO;
using SGCI_app.domain.Factory;
using SGCI_app.infrastructure.postgres;

namespace SGCI_app.application.UI
{
    public class ProviderMenu
    {
        private readonly ProviderService _service;

        public ProviderMenu()
        {
            string connStr = "Host=localhost;database=sgci;Port=5432;Username=postgres;Password=1219;Pooling=true";
            var factory = new ConexDBFactory(connStr);
            _service = new ProviderService(factory.CrearProviderRepository());
        }

        public void ShowMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== GESTIÓN DE PROVEEDORES ===");
                Console.WriteLine("1. Crear Proveedor");
                Console.WriteLine("2. Listar Proveedores");
                Console.WriteLine("3. Actualizar Proveedor");
                Console.WriteLine("4. Eliminar Proveedor");
                Console.WriteLine("0. Volver al Menú Principal");
                Console.Write("\nSeleccione una opción: ");

                string? input = Console.ReadLine();

                if (string.IsNullOrEmpty(input))
                {
                    Console.WriteLine("Por favor, ingrese una opción válida.");
                    Console.ReadKey();
                    continue;
                }

                switch (input)
                {
                    case "1":
                        CrearProveedor();
                        break;
                    case "2":
                        ListarProveedores();
                        break;
                    case "3":
                        ActualizarProveedor();
                        break;
                    case "4":
                        EliminarProveedor();
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

        private void CrearProveedor()
        {
            Console.Clear();
            Console.WriteLine("=== CREAR NUEVO PROVEEDOR ===");
            
            // Datos de dirección
            Console.WriteLine("\nDatos de dirección:");
            Console.Write("Calle: ");
            string? calle = Console.ReadLine();
            
            Console.Write("Número de edificio: ");
            string? numeroEdificio = Console.ReadLine();
            
            Console.Write("Código postal: ");
            string? codigoPostal = Console.ReadLine();
            
            Console.Write("ID de ciudad: ");
            if (!int.TryParse(Console.ReadLine(), out int ciudadId))
            {
                Console.WriteLine("ID de ciudad inválido.");
                Console.ReadKey();
                return;
            }
            
            Console.Write("Información adicional: ");
            string? infoAdicional = Console.ReadLine();

            // Datos personales
            Console.WriteLine("\nDatos personales:");
            Console.Write("Nombre: ");
            string? nombre = Console.ReadLine();
            
            Console.Write("Apellidos: ");
            string? apellidos = Console.ReadLine();
            
            Console.Write("Email: ");
            string? email = Console.ReadLine();
            
            Console.Write("ID del tipo de tercero: ");
            if (!int.TryParse(Console.ReadLine(), out int tipoTerceroId))
            {
                Console.WriteLine("ID de tipo de tercero inválido.");
                Console.ReadKey();
                return;
            }
            
            Console.Write("ID del tipo de documento: ");
            if (!int.TryParse(Console.ReadLine(), out int tipoDocId))
            {
                Console.WriteLine("ID de tipo de documento inválido.");
                Console.ReadKey();
                return;
            }

            // Datos de proveedor
            Console.WriteLine("\nDatos de proveedor:");
            Console.Write("Descuento (%): ");
            if (!double.TryParse(Console.ReadLine(), out double descuento))
            {
                Console.WriteLine("Descuento inválido.");
                Console.ReadKey();
                return;
            }
            
            Console.Write("Día de pago: ");
            if (!int.TryParse(Console.ReadLine(), out int diaPago))
            {
                Console.WriteLine("Día de pago inválido.");
                Console.ReadKey();
                return;
            }

            var proveedor = new DtoProvider 
            { 
                Address = new DtoAddress
                {
                    Calle = calle,
                    NumeroEdificio = numeroEdificio,
                    CodigoPostal = codigoPostal,
                    Ciudad_Id = ciudadId,
                    InfoAdicional = infoAdicional
                },
                Nombre = nombre,
                Apellidos = apellidos,
                Email = email,
                TipoTercero_id = tipoTerceroId,
                TipoDoc_id = tipoDocId,
                Provider = new DtoProv
                {
                    Descuento = descuento,
                    DiaPago = diaPago
                }
            };
            
            try
            {
                _service.CreateProvider(proveedor);
                Console.WriteLine("Proveedor creado exitosamente.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al crear el proveedor: {ex.Message}");
            }
            
            Console.ReadKey();
        }

        private void ListarProveedores()
        {
            Console.Clear();
            Console.WriteLine("=== LISTA DE PROVEEDORES ===");
            
            try
            {
                var proveedores = _service.GetAllProviders();
                if (proveedores.Count == 0)
                {
                    Console.WriteLine("No hay proveedores registrados.");
                }
                else
                {
                    foreach (var proveedor in proveedores)
                    {
                        Console.WriteLine($"ID: {proveedor.Id}");
                        Console.WriteLine($"Nombre: {proveedor.Nombre}");
                        Console.WriteLine($"Descuento: {proveedor.Provider.Descuento}%");
                        Console.WriteLine($"Día de Pago: {proveedor.Provider.DiaPago}");
                        Console.WriteLine("------------------------");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al listar los proveedores: {ex.Message}");
            }
            
            Console.WriteLine("\nPresione cualquier tecla para continuar...");
            Console.ReadKey();
        }

        private void ActualizarProveedor()
        {
            Console.Clear();
            Console.WriteLine("=== ACTUALIZAR PROVEEDOR ===");
            
            Console.Write("ID del proveedor a actualizar: ");
            if (!int.TryParse(Console.ReadLine(), out int proveedorId))
            {
                Console.WriteLine("ID de proveedor inválido.");
                Console.ReadKey();
                return;
            }

            // Datos personales
            Console.WriteLine("\nDatos personales:");
            Console.Write("Nuevo nombre: ");
            string? nombre = Console.ReadLine();
            
            Console.Write("Nuevos apellidos: ");
            string? apellidos = Console.ReadLine();
            
            Console.Write("Nuevo email: ");
            string? email = Console.ReadLine();
            
            Console.Write("Nuevo ID del tipo de tercero: ");
            if (!int.TryParse(Console.ReadLine(), out int tipoTerceroId))
            {
                Console.WriteLine("ID de tipo de tercero inválido.");
                Console.ReadKey();
                return;
            }
            
            Console.Write("Nuevo ID del tipo de documento: ");
            if (!int.TryParse(Console.ReadLine(), out int tipoDocId))
            {
                Console.WriteLine("ID de tipo de documento inválido.");
                Console.ReadKey();
                return;
            }

            // Datos de proveedor
            Console.WriteLine("\nDatos de proveedor:");
            Console.Write("Nuevo descuento (%): ");
            if (!double.TryParse(Console.ReadLine(), out double descuento))
            {
                Console.WriteLine("Descuento inválido.");
                Console.ReadKey();
                return;
            }
            
            Console.Write("Nuevo día de pago: ");
            if (!int.TryParse(Console.ReadLine(), out int diaPago))
            {
                Console.WriteLine("Día de pago inválido.");
                Console.ReadKey();
                return;
            }

            var proveedor = new DtoProvider 
            { 
                Nombre = nombre,
                Apellidos = apellidos,
                Email = email,
                TipoTercero_id = tipoTerceroId,
                TipoDoc_id = tipoDocId,
                Provider = new DtoProv
                {
                    Descuento = descuento,
                    DiaPago = diaPago
                }
            };
            
            try
            {
                _service.UpdateProvider(proveedorId, proveedor);
                Console.WriteLine("Proveedor actualizado exitosamente.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al actualizar el proveedor: {ex.Message}");
            }
            
            Console.ReadKey();
        }

        private void EliminarProveedor()
        {
            Console.Clear();
            Console.WriteLine("=== ELIMINAR PROVEEDOR ===");
            
            Console.Write("ID del proveedor a eliminar: ");
            if (!int.TryParse(Console.ReadLine(), out int proveedorId))
            {
                Console.WriteLine("ID de proveedor inválido.");
                Console.ReadKey();
                return;
            }
            
            try
            {
                _service.DeleteProvider(proveedorId);
                Console.WriteLine("Proveedor eliminado exitosamente.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al eliminar el proveedor: {ex.Message}");
            }
            
            Console.ReadKey();
        }
    }
} 