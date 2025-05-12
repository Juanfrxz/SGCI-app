using System;
using SGCI_app.application.Services;
using SGCI_app.domain.Entities;
using SGCI_app.domain.Factory;
using SGCI_app.infrastructure.postgres;
using System.Linq;

namespace SGCI_app.application.UI
{
    public class CashSessionMenu : BaseMenu
    {
        private readonly CashSessionService _service;

        public CashSessionMenu() : base(showIntro: false)
        {
            string connStr = "Host=localhost;database=sgci;Port=5432;Username=postgres;Password=1219;Pooling=true";
            var factory = new ConexDBFactory(connStr);
            _service = new CashSessionService(factory.CrearCashSessionRepository());
        }

        public override void ShowMenu()
        {
            while (true)
            {
                ShowHeader("GESTIÓN DE SESIONES DE CAJA");
                Console.WriteLine("1. Abrir Sesión");
                Console.WriteLine("2. Cerrar Sesión");
                Console.WriteLine("3. Listar Sesiones");
                Console.WriteLine("4. Eliminar Sesión");
                Console.WriteLine("0. Volver al menú principal");
                DrawSeparator();

                int option = GetValidatedIntInput("Seleccione una opción: ", 0, 4);

                switch (option)
                {
                    case 1:
                        AbrirSesion();
                        break;
                    case 2:
                        CerrarSesion();
                        break;
                    case 3:
                        ListarSesiones();
                        break;
                    case 4:
                        EliminarSesion();
                        break;
                    case 0:
                        return;
                }
            }
        }

        private void AbrirSesion()
        {
            ShowHeader("ABRIR SESIÓN DE CAJA");
            int balanceApertura = GetValidatedIntInput("Balance de apertura: ");

            try
            {
                _service.CrearCashSession(balanceApertura);
                ShowSuccessMessage("Sesión abierta exitosamente.");
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Error al abrir sesión: {ex.Message}");
            }
        }

        private void CerrarSesion()
        {
            ShowHeader("CERRAR SESIÓN DE CAJA");
            int id = GetValidatedIntInput("ID de sesión a cerrar: ");

            try
            {
                _service.CerrarCashSession(id);
                ShowSuccessMessage("Sesión cerrada exitosamente.");
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Error al cerrar sesión: {ex.Message}");
            }
        }

        private void ListarSesiones()
        {
            ShowHeader("LISTA DE SESIONES DE CAJA");
            try
            {
                var sesiones = _service.ObtenerTodasLasSessions();
                if (sesiones == null || !sesiones.Any())
                {
                    ShowInfoMessage("No hay sesiones registradas.");
                }
                else
                {
                    foreach (var s in sesiones)
                    {
                        Console.WriteLine($"ID: {s.Id}\tApertura: {s.AperturaCaja}\tCierre: {(s.CierreCaja.HasValue ? s.CierreCaja.ToString() : "--")}\tBalance Apertura: {s.BalanceApertura}\tBalance Cierre: {s.BalaceCierre}");
                    }
                }
                Console.WriteLine();
                Console.Write("Presione cualquier tecla para continuar...");
                Console.ReadKey(true);
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Error al listar sesiones: {ex.Message}");
            }
        }

        private void EliminarSesion()
        {
            ShowHeader("ELIMINAR SESIÓN DE CAJA");
            int id = GetValidatedIntInput("ID de sesión a eliminar: ");

            try
            {
                _service.EliminarCashSession(id);
                ShowSuccessMessage("Sesión eliminada exitosamente.");
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Error al eliminar sesión: {ex.Message}");
            }
        }
    }
}
