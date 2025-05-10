using System;
using SGCI_app.domain.DTO;
using SGCI_app.domain.Factory;
using SGCI_app.infrastructure.postgres;
using SGCI_app.application.services;

namespace SGCI_app.application.UI
{
    public class ClientMenu
    {
        private readonly ClientService _service;

        public ClientMenu()
        {
            string connStr = "Host=localhost;database=sgci;Port=5432;Username=postgres;Password=campus2023;Pooling=true";
            var factory = new ConexDBFactory(connStr);
            _service = new ClientService(factory.CrearClientRepository());
        }

        public void ShowMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== GESTIÓN DE CLIENTES DTO ===");
                Console.WriteLine("1. Crear Cliente DTO");
                Console.WriteLine("2. Listar Clientes DTO");
                Console.WriteLine("3. Actualizar Cliente DTO");
                Console.WriteLine("4. Eliminar Cliente DTO");
                Console.WriteLine("0. Volver al menú principal");
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
                        CrearDtoClient();
                        break;
                    case "2":
                        ListarDtoClients();
                        break;
                    case "3":
                        ActualizarDtoClient();
                        break;
                    case "4":
                        EliminarDtoClient();
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

        private void CrearDtoClient()
        {
            Console.Clear();
            Console.WriteLine("=== CREAR NUEVO CLIENTE DTO ===");
            
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

            // Datos de cliente
            Console.WriteLine("\nDatos de cliente:");
            Console.Write("Fecha de nacimiento (YYYY-MM-DD): ");
            if (!DateTime.TryParse(Console.ReadLine(), out DateTime fechaNac))
            {
                Console.WriteLine("Fecha inválida.");
                Console.ReadKey();
                return;
            }
            
            Console.Write("Fecha de última compra (YYYY-MM-DD): ");
            if (!DateTime.TryParse(Console.ReadLine(), out DateTime fechaUltCompra))
            {
                Console.WriteLine("Fecha inválida.");
                Console.ReadKey();
                return;
            }

            var dtoClient = new DtoClient 
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
                Client = new DtoCli
                {
                    FechaNacimiento = fechaNac,
                    FechaUltimaCompra = fechaUltCompra
                }
            };
            
            try
            {
                _service.CrearClienteDto(dtoClient);
                Console.WriteLine("Cliente DTO creado exitosamente.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al crear el cliente DTO: {ex.Message}");
            }
            
            Console.ReadKey();
        }

        private void ListarDtoClients()
        {
            Console.Clear();
            Console.WriteLine("=== LISTA DE CLIENTES DTO ===");
            
            try
            {
                _service.MostrarTodos();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al listar los clientes DTO: {ex.Message}");
            }
            
            Console.WriteLine("\nPresione cualquier tecla para continuar...");
            Console.ReadKey();
        }

        private void ActualizarDtoClient()
        {
            Console.Clear();
            Console.WriteLine("=== ACTUALIZAR CLIENTE DTO ===");
            
            Console.Write("ID del cliente a actualizar: ");
            if (!int.TryParse(Console.ReadLine(), out int clienteId))
            {
                Console.WriteLine("ID de cliente inválido.");
                Console.ReadKey();
                return;
            }

            Console.Write("Nuevo nombre: ");
            string? nuevoNombre = Console.ReadLine();
            
            Console.Write("Nuevos apellidos: ");
            string? nuevosApellidos = Console.ReadLine();
            
            Console.Write("Nuevo email: ");
            string? nuevoEmail = Console.ReadLine();
            
            Console.Write("Nuevo ID de tipo de tercero: ");
            if (!int.TryParse(Console.ReadLine(), out int nuevoTipoTerceroId))
            {
                Console.WriteLine("ID de tipo de tercero inválido.");
                Console.ReadKey();
                return;
            }
            
            Console.Write("Nuevo ID de tipo de documento: ");
            if (!int.TryParse(Console.ReadLine(), out int nuevoTipoDocId))
            {
                Console.WriteLine("ID de tipo de documento inválido.");
                Console.ReadKey();
                return;
            }

            var dtoClient = new DtoClient 
            { 
                Id = clienteId,
                Nombre = nuevoNombre,
                Apellidos = nuevosApellidos,
                Email = nuevoEmail,
                TipoTercero_id = nuevoTipoTerceroId,
                TipoDoc_id = nuevoTipoDocId
            };
            
            try
            {
                _service.ActualizarDatosPersonalesCliente(clienteId, dtoClient);
                Console.WriteLine("Cliente DTO actualizado exitosamente.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al actualizar el cliente DTO: {ex.Message}");
            }
            
            Console.ReadKey();
        }

        private void EliminarDtoClient()
        {
            Console.Clear();
            Console.WriteLine("=== ELIMINAR CLIENTE DTO ===");
            
            Console.Write("ID del cliente a eliminar: ");
            if (!int.TryParse(Console.ReadLine(), out int clienteId))
            {
                Console.WriteLine("ID de cliente inválido.");
                Console.ReadKey();
                return;
            }
            
            try
            {
                _service.EliminarCliente(clienteId);
                Console.WriteLine("Cliente DTO eliminado exitosamente.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al eliminar el cliente DTO: {ex.Message}");
            }
            
            Console.ReadKey();
        }
    }
} 