using System;
using SGCI_app.application.Services;
using SGCI_app.domain.Entities;
using SGCI_app.domain.Factory;
using SGCI_app.infrastructure.postgres;

namespace SGCI_app.application.UI
{
    public class ThirdPartyTypeMenu
    {
        private readonly ThirdPartyTypeService _service;

        public ThirdPartyTypeMenu()
        {
            string connStr = "Host=localhost;database=sgci;Port=5432;Username=postgres;Password=1219;Pooling=true";
            var factory = new ConexDBFactory(connStr);
            _service = new ThirdPartyTypeService(factory.CrearThirdPartyTypeRepository());
        }

        public void ShowMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== GESTIÓN DE TIPOS DE TERCEROS ===");
                Console.WriteLine("1. Crear Tipo de Tercero");
                Console.WriteLine("2. Listar Tipos de Tercero");
                Console.WriteLine("3. Actualizar Tipo de Tercero");
                Console.WriteLine("4. Eliminar Tipo de Tercero");
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
                    case "1": CrearTipoTercero(); break;
                    case "2": ListarTiposTercero(); break;
                    case "3": ActualizarTipoTercero(); break;
                    case "4": EliminarTipoTercero(); break;
                    case "0": return;
                    default:
                        Console.WriteLine("Opción no válida. Presione cualquier tecla para continuar...");
                        Console.ReadKey();
                        break;
                }
            }
        }

        private void CrearTipoTercero()
        {
            Console.Clear();
            Console.WriteLine("=== CREAR TIPO DE TERCERO ===");

            Console.Write("Descripción: ");
            string? descripcion = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(descripcion))
            {
                Console.WriteLine("Descripción inválida.");
                Console.ReadKey();
                return;
            }

            var entity = new ThirdPartyType { Descripcion = descripcion };
            try
            {
                _service.CrearThirdPartyType(entity);
                Console.WriteLine("\nTipo de tercero creado exitosamente.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nError al crear tipo de tercero: {ex.Message}");
            }

            Console.WriteLine("\nPresione cualquier tecla para continuar...");
            Console.ReadKey();
        }

        private void ListarTiposTercero()
        {
            Console.Clear();
            Console.WriteLine("=== LISTA DE TIPOS DE TERCERO ===\n");

            try
            {
                _service.MostrarTodos();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nError al listar tipos de terceros: {ex.Message}");
            }

            Console.WriteLine("\nPresione cualquier tecla para continuar...");
            Console.ReadKey();
        }

        private void ActualizarTipoTercero()
        {
            Console.Clear();
            Console.WriteLine("=== ACTUALIZAR TIPO DE TERCERO ===");

            Console.Write("ID del tipo de tercero a actualizar: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("ID inválido.");
                Console.ReadKey();
                return;
            }

            Console.Write("Nueva descripción (dejar en blanco para mantener): ");
            string? descripcion = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(descripcion)) descripcion = null;

            var entity = new ThirdPartyType { Descripcion = descripcion! };
            try
            {
                _service.ActualizarThirdPartyType(id, entity);
                Console.WriteLine("\nTipo de tercero actualizado exitosamente.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nError al actualizar tipo de tercero: {ex.Message}");
            }

            Console.WriteLine("\nPresione cualquier tecla para continuar...");
            Console.ReadKey();
        }

        private void EliminarTipoTercero()
        {
            Console.Clear();
            Console.WriteLine("=== ELIMINAR TIPO DE TERCERO ===");

            Console.Write("ID del tipo de tercero a eliminar: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("ID inválido.");
                Console.ReadKey();
                return;
            }

            try
            {
                _service.EliminarThirdPartyType(id);
                Console.WriteLine("\nTipo de tercero eliminado exitosamente.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nError al eliminar tipo de tercero: {ex.Message}");
            }

            Console.WriteLine("\nPresione cualquier tecla para continuar...");
            Console.ReadKey();
        }
    }
}