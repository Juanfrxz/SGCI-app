using System;
using SGCI_app.application.Services;
using SGCI_app.domain.Entities;
using SGCI_app.domain.Factory;
using SGCI_app.infrastructure.postgres;

namespace SGCI_app.application.UI
{
    public class PhoneTypeMenu : BaseMenu
    {
        private readonly PhoneTypeService _service;

        public PhoneTypeMenu() : base(showIntro: false)
        {
            string connStr = "Host=localhost;database=sgci;Port=5432;Username=postgres;Password=1219;Pooling=true";
            var factory = new ConexDBFactory(connStr);
            _service = new PhoneTypeService(factory.CrearPhoneTypeRepository());
        }

        public override void ShowMenu()
        {
            while (true)
            {
                ShowHeader("GESTIÓN DE TIPOS DE TELÉFONO");
                Console.WriteLine("1. Crear Tipo de Teléfono");
                Console.WriteLine("2. Listar Tipos de Teléfono");
                Console.WriteLine("3. Actualizar Tipo de Teléfono");
                Console.WriteLine("4. Eliminar Tipo de Teléfono");
                Console.WriteLine("0. Volver al menú principal");
                DrawSeparator();

                int option = GetValidatedIntInput("Seleccione una opción: ", 0, 4);
                switch (option)
                {
                    case 1:
                        CrearPhoneType();
                        break;
                    case 2:
                        ListarPhoneTypes();
                        break;
                    case 3:
                        ActualizarPhoneType();
                        break;
                    case 4:
                        EliminarPhoneType();
                        break;
                    case 0:
                        return;
                }
            }
        }

        private void CrearPhoneType()
        {
            ShowHeader("CREAR TIPO DE TELÉFONO");
            string descripcion = GetValidatedInput("Descripción: ");

            var entity = new PhoneType { Descripcion = descripcion };
            try
            {
                _service.CrearPhoneType(entity);
                ShowSuccessMessage("Tipo de teléfono creado exitosamente.");
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Error al crear tipo de teléfono: {ex.Message}");
            }
        }

        private void ListarPhoneTypes()
        {
            ShowHeader("LISTA DE TIPOS DE TELÉFONO");
            try
            {
                _service.MostrarTodos();
                ShowInfoMessage("Listado de tipos de teléfono completado.");
                Console.WriteLine();
                Console.Write("Presione cualquier tecla para continuar...");
                Console.ReadKey(true);
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Error al listar tipos de teléfono: {ex.Message}");
            }
        }

        private void ActualizarPhoneType()
        {
            ShowHeader("ACTUALIZAR TIPO DE TELÉFONO");
            int id = GetValidatedIntInput("ID del tipo de teléfono a actualizar: ");
            string descripcion = GetValidatedInput("Nueva descripción (dejar en blanco para mantener): ", allowEmpty: true);

            var entity = new PhoneType { Id = id, Descripcion = string.IsNullOrWhiteSpace(descripcion) ? null : descripcion };
            try
            {
                _service.ActualizarPhoneType(id, entity);
                ShowSuccessMessage("Tipo de teléfono actualizado exitosamente.");
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Error al actualizar tipo de teléfono: {ex.Message}");
            }
        }

        private void EliminarPhoneType()
        {
            ShowHeader("ELIMINAR TIPO DE TELÉFONO");
            int id = GetValidatedIntInput("ID del tipo de teléfono a eliminar: ");

            try
            {
                _service.EliminarPhoneType(id);
                ShowSuccessMessage("Tipo de teléfono eliminado exitosamente.");
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Error al eliminar tipo de teléfono: {ex.Message}");
            }
        }
    }
}
