using System;
using SGCI_app.application.services;
using SGCI_app.domain.Entities;
using SGCI_app.domain.Factory;
using SGCI_app.infrastructure.postgres;

namespace SGCI_app.application.UI
{
    public class DocTypeMenu : BaseMenu
    {
        private readonly DocTypeService _service;

        public DocTypeMenu() : base(showIntro: false)
        {
            string connStr = "Host=localhost;database=sgci;Port=5432;Username=postgres;Password=1219;Pooling=true";
            var factory = new ConexDBFactory(connStr);
            _service = new DocTypeService(factory.CrearDocTypeRepository());
        }

        public override void ShowMenu()
        {
            while (true)
            {
                ShowHeader("GESTIÓN DE TIPOS DE DOCUMENTO");
                Console.WriteLine("1. Crear Tipo Documento");
                Console.WriteLine("2. Listar Tipos Documento");
                Console.WriteLine("3. Actualizar Tipo Documento");
                Console.WriteLine("4. Eliminar Tipo Documento");
                Console.WriteLine("0. Volver al menú principal");
                DrawSeparator();

                int option = GetValidatedIntInput("Seleccione una opción: ", 0, 4);
                switch (option)
                {
                    case 1:
                        CrearTipoDocumento();
                        break;
                    case 2:
                        ListarTiposDocumento();
                        break;
                    case 3:
                        ActualizarTipoDocumento();
                        break;
                    case 4:
                        EliminarTipoDocumento();
                        break;
                    case 0:
                        return;
                }
            }
        }

        private void CrearTipoDocumento()
        {
            ShowHeader("CREAR TIPO DE DOCUMENTO");
            string descripcion = GetValidatedInput("Descripción: ");

            var entity = new DocType { Descripcion = descripcion };

            try
            {
                _service.CrearDocType(entity);
                ShowSuccessMessage("Tipo de documento creado exitosamente.");
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Error al crear tipo de documento: {ex.Message}");
            }
        }

        private void ListarTiposDocumento()
        {
            ShowHeader("LISTA DE TIPOS DE DOCUMENTO");

            try
            {
                _service.MostrarTodos();
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Error al listar tipos de documento: {ex.Message}");
            }

            Console.WriteLine();
            Console.Write("Presione cualquier tecla para continuar...");
            Console.ReadKey(true);
        }

        private void ActualizarTipoDocumento()
        {
            ShowHeader("ACTUALIZAR TIPO DE DOCUMENTO");
            int id = GetValidatedIntInput("ID del tipo de documento a actualizar: ");
            string descripcion = GetValidatedInput("Nueva descripción (dejar en blanco para mantener la actual): ", allowEmpty: true);

            var entity = new DocType { Id = id, Descripcion = string.IsNullOrWhiteSpace(descripcion) ? null : descripcion };

            try
            {
                _service.ActualizarDocType(id, entity);
                ShowSuccessMessage("Tipo de documento actualizado exitosamente.");
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Error al actualizar tipo de documento: {ex.Message}");
            }
        }

        private void EliminarTipoDocumento()
        {
            ShowHeader("ELIMINAR TIPO DE DOCUMENTO");
            int id = GetValidatedIntInput("ID del tipo de documento a eliminar: ");

            try
            {
                _service.EliminarDocType(id);
                ShowSuccessMessage("Tipo de documento eliminado exitosamente.");
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Error al eliminar tipo de documento: {ex.Message}");
            }
        }
    }
}