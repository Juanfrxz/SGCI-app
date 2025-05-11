using System;
using SGCI_app.application.Services;
using SGCI_app.domain.Entities;
using SGCI_app.domain.Factory;
using SGCI_app.infrastructure.postgres;

namespace SGCI_app.application.UI
{
    public class PhoneTypeMenu
    {
        private readonly PhoneTypeService _service;

        public PhoneTypeMenu()
        {
            string connStr = "Host=localhost;database=sgci;Port=5432;Username=postgres;Password=1219;Pooling=true";
            var factory = new ConexDBFactory(connStr);
            _service = new PhoneTypeService(factory.CrearPhoneTypeRepository());
        }

        public void ShowMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== GESTIÓN DE TIPOS DE TELÉFONO ===");
                Console.WriteLine("1. Crear Tipo de Teléfono");
                Console.WriteLine("2. Listar Tipos de Teléfono");
                Console.WriteLine("3. Actualizar Tipo de Teléfono");
                Console.WriteLine("4. Eliminar Tipo de Teléfono");
                Console.WriteLine("0. Volver al menú principal");
                Console.Write("\nSeleccione una opción: ");

                string? input = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(input))
                {
                    Console.WriteLine("Por favor, ingrese una opción válida.");
                    Console.ReadKey();
                    continue;
                }

                switch (input)
                {
                    case "1": CrearPhoneType(); break;
                    case "2": ListarPhoneTypes(); break;
                    case "3": ActualizarPhoneType(); break;
                    case "4": EliminarPhoneType(); break;
                    case "0": return;
                    default:
                        Console.WriteLine("Opción no válida. Presione cualquier tecla para continuar...");
                        Console.ReadKey();
                        break;
                }
            }
        }

        private void CrearPhoneType()
        {
            Console.Clear();
            Console.WriteLine("=== CREAR TIPO DE TELÉFONO ===");

            Console.Write("Descripción: ");
            string? descripcion = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(descripcion))
            {
                Console.WriteLine("Descripción inválida.");
                Console.ReadKey();
                return;
            }

            var entity = new PhoneType { Descripcion = descripcion };
            try
            {
                _service.CrearPhoneType(entity);
                Console.WriteLine("\nTipo de teléfono creado exitosamente.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nError al crear tipo de teléfono: {ex.Message}");
            }

            Console.WriteLine("\nPresione cualquier tecla para continuar...");
            Console.ReadKey();
        }

        private void ListarPhoneTypes()
        {
            Console.Clear();
            Console.WriteLine("=== LISTA DE TIPOS DE TELÉFONO ===\n");

            try
            {
                _service.MostrarTodos();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nError al listar tipos de teléfono: {ex.Message}");
            }

            Console.WriteLine("\nPresione cualquier tecla para continuar...");
            Console.ReadKey();
        }

        private void ActualizarPhoneType()
        {
            Console.Clear();
            Console.WriteLine("=== ACTUALIZAR TIPO DE TELÉFONO ===");

            Console.Write("ID del tipo de teléfono a actualizar: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("ID inválido.");
                Console.ReadKey();
                return;
            }

            Console.Write("Nueva descripción (dejar en blanco para mantener): ");
            string? descripcion = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(descripcion)) descripcion = null;

            var entity = new PhoneType { Descripcion = descripcion };
            try
            {
                _service.ActualizarPhoneType(id, entity);
                Console.WriteLine("\nTipo de teléfono actualizado exitosamente.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nError al actualizar tipo de teléfono: {ex.Message}");
            }

            Console.WriteLine("\nPresione cualquier tecla para continuar...");
            Console.ReadKey();
        }

        private void EliminarPhoneType()
        {
            Console.Clear();
            Console.WriteLine("=== ELIMINAR TIPO DE TELÉFONO ===");

            Console.Write("ID del tipo de teléfono a eliminar: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("ID inválido.");
                Console.ReadKey();
                return;
            }

            try
            {
                _service.EliminarPhoneType(id);
                Console.WriteLine("\nTipo de teléfono eliminado exitosamente.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nError al eliminar tipo de teléfono: {ex.Message}");
            }

            Console.WriteLine("\nPresione cualquier tecla para continuar...");
            Console.ReadKey();
        }
    }
}
