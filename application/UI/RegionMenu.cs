using System;
using SGCI_app.domain.Entities;
using SGCI_app.domain.Factory;
using SGCI_app.infrastructure.postgres;
using SGCI_app.application.services;

namespace SGCI_app.application.UI
{
    public class RegionMenu
    {
        private readonly RegionService _service;

        public RegionMenu()
        {
            string connStr = "Host=localhost;database=sgci;Port=5432;Username=postgres;Password=1219;Pooling=true";
            var factory = new ConexDBFactory(connStr);
            _service = new RegionService(factory.CrearRegionRepository());
        }

        public void ShowMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== GESTIÓN DE REGIONES ===");
                Console.WriteLine("1. Crear Región");
                Console.WriteLine("2. Listar Regiones");
                Console.WriteLine("3. Actualizar Región");
                Console.WriteLine("4. Eliminar Región");
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
                        CrearRegion();
                        break;
                    case "2":
                        ListarRegiones();
                        break;
                    case "3":
                        ActualizarRegion();
                        break;
                    case "4":
                        EliminarRegion();
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

        private void CrearRegion()
        {
            Console.Clear();
            Console.WriteLine("=== CREAR NUEVA REGIÓN ===");
            
            Console.Write("Nombre de la región: ");
            string? nombre = Console.ReadLine();

            Console.Write("ID del país: ");
            if (!int.TryParse(Console.ReadLine(), out int paisId))
            {
                Console.WriteLine("ID de país inválido.");
                Console.ReadKey();
                return;
            }

            var region = new Region { Nombre = nombre, Pais_Id = paisId };
            
            try
            {
                _service.CrearRegion(region);
                Console.WriteLine("Región creada exitosamente.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al crear la región: {ex.Message}");
            }
            
            Console.ReadKey();
        }

        private void ListarRegiones()
        {
            Console.Clear();
            Console.WriteLine("=== LISTA DE REGIONES ===");
            
            try
            {
                _service.MostrarTodos();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al listar las regiones: {ex.Message}");
            }
            
            Console.WriteLine("\nPresione cualquier tecla para continuar...");
            Console.ReadKey();
        }

        private void ActualizarRegion()
        {
            Console.Clear();
            Console.WriteLine("=== ACTUALIZAR REGIÓN ===");
            
            Console.Write("ID de la región a actualizar: ");
            if (!int.TryParse(Console.ReadLine(), out int regionId))
            {
                Console.WriteLine("ID de región inválido.");
                Console.ReadKey();
                return;
            }

            Console.Write("Nuevo nombre: ");
            string? nuevoNombre = Console.ReadLine();

            Console.Write("Nuevo ID de país: ");
            if (!int.TryParse(Console.ReadLine(), out int nuevoPaisId))
            {
                Console.WriteLine("ID de país inválido.");
                Console.ReadKey();
                return;
            }

            var region = new Region { Id = regionId, Nombre = nuevoNombre, Pais_Id = nuevoPaisId };
            
            try
            {
                _service.ActualuizarRegion(region);
                Console.WriteLine("Región actualizada exitosamente.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al actualizar la región: {ex.Message}");
            }
            
            Console.ReadKey();
        }

        private void EliminarRegion()
        {
            Console.Clear();
            Console.WriteLine("=== ELIMINAR REGIÓN ===");
            
            Console.Write("ID de la región a eliminar: ");
            if (!int.TryParse(Console.ReadLine(), out int regionId))
            {
                Console.WriteLine("ID de región inválido.");
                Console.ReadKey();
                return;
            }
            
            try
            {
                _service.EliminarRegion(regionId);
                Console.WriteLine("Región eliminada exitosamente.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al eliminar la región: {ex.Message}");
            }
            
            Console.ReadKey();
        }
    }
} 