// CashSessionMenu.cs
using System;
using SGCI_app.application.Services;
using SGCI_app.domain.Entities;
using SGCI_app.domain.Factory;
using SGCI_app.infrastructure.postgres;

namespace SGCI_app.application.UI
{
    public class CashSessionMenu
    {
        private readonly CashSessionService _service;

        public CashSessionMenu()
        {
            string connStr = "Host=localhost;database=sgci;Port=5432;Username=postgres;Password=1219;Pooling=true";
            var factory = new ConexDBFactory(connStr);
            _service = new CashSessionService(factory.CrearCashSessionRepository());
        }

        public void ShowMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== GESTIÓN DE SESIONES DE CAJA ===");
                Console.WriteLine("1. Abrir Sesión");
                Console.WriteLine("2. Cerrar Sesión");
                Console.WriteLine("3. Listar Sesiones");
                Console.WriteLine("4. Eliminar Sesión");
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
                    case "1": AbrirSesion(); break;
                    case "2": CerrarSesion(); break;
                    case "3": ListarSesiones(); break;
                    case "4": EliminarSesion(); break;
                    case "0": return;
                    default:
                        Console.WriteLine("Opción no válida. Presione cualquier tecla para continuar...");
                        Console.ReadKey();
                        break;
                }
            }
        }

        private void AbrirSesion()
        {
            Console.Clear();
            Console.WriteLine("=== ABRIR SESIÓN DE CAJA ===");
            Console.Write("Balance de apertura: ");
            if (!int.TryParse(Console.ReadLine(), out int balanceApertura))
            {
                Console.WriteLine("Balance inválido.");
                Console.ReadKey();
                return;
            }

            try
            {
                _service.CrearCashSession(balanceApertura);
                Console.WriteLine("\nSesión abierta exitosamente.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nError al abrir sesión: {ex.Message}");
            }

            Console.WriteLine("\nPresione cualquier tecla para continuar...");
            Console.ReadKey();
        }

        private void CerrarSesion()
        {
            Console.Clear();
            Console.WriteLine("=== CERRAR SESIÓN DE CAJA ===");
            Console.Write("ID de sesión a cerrar: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("ID inválido.");
                Console.ReadKey();
                return;
            }

            try
            {
                _service.CerrarCashSession(id);
                Console.WriteLine("\nSesión cerrada exitosamente.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nError al cerrar sesión: {ex.Message}");
            }

            Console.WriteLine("\nPresione cualquier tecla para continuar...");
            Console.ReadKey();
        }

        private void ListarSesiones()
        {
            Console.Clear();
            Console.WriteLine("=== LISTA DE SESIONES DE CAJA ===\n");
            try
            {
                var lista = _service.ObtenerTodasLasSessions();
                foreach (var s in lista)
                {
                    Console.WriteLine($"ID: {s.Id}, Apertura: {s.AperturaCaja}, Cierre: { (s.CierreCaja.HasValue ? s.CierreCaja.ToString() : "--") }, Balance Apertura: {s.BalanceApertura}, Balance Cierre: {s.BalaceCierre}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nError al listar sesiones: {ex.Message}");
            }

            Console.WriteLine("\nPresione cualquier tecla para continuar...");
            Console.ReadKey();
        }

        private void EliminarSesion()
        {
            Console.Clear();
            Console.WriteLine("=== ELIMINAR SESIÓN DE CAJA ===");
            Console.Write("ID de sesión a eliminar: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("ID inválido.");
                Console.ReadKey();
                return;
            }

            try
            {
                _service.EliminarCashSession(id);
                Console.WriteLine("\nSesión eliminada exitosamente.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nError al eliminar sesión: {ex.Message}");
            }

            Console.WriteLine("\nPresione cualquier tecla para continuar...");
            Console.ReadKey();
        }
    }
}