using System;
using SGCI_app.application.services;
using SGCI_app.application.Services;
using SGCI_app.domain.Entities;
using SGCI_app.domain.Factory;
using SGCI_app.infrastructure.postgres;

namespace SGCI_app.application.UI
{
    public class CityMenu : BaseMenu
    {
        private readonly CityService _service;

        public CityMenu() : base(showIntro: false)
        {
            string connStr = "Host=localhost;database=sgci;Port=5432;Username=postgres;Password=1219;Pooling=true";
            var factory = new ConexDBFactory(connStr);
            _service = new CityService(factory.CrearCityRepository());
        }

        public override void ShowMenu()
        {
            while (true)
            {
                ShowHeader("GESTIÓN DE CIUDADES");
                Console.WriteLine("1. Crear Ciudad");
                Console.WriteLine("2. Listar Ciudades");
                Console.WriteLine("3. Actualizar Ciudad");
                Console.WriteLine("4. Eliminar Ciudad");
                Console.WriteLine("0. Volver al menú principal");
                DrawSeparator();

                int option = GetValidatedIntInput("Seleccione una opción: ", 0, 4);

                switch (option)
                {
                    case 1:
                        CrearCiudad();
                        break;
                    case 2:
                        ListarCiudades();
                        break;
                    case 3:
                        ActualizarCiudad();
                        break;
                    case 4:
                        EliminarCiudad();
                        break;
                    case 0:
                        return;
                }
            }
        }

        private void CrearCiudad()
        {
            ShowHeader("CREAR NUEVA CIUDAD");
            string nombre = GetValidatedInput("Nombre de la ciudad: ");
            int regionId = GetValidatedIntInput("ID de la región: ");

            var ciudad = new City { Nombre = nombre, Region_Id = regionId };
            try
            {
                _service.CrearCity(ciudad);
                ShowSuccessMessage("Ciudad creada exitosamente.");
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Error al crear la ciudad: {ex.Message}");
            }
        }

        private void ListarCiudades()
        {
            ShowHeader("LISTA DE CIUDADES");
            try
            {
                _service.MostrarTodos();
                ShowInfoMessage("Listado de ciudades completado.");
                Console.WriteLine();
                Console.Write("Presione cualquier tecla para continuar...");
                Console.ReadKey(true);
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Error al listar las ciudades: {ex.Message}");
            }
        }

        private void ActualizarCiudad()
        {
            ShowHeader("ACTUALIZAR CIUDAD");
            int ciudadId = GetValidatedIntInput("ID de la ciudad a actualizar: ");
            string nuevoNombre = GetValidatedInput("Nuevo nombre: ");
            int nuevaRegionId = GetValidatedIntInput("Nuevo ID de región: ");

            var ciudad = new City { Id = ciudadId, Nombre = nuevoNombre, Region_Id = nuevaRegionId };
            try
            {
                _service.ActualizarCity(ciudad);
                ShowSuccessMessage("Ciudad actualizada exitosamente.");
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Error al actualizar la ciudad: {ex.Message}");
            }
        }

        private void EliminarCiudad()
        {
            ShowHeader("ELIMINAR CIUDAD");
            int ciudadId = GetValidatedIntInput("ID de la ciudad a eliminar: ");

            try
            {
                _service.EliminarCity(ciudadId);
                ShowSuccessMessage("Ciudad eliminada exitosamente.");
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Error al eliminar la ciudad: {ex.Message}");
            }
        }
    }
}