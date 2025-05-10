using System;
using SGCI_app.domain.Entities;
using SGCI_app.domain.Factory;
using SGCI_app.infrastructure.postgres;
using SGCI_app.application.services;

namespace SGCI_app.application.UI
{
    public class ArlMenu
    {
        private readonly ArlService _service;

        public ArlMenu()
        {
            string connStr = "Host=localhost;database=sgci;Port=5432;Username=postgres;Password=1219;Pooling=true";
            var factory = new ConexDBFactory(connStr);
            _service = new ArlService(factory.CrearArlRepository());
        }

        public void ShowMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== GESTIÓN DE ARL ===");
                Console.WriteLine("1. Crear ARL");
                Console.WriteLine("2. Listar ARL");
                Console.WriteLine("3. Actualizar ARL");
                Console.WriteLine("4. Eliminar ARL");
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
                        CrearArl();
                        break;
                    case "2":
                        ListarArl();
                        break;
                    case "3":
                        ActualizarArl();
                        break;
                    case "4":
                        EliminarArl();
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

        private void CrearArl()
        {
            Console.Clear();
            Console.WriteLine("=== CREAR NUEVA ARL ===");
            
            Console.Write("Nombre de la ARL: ");
            string? nombre = Console.ReadLine();

            var arl = new ARL 
            { 
                Nombre = nombre
            };
            
            try
            {
                _service.CrearArl(arl);
                Console.WriteLine("ARL creada exitosamente.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al crear la ARL: {ex.Message}");
            }
            
            Console.ReadKey();
        }

        private void ListarArl()
        {
            Console.Clear();
            Console.WriteLine("=== LISTA DE ARL ===");
            
            try
            {
                _service.MostrarTodos();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al listar las ARL: {ex.Message}");
            }
            
            Console.WriteLine("\nPresione cualquier tecla para continuar...");
            Console.ReadKey();
        }

        private void ActualizarArl()
        {
            Console.Clear();
            Console.WriteLine("=== ACTUALIZAR ARL ===");
            
            Console.Write("ID de la ARL a actualizar: ");
            if (!int.TryParse(Console.ReadLine(), out int arlId))
            {
                Console.WriteLine("ID de ARL inválido.");
                Console.ReadKey();
                return;
            }

            Console.Write("Nuevo nombre: ");
            string? nuevoNombre = Console.ReadLine();

            var arl = new ARL 
            { 
                Id = arlId,
                Nombre = nuevoNombre
            };
            
            try
            {
                _service.ActualizarArl(arlId, arl);
                Console.WriteLine("ARL actualizada exitosamente.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al actualizar la ARL: {ex.Message}");
            }
            
            Console.ReadKey();
        }

        private void EliminarArl()
        {
            Console.Clear();
            Console.WriteLine("=== ELIMINAR ARL ===");
            
            Console.Write("ID de la ARL a eliminar: ");
            if (!int.TryParse(Console.ReadLine(), out int arlId))
            {
                Console.WriteLine("ID de ARL inválido.");
                Console.ReadKey();
                return;
            }
            
            try
            {
                _service.EliminarArl(arlId);
                Console.WriteLine("ARL eliminada exitosamente.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al eliminar la ARL: {ex.Message}");
            }
            
            Console.ReadKey();
        }
    }
} 