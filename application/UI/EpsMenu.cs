using System;
using System.Collections.Generic;
using SGCI_app.application.services;
using SGCI_app.domain.Entities;
using SGCI_app.domain.Factory;
using SGCI_app.infrastructure.postgres;

namespace SGCI_app.application.UI
{
    public class EpsMenu
    {
        private readonly EpsService _service;

        public EpsMenu()
        {
            string connStr = "Host=localhost;database=sgci;Port=5432;Username=postgres;Password=1219;Pooling=true";
            var factory = new ConexDBFactory(connStr);
            _service = new EpsService(factory.CrearEpsRepository());
        }

        public void ShowMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== GESTIÓN DE EPS ===");
                Console.WriteLine("1. Crear EPS");
                Console.WriteLine("2. Listar EPS");
                Console.WriteLine("3. Actualizar EPS");
                Console.WriteLine("4. Eliminar EPS");
                Console.WriteLine("0. Volver al Menú Principal");
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
                        CrearEps();
                        break;
                    case "2":
                        ListarEps();
                        break;
                    case "3":
                        ActualizarEps();
                        break;
                    case "4":
                        EliminarEps();
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

        private void CrearEps()
        {
            Console.Clear();
            Console.WriteLine("=== CREAR EPS ===");

            var eps = new EPS();

            Console.Write("Nombre de la EPS: ");
            eps.Nombre = Console.ReadLine();

            try
            {
                _service.CrearEps(eps);
                Console.WriteLine("\nEPS creada exitosamente.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nError al crear la EPS: {ex.Message}");
            }

            Console.WriteLine("\nPresione cualquier tecla para continuar...");
            Console.ReadKey();
        }

        private void ListarEps()
        {
            Console.Clear();
            Console.WriteLine("=== LISTA DE EPS ===\n");

            try
            {
                _service.ObtenerTodasEps();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nError al obtener las EPS: {ex.Message}");
            }

            Console.WriteLine("\nPresione cualquier tecla para continuar...");
            Console.ReadKey();
        }

        private void ActualizarEps()
        {
            Console.Clear();
            Console.WriteLine("=== ACTUALIZAR EPS ===");

            Console.Write("Ingrese el ID de la EPS a actualizar: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("ID inválido.");
                Console.ReadKey();
                return;
            }

            var eps = new EPS { Id = id };

            Console.Write("Nuevo nombre (dejar en blanco para mantener el actual): ");
            eps.Nombre = Console.ReadLine();

            try
            {
                _service.ActualizarEps(id, eps);
                Console.WriteLine("\nEPS actualizada exitosamente.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nError al actualizar la EPS: {ex.Message}");
            }

            Console.WriteLine("\nPresione cualquier tecla para continuar...");
            Console.ReadKey();
        }

        private void EliminarEps()
        {
            Console.Clear();
            Console.WriteLine("=== ELIMINAR EPS ===");

            Console.Write("Ingrese el ID de la EPS a eliminar: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("ID inválido.");
                Console.ReadKey();
                return;
            }

            try
            {
                _service.EliminarEps(id);
                Console.WriteLine("\nEPS eliminada exitosamente.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nError al eliminar la EPS: {ex.Message}");
            }

            Console.WriteLine("\nPresione cualquier tecla para continuar...");
            Console.ReadKey();
        }
    }
} 