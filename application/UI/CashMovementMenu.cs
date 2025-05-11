using System;
using SGCI_app.application.Services;
using SGCI_app.domain.Factory;
using SGCI_app.infrastructure.postgres;

namespace SGCI_app.application.UI
{
    public class CashMovementMenu : BaseMenu
    {
        private readonly CashMovementService _service;

        public CashMovementMenu() : base(showIntro: false)
        {
            string connStr = "Host=localhost;database=sgci;Port=5432;Username=postgres;Password=1219;Pooling=true";
            var factory = new ConexDBFactory(connStr);
            _service = new CashMovementService(factory.CrearCashMovementRepository());
        }

        public override void ShowMenu()
        {
            while (true)
            {
                ShowHeader("GESTIÓN DE MOVIMIENTOS DE CAJA");
                Console.WriteLine("1. Listar Movimientos");
                Console.WriteLine("0. Volver al menú principal");
                DrawSeparator();

                int option = GetValidatedIntInput("Seleccione una opción: ", 0, 1);

                switch (option)
                {
                    case 1:
                        ListarMovimientos();
                        break;
                    case 0:
                        return;
                }
            }
        }

        private void ListarMovimientos()
        {
            ShowHeader("LISTA DE MOVIMIENTOS DE CAJA");

            try
            {
                _service.MostrarTodos();
                ShowInfoMessage("Listado completado.");
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Error al listar movimientos: {ex.Message}");
            }
            Console.WriteLine();
            Console.Write("Presione cualquier tecla para continuar...");
            Console.ReadKey(true);
        }
    }
}
