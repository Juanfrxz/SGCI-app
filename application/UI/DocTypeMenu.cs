using System;
using SGCI_app.application.services;
using SGCI_app.domain.Entities;
using SGCI_app.domain.Factory;
using SGCI_app.infrastructure.postgres;

namespace SGCI_app.application.UI
{
    public class DocTypeMenu
    {
        private readonly DocTypeService _service;

        public DocTypeMenu()
        {
            string connStr = "Host=localhost;database=sgci;Port=5432;Username=postgres;Password=1219;Pooling=true";
            var factory = new ConexDBFactory(connStr);
            _service = new DocTypeService(factory.CrearDocTypeRepository());
        }

        public void ShowMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== GESTIÓN DE TIPOS DE DOCUMENTO ===");
                Console.WriteLine("1. Crear Tipo Documento");
                Console.WriteLine("2. Listar Tipos Documento");
                Console.WriteLine("3. Actualizar Tipo Documento");
                Console.WriteLine("4. Eliminar Tipo Documento");
                Console.WriteLine("0. Volver al menú principal");
                Console.Write("\nSeleccione una opción: ");

                string? input = Console.ReadLine();
                if (string.IsNullOrEmpty(input))
                {
                    Console.WriteLine("Por favor, ingrese una opción válida.");
                    Console.ReadKey();
                    continue;
                }

                switch (input)
                {
                    case "1":
                        CrearTipoDocumento();
                        break;
                    case "2":
                        ListarTiposDocumento();
                        break;
                    case "3":
                        ActualizarTipoDocumento();
                        break;
                    case "4":
                        EliminarTipoDocumento();
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Opción no válida. Presione cualquier tecla para continuar...");
                        Console.ReadKey();
                        break;
                }
            }
        }

        private void CrearTipoDocumento()
        {
            Console.Clear();
            Console.WriteLine("=== CREAR TIPO DE DOCUMENTO ===");

            Console.Write("Descripción: ");
            string? descripcion = Console.ReadLine();
            var entity = new DocType { Descripcion = descripcion };

            try
            {
                _service.CrearDocType(entity);
                Console.WriteLine("\nTipo de documento creado exitosamente.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nError al crear tipo de documento: {ex.Message}");
            }

            Console.WriteLine("\nPresione cualquier tecla para continuar...");
            Console.ReadKey();
        }

        private void ListarTiposDocumento()
        {
            Console.Clear();
            Console.WriteLine("=== LISTA DE TIPOS DE DOCUMENTO ===\n");

            try
            {
                _service.MostrarTodos();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nError al listar tipos de documento: {ex.Message}");
            }

            Console.WriteLine("\nPresione cualquier tecla para continuar...");
            Console.ReadKey();
        }

        private void ActualizarTipoDocumento()
        {
            Console.Clear();
            Console.WriteLine("=== ACTUALIZAR TIPO DE DOCUMENTO ===");

            Console.Write("ID del tipo de documento a actualizar: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("ID inválido.");
                Console.ReadKey();
                return;
            }

            Console.Write("Nueva descripción (dejar en blanco para mantener la actual): ");
            string? descripcion = Console.ReadLine();
            var entity = new DocType { Id = id, Descripcion = string.IsNullOrWhiteSpace(descripcion) ? null : descripcion };

            try
            {
                _service.ActualizarDocType(id, entity);
                Console.WriteLine("\nTipo de documento actualizado exitosamente.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nError al actualizar tipo de documento: {ex.Message}");
            }

            Console.WriteLine("\nPresione cualquier tecla para continuar...");
            Console.ReadKey();
        }

        private void EliminarTipoDocumento()
        {
            Console.Clear();
            Console.WriteLine("=== ELIMINAR TIPO DE DOCUMENTO ===");

            Console.Write("ID del tipo de documento a eliminar: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("ID inválido.");
                Console.ReadKey();
                return;
            }

            try
            {
                _service.EliminarDocType(id);
                Console.WriteLine("\nTipo de documento eliminado exitosamente.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nError al eliminar tipo de documento: {ex.Message}");
            }

            Console.WriteLine("\nPresione cualquier tecla para continuar...");
            Console.ReadKey();
        }
    }
}
