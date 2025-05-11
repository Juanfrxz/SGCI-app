using System;
using SGCI_app.domain.Entities;
using SGCI_app.domain.Factory;
using SGCI_app.infrastructure.postgres;
using SGCI_app.application.services;

namespace SGCI_app.application.UI
{
    public class ArlMenu : BaseMenu
    {
        private readonly ArlService _service;

        // Ahora invocamos explícitamente base(false) para no mostrar la intro
        public ArlMenu() : base(showIntro: false)
        {
            string connStr = "Host=localhost;database=sgci;Port=5432;Username=postgres;Password=1219;Pooling=true";
            var factory = new ConexDBFactory(connStr);
            _service = new ArlService(factory.CrearArlRepository());
        }

        public override void ShowMenu()
        {
            while (true)
            {
                ShowHeader("GESTIÓN DE ARL");
                Console.WriteLine("1. Crear ARL");
                Console.WriteLine("2. Listar ARL");
                Console.WriteLine("3. Actualizar ARL");
                Console.WriteLine("4. Eliminar ARL");
                Console.WriteLine("0. Volver al menú principal");
                DrawSeparator();

                int option = GetValidatedIntInput("Seleccione una opción: ", 0, 4);

                switch (option)
                {
                    case 1:
                        CrearArl();
                        break;
                    case 2:
                        ListarArl();
                        break;
                    case 3:
                        ActualizarArl();
                        break;
                    case 4:
                        EliminarArl();
                        break;
                    case 0:
                        return;
                }
            }
        }

        private void CrearArl()
        {
            ShowHeader("CREAR NUEVA ARL");
            string nombre = GetValidatedInput("Nombre de la ARL: ");

            var arl = new ARL { Nombre = nombre };

            try
            {
                _service.CrearArl(arl);
                ShowSuccessMessage("ARL creada exitosamente.");
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Error al crear la ARL: {ex.Message}");
            }
        }

        private void ListarArl()
        {
            ShowHeader("LISTA DE ARL");
            try
            {
                _service.MostrarTodos();
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Error al listar las ARL: {ex.Message}");
                return;
            }

            Console.WriteLine();
            Console.Write("Presione cualquier tecla para continuar...");
            Console.ReadKey(true);
        }

        private void ActualizarArl()
        {
            ShowHeader("ACTUALIZAR ARL");
            int arlId = GetValidatedIntInput("ID de la ARL a actualizar: ");
            string nuevoNombre = GetValidatedInput("Nuevo nombre: ");

            var arl = new ARL { Id = arlId, Nombre = nuevoNombre };
            try
            {
                _service.ActualizarArl(arlId, arl);
                ShowSuccessMessage("ARL actualizada exitosamente.");
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Error al actualizar la ARL: {ex.Message}");
            }
        }

        private void EliminarArl()
        {
            ShowHeader("ELIMINAR ARL");
            int arlId = GetValidatedIntInput("ID de la ARL a eliminar: ");

            try
            {
                _service.EliminarArl(arlId);
                ShowSuccessMessage("ARL eliminada exitosamente.");
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Error al eliminar la ARL: {ex.Message}");
            }
        }
    }
}