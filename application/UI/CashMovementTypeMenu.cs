using System;
using SGCI_app.application.Services;
using SGCI_app.domain.Entities;
using SGCI_app.domain.Factory;
using SGCI_app.infrastructure.postgres;

namespace SGCI_app.application.UI
{
    public class CashMovementTypeMenu
    {
        private readonly CashMovementTypeService _service;

        public CashMovementTypeMenu()
        {
            string connStr = "Host=localhost;database=sgci;Port=5432;Username=postgres;Password=1219;Pooling=true";
            var factory = new ConexDBFactory(connStr);
            _service = new CashMovementTypeService(factory.CrearCashMovementTypeRepository());
        }

        public void ShowMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== GESTIÓN DE TIPOS DE MOVIMIENTO DE CAJA ===");
                Console.WriteLine("1. Crear Tipo de Movimiento");
                Console.WriteLine("2. Listar Tipos de Movimiento");
                Console.WriteLine("3. Actualizar Tipo de Movimiento");
                Console.WriteLine("4. Eliminar Tipo de Movimiento");
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
                    case "1": CrearTipo(); break;
                    case "2": ListarTipos(); break;
                    case "3": ActualizarTipo(); break;
                    case "4": EliminarTipo(); break;
                    case "0": return;
                    default:
                        Console.WriteLine("Opción no válida. Presione cualquier tecla para continuar...");
                        Console.ReadKey();
                        break;
                }
            }
        }

        private void CrearTipo()
        {
            Console.Clear();
            Console.WriteLine("=== CREAR TIPO DE MOVIMIENTO DE CAJA ===");

            Console.Write("Nombre: ");
            var nombre = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(nombre))
            {
                Console.WriteLine("Nombre inválido."); Console.ReadKey(); return;
            }
            Console.Write("Tipo (p.ej. compra/venta): ");
            var tipo = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(tipo))
            {
                Console.WriteLine("Tipo inválido."); Console.ReadKey(); return;
            }

            var entity = new CashMovementType { Nombre = nombre, Tipo = tipo };
            try
            {
                _service.CrearCashMovementType(entity);
                Console.WriteLine("\nTipo de movimiento creado exitosamente.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nError al crear tipo de movimiento: {ex.Message}");
            }
            Console.WriteLine("\nPresione cualquier tecla para continuar..."); Console.ReadKey();
        }

        private void ListarTipos()
        {
            Console.Clear();
            Console.WriteLine("=== LISTA DE TIPOS DE MOVIMIENTO DE CAJA ===\n");
            try
            {
                _service.MostrarTodos();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nError al listar tipos: {ex.Message}");
            }
            Console.WriteLine("\nPresione cualquier tecla para continuar..."); Console.ReadKey();
        }

        private void ActualizarTipo()
        {
            Console.Clear();
            Console.WriteLine("=== ACTUALIZAR TIPO DE MOVIMIENTO DE CAJA ===");
            Console.Write("ID a actualizar: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("ID inválido."); Console.ReadKey(); return;
            }
            Console.Write("Nuevo nombre (dejar en blanco para mantener): ");
            var nombre = Console.ReadLine();
            Console.Write("Nuevo tipo (dejar en blanco para mantener): ");
            var tipo = Console.ReadLine();

            var entity = new CashMovementType
            {
                Nombre = string.IsNullOrWhiteSpace(nombre) ? null : nombre,
                Tipo = string.IsNullOrWhiteSpace(tipo) ? null : tipo
            };
            try
            {
                _service.ActualizarCashMovementType(id, entity);
                Console.WriteLine("\nTipo actualizado exitosamente.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nError al actualizar tipo: {ex.Message}");
            }
            Console.WriteLine("\nPresione cualquier tecla para continuar..."); Console.ReadKey();
        }

        private void EliminarTipo()
        {
            Console.Clear();
            Console.WriteLine("=== ELIMINAR TIPO DE MOVIMIENTO DE CAJA ===");
            Console.Write("ID a eliminar: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("ID inválido."); Console.ReadKey(); return;
            }
            try
            {
                _service.EliminarCashMovementType(id);
                Console.WriteLine("\nTipo eliminado exitosamente.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nError al eliminar tipo: {ex.Message}");
            }
            Console.WriteLine("\nPresione cualquier tecla para continuar..."); Console.ReadKey();
        }
    }
}
