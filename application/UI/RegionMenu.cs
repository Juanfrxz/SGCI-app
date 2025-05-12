using System;
using SGCI_app.domain.Entities;
using SGCI_app.domain.Factory;
using SGCI_app.infrastructure.postgres;
using SGCI_app.application.services;

namespace SGCI_app.application.UI
{
    public class RegionMenu : BaseMenu
    {
        private readonly RegionService _service;

        public RegionMenu() : base(showIntro: false)
        {
            string connStr = "Host=localhost;database=sgci;Port=5432;Username=postgres;Password=1219;Pooling=true";
            var factory = new ConexDBFactory(connStr);
            _service = new RegionService(factory.CrearRegionRepository());
        }

        public override void ShowMenu()
        {
            while (true)
            {
                ShowHeader("GESTIÓN DE REGIONES");
                Console.WriteLine("1. Crear Región");
                Console.WriteLine("2. Listar Regiones");
                Console.WriteLine("3. Actualizar Región");
                Console.WriteLine("4. Eliminar Región");
                Console.WriteLine("0. Volver al menú principal");
                DrawSeparator();

                int option = GetValidatedIntInput("Seleccione una opción: ", 0, 4);
                switch (option)
                {
                    case 1:
                        CrearRegion();
                        break;
                    case 2:
                        ListarRegiones();
                        break;
                    case 3:
                        ActualizarRegion();
                        break;
                    case 4:
                        EliminarRegion();
                        break;
                    case 0:
                        return;
                }
            }
        }

        private void CrearRegion()
        {
            ShowHeader("CREAR NUEVA REGIÓN");
            
            try
            {
                var region = new Region();
                region.Nombre = GetValidatedInput("Nombre de la región: ");
                region.Pais_Id = GetValidatedIntInput("ID del país: ");
                
                _service.CrearRegion(region);
                ShowSuccessMessage("Región creada exitosamente.");
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Error al crear la región: {ex.Message}");
            }
        }

        private void ListarRegiones()
        {
            ShowHeader("LISTA DE REGIONES");
            
            try
            {
                _service.MostrarTodos();
                ShowInfoMessage("Listado de regiones completado.");
                Console.WriteLine();
                Console.Write("Presione cualquier tecla para continuar...");
                Console.ReadKey(true);
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Error al listar las regiones: {ex.Message}");
            }
        }

        private void ActualizarRegion()
        {
            ShowHeader("ACTUALIZAR REGIÓN");
            
            try
            {
                int regionId = GetValidatedIntInput("ID de la región a actualizar: ");
                var region = new Region { Id = regionId };

                string nombreInput = GetValidatedInput("Nuevo nombre (dejar en blanco para mantener el actual): ", allowEmpty: true);
                if (!string.IsNullOrEmpty(nombreInput)) region.Nombre = nombreInput;

                string paisInput = GetValidatedInput("Nuevo ID de país (dejar en blanco para mantener el actual): ", allowEmpty: true);
                if (!string.IsNullOrEmpty(paisInput) && int.TryParse(paisInput, out int paisId))
                    region.Pais_Id = paisId;
                
                _service.ActualuizarRegion(region);
                ShowSuccessMessage("Región actualizada exitosamente.");
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Error al actualizar la región: {ex.Message}");
            }
        }

        private void EliminarRegion()
        {
            ShowHeader("ELIMINAR REGIÓN");
            
            try
            {
                int regionId = GetValidatedIntInput("ID de la región a eliminar: ");
                _service.EliminarRegion(regionId);
                ShowSuccessMessage("Región eliminada exitosamente.");
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Error al eliminar la región: {ex.Message}");
            }
        }
    }
} 