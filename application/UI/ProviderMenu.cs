using System;
using SGCI_app.application.services;
using SGCI_app.domain.DTO;
using SGCI_app.domain.Factory;
using SGCI_app.infrastructure.postgres;

namespace SGCI_app.application.UI
{
    public class ProviderMenu : BaseMenu
    {
        private readonly ProviderService _service;

        public ProviderMenu() : base(showIntro: false)
        {
            string connStr = "Host=localhost;database=sgci;Port=5432;Username=postgres;Password=1219;Pooling=true";
            var factory = new ConexDBFactory(connStr);
            _service = new ProviderService(factory.CrearProviderRepository());
        }

        public override void ShowMenu()
        {
            while (true)
            {
                ShowHeader("GESTIÓN DE PROVEEDORES");
                Console.WriteLine("1. Crear Proveedor");
                Console.WriteLine("2. Listar Proveedores");
                Console.WriteLine("3. Actualizar Proveedor");
                Console.WriteLine("4. Eliminar Proveedor");
                Console.WriteLine("0. Volver al menú principal");
                DrawSeparator();

                int option = GetValidatedIntInput("Seleccione una opción: ", 0, 4);
                switch (option)
                {
                    case 1:
                        CrearProveedor();
                        break;
                    case 2:
                        ListarProveedores();
                        break;
                    case 3:
                        ActualizarProveedor();
                        break;
                    case 4:
                        EliminarProveedor();
                        break;
                    case 0:
                        return;
                }
            }
        }

        private void CrearProveedor()
        {
            ShowHeader("CREAR NUEVO PROVEEDOR");
            
            // Datos de dirección
            Console.WriteLine("\nDatos de dirección:");
            string calle = GetValidatedInput("Calle: ");
            string numeroEdificio = GetValidatedInput("Número de edificio: ");
            string codigoPostal = GetValidatedInput("Código postal: ");
            int ciudadId = GetValidatedIntInput("ID de ciudad: ");
            string infoAdicional = GetValidatedInput("Información adicional: ", allowEmpty: true);

            // Datos personales
            Console.WriteLine("\nDatos personales:");
            string nombre = GetValidatedInput("Nombre: ");
            string apellidos = GetValidatedInput("Apellidos: ");
            string email = GetValidatedInput("Email: ");
            int tipoTerceroId = GetValidatedIntInput("ID del tipo de tercero: ");
            int tipoDocId = GetValidatedIntInput("ID del tipo de documento: ");

            // Datos de proveedor
            Console.WriteLine("\nDatos de proveedor:");
            double descuento = GetValidatedDoubleInput("Descuento (%): ");
            int diaPago = GetValidatedIntInput("Día de pago: ");

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
                ShowSuccessMessage("Proveedor creado exitosamente.");
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Error al crear el proveedor: {ex.Message}");
            }
        }

        private void ListarProveedores()
        {
            ShowHeader("LISTA DE PROVEEDORES");
            try
            {
                var proveedores = _service.GetAllProviders();
                if (proveedores.Count == 0)
                {
                    ShowInfoMessage("No hay proveedores registrados.");
                }
                else
                {
                    foreach (var proveedor in proveedores)
                    {
                        Console.WriteLine($"ID: {proveedor.Id}");
                        Console.WriteLine($"Nombre: {proveedor.Nombre}");
                        Console.WriteLine($"Descuento: {proveedor.Provider.Descuento}%");
                        Console.WriteLine($"Día de Pago: {proveedor.Provider.DiaPago}");
                        Console.WriteLine($"Id Tercero Proveedor: {proveedor.Provider.Tercero_Id}");
                        Console.WriteLine("------------------------");
                    }
                }
                ShowInfoMessage("Listado de proveedores completado.");
                Console.WriteLine();
                Console.Write("Presione cualquier tecla para continuar...");
                Console.ReadKey(true);
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Error al listar los proveedores: {ex.Message}");
            }
        }

        private void ActualizarProveedor()
        {
            ShowHeader("ACTUALIZAR PROVEEDOR");
            int proveedorId = GetValidatedIntInput("ID del proveedor a actualizar: ");

            // Datos personales
            Console.WriteLine("\nDatos personales:");
            string nombre = GetValidatedInput("Nuevo nombre (dejar en blanco para mantener el actual): ", allowEmpty: true);
            string apellidos = GetValidatedInput("Nuevos apellidos (dejar en blanco para mantener los actuales): ", allowEmpty: true);
            string email = GetValidatedInput("Nuevo email (dejar en blanco para mantener el actual): ", allowEmpty: true);
            int tipoTerceroId = GetValidatedIntInput("Nuevo ID del tipo de tercero: ");
            int tipoDocId = GetValidatedIntInput("Nuevo ID del tipo de documento: ");

            // Datos de proveedor
            Console.WriteLine("\nDatos de proveedor:");
            double descuento = GetValidatedDoubleInput("Nuevo descuento (%): ");
            int diaPago = GetValidatedIntInput("Nuevo día de pago: ");

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
                ShowSuccessMessage("Proveedor actualizado exitosamente.");
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Error al actualizar el proveedor: {ex.Message}");
            }
        }

        private void EliminarProveedor()
        {
            ShowHeader("ELIMINAR PROVEEDOR");
            int proveedorId = GetValidatedIntInput("ID del proveedor a eliminar: ");
            
            try
            {
                _service.DeleteProvider(proveedorId);
                ShowSuccessMessage("Proveedor eliminado exitosamente.");
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Error al eliminar el proveedor: {ex.Message}");
            }
        }

        private double GetValidatedDoubleInput(string prompt)
        {
            while (true)
            {
                string input = GetValidatedInput(prompt);
                if (double.TryParse(input, out double value))
                    return value;
                ShowErrorMessage("Por favor, ingrese un número válido.");
            }
        }
    }
} 