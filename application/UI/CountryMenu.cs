using System;
using SGCI_app.domain.Entities;
using SGCI_app.domain.Factory;
using SGCI_app.infrastructure.postgres;
using SGCI_app.application.services;

namespace SGCI_app.application.UI
{
    public class CountryMenu
    {
        private readonly CountryService _service;

        public CountryMenu()
        {
            string connStr = "Host=localhost;database=sgci;Port=5432;Username=postgres;Password=campus2023;Pooling=true";
            var factory = new ConexDBFactory(connStr);
            _service = new CountryService(factory.CrearCountryRepository());
        }

        public void ShowMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== GESTIÓN DE PAÍSES ===");
                Console.WriteLine("1. Crear País");
                Console.WriteLine("2. Listar Países");
                Console.WriteLine("3. Actualizar País");
                Console.WriteLine("4. Eliminar País");
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
                        CrearPais();
                        break;
                    case "2":
                        ListarPaises();
                        break;
                    case "3":
                        ActualizarPais();
                        break;
                    case "4":
                        EliminarPais();
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

        private void CrearPais()
        {
            Console.Clear();
            Console.WriteLine("=== CREAR NUEVO PAÍS ===");
            
            Console.Write("Nombre del país: ");
            string? nombre = Console.ReadLine();

            var pais = new Country { Nombre = nombre };
            
            try
            {
                _service.CrearCountry(pais);
                Console.WriteLine("País creado exitosamente.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al crear el país: {ex.Message}");
            }
            
            Console.ReadKey();
        }

        private void ListarPaises()
        {
            Console.Clear();
            Console.WriteLine("=== LISTA DE PAÍSES ===");
            
            try
            {
                _service.MostrarTodos();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al listar los países: {ex.Message}");
            }
            
            Console.WriteLine("\nPresione cualquier tecla para continuar...");
            Console.ReadKey();
        }

        private void ActualizarPais()
        {
            Console.Clear();
            Console.WriteLine("=== ACTUALIZAR PAÍS ===");
            
            Console.Write("ID del país a actualizar: ");
            if (!int.TryParse(Console.ReadLine(), out int paisId))
            {
                Console.WriteLine("ID de país inválido.");
                Console.ReadKey();
                return;
            }

            Console.Write("Nuevo nombre: ");
            string? nuevoNombre = Console.ReadLine();

            var pais = new Country { Id = paisId, Nombre = nuevoNombre };
            
            try
            {
                _service.ActualizarCountry(pais);
                Console.WriteLine("País actualizado exitosamente.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al actualizar el país: {ex.Message}");
            }
            
            Console.ReadKey();
        }

        private void EliminarPais()
        {
            Console.Clear();
            Console.WriteLine("=== ELIMINAR PAÍS ===");
            
            Console.Write("ID del país a eliminar: ");
            if (!int.TryParse(Console.ReadLine(), out int paisId))
            {
                Console.WriteLine("ID de país inválido.");
                Console.ReadKey();
                return;
            }
            
            try
            {
                _service.EliminarCountry(paisId);
                Console.WriteLine("País eliminado exitosamente.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al eliminar el país: {ex.Message}");
            }
            
            Console.ReadKey();
        }
    }
} 