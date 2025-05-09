using System;
using SGCI_app.domain.Entities;
using SGCI_app.domain.Factory;
using SGCI_app.infrastructure.postgres;
using SGCI_app.application.services;

namespace SGCI_app.application.UI
{
    public class CityMenu
    {
        private readonly CityService _service;

        public CityMenu()
        {
            string connStr = "Host=localhost;database=sgci;Port=5432;Username=postgres;Password=juan1374;Pooling=true";
            var factory = new ConexDBFactory(connStr);
            _service = new CityService(factory.CrearCityRepository());
        }

        public void ShowMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== GESTIÓN DE CIUDADES ===");
                Console.WriteLine("1. Crear Ciudad");
                Console.WriteLine("2. Listar Ciudades");
                Console.WriteLine("3. Actualizar Ciudad");
                Console.WriteLine("4. Eliminar Ciudad");
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
                        CrearCiudad();
                        break;
                    case "2":
                        ListarCiudades();
                        break;
                    case "3":
                        ActualizarCiudad();
                        break;
                    case "4":
                        EliminarCiudad();
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

        private void CrearCiudad()
        {
            Console.Clear();
            Console.WriteLine("=== CREAR NUEVA CIUDAD ===");
            
            Console.Write("Nombre de la ciudad: ");
            string? nombre = Console.ReadLine();
            
            Console.Write("ID de la región: ");
            if (!int.TryParse(Console.ReadLine(), out int regionId))
            {
                Console.WriteLine("ID de región inválido.");
                Console.ReadKey();
                return;
            }

            var ciudad = new City { Nombre = nombre, Region_Id = regionId };
            
            try
            {
                _service.CrearCity(ciudad);
                Console.WriteLine("Ciudad creada exitosamente.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al crear la ciudad: {ex.Message}");
            }
            
            Console.ReadKey();
        }

        private void ListarCiudades()
        {
            Console.Clear();
            Console.WriteLine("=== LISTA DE CIUDADES ===");
            
            try
            {
                _service.MostrarTodos();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al listar las ciudades: {ex.Message}");
            }
            
            Console.WriteLine("\nPresione cualquier tecla para continuar...");
            Console.ReadKey();
        }

        private void ActualizarCiudad()
        {
            Console.Clear();
            Console.WriteLine("=== ACTUALIZAR CIUDAD ===");
            
            Console.Write("ID de la ciudad a actualizar: ");
            if (!int.TryParse(Console.ReadLine(), out int ciudadId))
            {
                Console.WriteLine("ID de ciudad inválido.");
                Console.ReadKey();
                return;
            }

            Console.Write("Nuevo nombre: ");
            string? nuevoNombre = Console.ReadLine();
            
            Console.Write("Nuevo ID de región: ");
            if (!int.TryParse(Console.ReadLine(), out int nuevaRegionId))
            {
                Console.WriteLine("ID de región inválido.");
                Console.ReadKey();
                return;
            }

            var ciudad = new City { Id = ciudadId, Nombre = nuevoNombre, Region_Id = nuevaRegionId };
            
            try
            {
                _service.ActualizarCity(ciudad);
                Console.WriteLine("Ciudad actualizada exitosamente.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al actualizar la ciudad: {ex.Message}");
            }
            
            Console.ReadKey();
        }

        private void EliminarCiudad()
        {
            Console.Clear();
            Console.WriteLine("=== ELIMINAR CIUDAD ===");
            
            Console.Write("ID de la ciudad a eliminar: ");
            if (!int.TryParse(Console.ReadLine(), out int ciudadId))
            {
                Console.WriteLine("ID de ciudad inválido.");
                Console.ReadKey();
                return;
            }
            
            try
            {
                _service.EliminarCity(ciudadId);
                Console.WriteLine("Ciudad eliminada exitosamente.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al eliminar la ciudad: {ex.Message}");
            }
            
            Console.ReadKey();
        }
    }
} 