using SGCI_app.domain.Entities;
using SGCI_app.infrastructure.postgres;
using SGCI_app.application.services;

namespace SGCI_app.application.UI
{
    public class CountryMenu : BaseMenu
    {
        private readonly CountryService _service;

        public CountryMenu() : base(showIntro: false)
        {
            string connStr = "Host=localhost;database=sgci;Port=5432;Username=postgres;Password=1219;Pooling=true";
            var factory = new ConexDBFactory(connStr);
            _service = new CountryService(factory.CrearCountryRepository());
        }

        public override void ShowMenu()
        {
            while (true)
            {
                ShowHeader("GESTIÓN DE PAÍSES");
                Console.WriteLine("1. Crear País");
                Console.WriteLine("2. Listar Países");
                Console.WriteLine("3. Actualizar País");
                Console.WriteLine("4. Eliminar País");
                Console.WriteLine("0. Volver al menú principal");
                DrawSeparator();

                int option = GetValidatedIntInput("Seleccione una opción: ", 0, 4);
                switch (option)
                {
                    case 1:
                        CrearPais();
                        break;
                    case 2:
                        ListarPaises();
                        break;
                    case 3:
                        ActualizarPais();
                        break;
                    case 4:
                        EliminarPais();
                        break;
                    case 0:
                        return;
                }
            }
        }

        private void CrearPais()
        {
            ShowHeader("CREAR NUEVO PAÍS");
            string nombre = GetValidatedInput("Nombre del país: ");
            var pais = new Country { Nombre = nombre };

            try
            {
                _service.CrearCountry(pais);
                ShowSuccessMessage("País creado exitosamente.");
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Error al crear el país: {ex.Message}");
            }
        }

        private void ListarPaises()
        {
            ShowHeader("LISTA DE PAÍSES");

            try
            {
                _service.MostrarTodos();
                ShowInfoMessage("Listado de países completado.");
                Console.WriteLine();
                Console.Write("Presione cualquier tecla para continuar...");
                Console.ReadKey(true);
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Error al listar los países: {ex.Message}");
            }
        }

        private void ActualizarPais()
        {
            ShowHeader("ACTUALIZAR PAÍS");
            int paisId = GetValidatedIntInput("ID del país a actualizar: ");
            string nuevoNombre = GetValidatedInput("Nuevo nombre: ");

            var pais = new Country { Id = paisId, Nombre = nuevoNombre };

            try
            {
                _service.ActualizarCountry(pais);
                ShowSuccessMessage("País actualizado exitosamente.");
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Error al actualizar el país: {ex.Message}");
            }
        }

        private void EliminarPais()
        {
            ShowHeader("ELIMINAR PAÍS");
            int paisId = GetValidatedIntInput("ID del país a eliminar: ");

            try
            {
                _service.EliminarCountry(paisId);
                ShowSuccessMessage("País eliminado exitosamente.");
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Error al eliminar el país: {ex.Message}");
            }
        }
    }
}
