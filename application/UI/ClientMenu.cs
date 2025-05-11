using System;
using System.Linq;
using SGCI_app.domain.Entities;
using SGCI_app.domain.DTO;
using SGCI_app.domain.Factory;
using SGCI_app.infrastructure.postgres;
using SGCI_app.application.Services;
using SGCI_app.application.services;

namespace SGCI_app.application.UI
{
    public class ClientMenu : BaseMenu
    {
        private readonly ClientService _service;

        public ClientMenu() : base(showIntro: false)
        {
            string connStr = "Host=localhost;database=sgci;Port=5432;Username=postgres;Password=1219;Pooling=true";
            var factory = new ConexDBFactory(connStr);
            _service = new ClientService(factory.CrearClientRepository());
        }

        public override void ShowMenu()
        {
            while (true)
            {
                ShowHeader("GESTIÓN DE CLIENTES DTO");
                Console.WriteLine("1. Crear Cliente DTO");
                Console.WriteLine("2. Listar Clientes DTO");
                Console.WriteLine("3. Actualizar Cliente DTO");
                Console.WriteLine("4. Eliminar Cliente DTO");
                Console.WriteLine("0. Volver al menú principal");
                DrawSeparator();

                int option = GetValidatedIntInput("Seleccione una opción: ", 0, 4);
                switch (option)
                {
                    case 1:
                        CrearDtoClient();
                        break;
                    case 2:
                        ListarDtoClients();
                        break;
                    case 3:
                        ActualizarDtoClient();
                        break;
                    case 4:
                        EliminarDtoClient();
                        break;
                    case 0:
                        return;
                }
            }
        }

        private void CrearDtoClient()
        {
            ShowHeader("CREAR NUEVO CLIENTE DTO");

            // Datos de dirección
            string calle = GetValidatedInput("Calle: ");
            string numeroEdificio = GetValidatedInput("Número de edificio: ");
            string codigoPostal = GetValidatedInput("Código postal: ");
            int ciudadId = GetValidatedIntInput("ID de ciudad: ");
            string infoAdicional = GetValidatedInput("Información adicional: ", allowEmpty: true);

            // Datos personales
            string nombre = GetValidatedInput("Nombre: ");
            string apellidos = GetValidatedInput("Apellidos: ");
            string email = GetValidatedInput("Email: ");
            int tipoTerceroId = GetValidatedIntInput("ID del tipo de tercero: ");
            int tipoDocId = GetValidatedIntInput("ID del tipo de documento: ");

            // Datos de cliente
            DateTime fechaNac = DateTime.Parse(GetValidatedInput("Fecha de nacimiento (YYYY-MM-DD): "));
            DateTime fechaUltCompra = DateTime.Parse(GetValidatedInput("Fecha de última compra (YYYY-MM-DD): "));

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
                ShowSuccessMessage("Cliente DTO creado exitosamente.");
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Error al crear el cliente DTO: {ex.Message}");
            }
        }

        private void ListarDtoClients()
        {
            ShowHeader("LISTA DE CLIENTES DTO");
            try
            {
                _service.MostrarTodos();
                ShowInfoMessage("Listado de clientes completado.");
                Console.WriteLine();
                Console.Write("Presione cualquier tecla para continuar...");
                Console.ReadKey(true);
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Error al listar los clientes DTO: {ex.Message}");
            }
        }

        private void ActualizarDtoClient()
        {
            ShowHeader("ACTUALIZAR CLIENTE DTO");
            int clienteId = GetValidatedIntInput("ID del cliente a actualizar: ");
            string nuevoNombre = GetValidatedInput("Nuevo nombre: ");
            string nuevosApellidos = GetValidatedInput("Nuevos apellidos: ");
            string nuevoEmail = GetValidatedInput("Nuevo email: ");
            int nuevoTipoTerceroId = GetValidatedIntInput("Nuevo ID de tipo de tercero: ");
            int nuevoTipoDocId = GetValidatedIntInput("Nuevo ID de tipo de documento: ");

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
                ShowSuccessMessage("Cliente DTO actualizado exitosamente.");
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Error al actualizar el cliente DTO: {ex.Message}");
            }
        }

        private void EliminarDtoClient()
        {
            ShowHeader("ELIMINAR CLIENTE DTO");
            int clienteId = GetValidatedIntInput("ID del cliente a eliminar: ");

            try
            {
                _service.EliminarCliente(clienteId);
                ShowSuccessMessage("Cliente DTO eliminado exitosamente.");
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Error al eliminar el cliente DTO: {ex.Message}");
            }
        }
    }
}
