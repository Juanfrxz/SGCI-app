using System;
using SGCI_app.application.Services;
using SGCI_app.domain.Entities;
using SGCI_app.domain.Factory;
using SGCI_app.infrastructure.postgres;

namespace SGCI_app.application.UI
{
    public class ThirdPartyTypeMenu : BaseMenu
    {
        private readonly ThirdPartyTypeService _service;

        public ThirdPartyTypeMenu() : base(showIntro: false)
        {
            string connStr = "Host=localhost;database=sgci;Port=5432;Username=postgres;Password=1219;Pooling=true";
            var factory = new ConexDBFactory(connStr);
            _service = new ThirdPartyTypeService(factory.CrearThirdPartyTypeRepository());
        }

        public override void ShowMenu()
        {
            while (true)
            {
                ShowHeader("GESTIÓN DE TIPOS DE TERCEROS");
                Console.WriteLine("1. Crear Tipo de Tercero");
                Console.WriteLine("2. Listar Tipos de Tercero");
                Console.WriteLine("3. Actualizar Tipo de Tercero");
                Console.WriteLine("4. Eliminar Tipo de Tercero");
                Console.WriteLine("0. Volver al menú principal");
                DrawSeparator();

                int option = GetValidatedIntInput("Seleccione una opción: ", 0, 4);
                switch (option)
                {
                    case 1:
                        CrearTipoTercero();
                        break;
                    case 2:
                        ListarTiposTercero();
                        break;
                    case 3:
                        ActualizarTipoTercero();
                        break;
                    case 4:
                        EliminarTipoTercero();
                        break;
                    case 0:
                        return;
                }
            }
        }

        private void CrearTipoTercero()
        {
            ShowHeader("CREAR TIPO DE TERCERO");

            try
            {
                string descripcion = GetValidatedInput("Descripción: ");
                var entity = new ThirdPartyType { Descripcion = descripcion };
                
                _service.CrearThirdPartyType(entity);
                ShowSuccessMessage("Tipo de tercero creado exitosamente.");
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Error al crear tipo de tercero: {ex.Message}");
            }
        }

        private void ListarTiposTercero()
        {
            ShowHeader("LISTA DE TIPOS DE TERCERO");

            try
            {
                _service.MostrarTodos();
                ShowInfoMessage("Listado de tipos de tercero completado.");
                Console.WriteLine();
                Console.Write("Presione cualquier tecla para continuar...");
                Console.ReadKey(true);
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Error al listar tipos de terceros: {ex.Message}");
            }
        }

        private void ActualizarTipoTercero()
        {
            ShowHeader("ACTUALIZAR TIPO DE TERCERO");

            try
            {
                int id = GetValidatedIntInput("ID del tipo de tercero a actualizar: ");
                string descripcion = GetValidatedInput("Nueva descripción (dejar en blanco para mantener): ", allowEmpty: true);
                
                var entity = new ThirdPartyType { Descripcion = descripcion };
                _service.ActualizarThirdPartyType(id, entity);
                ShowSuccessMessage("Tipo de tercero actualizado exitosamente.");
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Error al actualizar tipo de tercero: {ex.Message}");
            }
        }

        private void EliminarTipoTercero()
        {
            ShowHeader("ELIMINAR TIPO DE TERCERO");

            try
            {
                int id = GetValidatedIntInput("ID del tipo de tercero a eliminar: ");
                _service.EliminarThirdPartyType(id);
                ShowSuccessMessage("Tipo de tercero eliminado exitosamente.");
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Error al eliminar tipo de tercero: {ex.Message}");
            }
        }
    }
}