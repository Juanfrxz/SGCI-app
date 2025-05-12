using System;
using SGCI_app.application.services;
using SGCI_app.domain.Entities;
using SGCI_app.domain.Factory;
using SGCI_app.infrastructure.postgres;

namespace SGCI_app.application.UI
{
    public class EpsMenu : BaseMenu
    {
        private readonly EpsService _service;

        public EpsMenu() : base(showIntro: false)
        {
            string connStr = "Host=localhost;database=sgci;Port=5432;Username=postgres;Password=1219;Pooling=true";
            var factory = new ConexDBFactory(connStr);
            _service = new EpsService(factory.CrearEpsRepository());
        }

        public override void ShowMenu()
        {
            while (true)
            {
                ShowHeader("GESTIÓN DE EPS");
                Console.WriteLine("1. Crear EPS");
                Console.WriteLine("2. Listar EPS");
                Console.WriteLine("3. Actualizar EPS");
                Console.WriteLine("4. Eliminar EPS");
                Console.WriteLine("0. Volver al menú principal");
                DrawSeparator();

                int option = GetValidatedIntInput("Seleccione una opción: ", 0, 4);
                switch (option)
                {
                    case 1:
                        CrearEps();
                        break;
                    case 2:
                        ListarEps();
                        break;
                    case 3:
                        ActualizarEps();
                        break;
                    case 4:
                        EliminarEps();
                        break;
                    case 0:
                        return;
                }
            }
        }

        private void CrearEps()
        {
            ShowHeader("CREAR EPS");
            var eps = new EPS();
            string nombre = GetValidatedInput("Nombre de la EPS: ");
            eps.Nombre = nombre;

            try
            {
                _service.CrearEps(eps);
                ShowSuccessMessage("EPS creada exitosamente.");
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Error al crear la EPS: {ex.Message}");
            }
        }

        private void ListarEps()
        {
            ShowHeader("LISTA DE EPS");
            try
            {
                _service.ObtenerTodasEps();
                ShowInfoMessage("Listado de EPS completado.");
                Console.WriteLine();
                Console.Write("Presione cualquier tecla para continuar...");
                Console.ReadKey(true);
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Error al obtener las EPS: {ex.Message}");
            }
        }

        private void ActualizarEps()
        {
            ShowHeader("ACTUALIZAR EPS");
            int id = GetValidatedIntInput("ID de la EPS a actualizar: ");
            string nuevoNombre = GetValidatedInput("Nuevo nombre (dejar en blanco para mantener el actual): ", allowEmpty: true);

            var eps = new EPS { Id = id, Nombre = string.IsNullOrWhiteSpace(nuevoNombre) ? null : nuevoNombre };

            try
            {
                _service.ActualizarEps(id, eps);
                ShowSuccessMessage("EPS actualizada exitosamente.");
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Error al actualizar la EPS: {ex.Message}");
            }
        }

        private void EliminarEps()
        {
            ShowHeader("ELIMINAR EPS");
            int id = GetValidatedIntInput("ID de la EPS a eliminar: ");

            try
            {
                _service.EliminarEps(id);
                ShowSuccessMessage("EPS eliminada exitosamente.");
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Error al eliminar la EPS: {ex.Message}");
            }
        }
    }
} 