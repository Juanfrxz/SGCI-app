using System;
using SGCI_app.application.Services;
using SGCI_app.domain.Entities;
using SGCI_app.domain.Factory;
using SGCI_app.infrastructure.postgres;

namespace SGCI_app.application.UI
{
    public class CashMovementMenu : BaseMenu
    {
        private readonly CashMovementService _service;

        public CashMovementMenu() : base(showIntro: false)
        {
            string connStr = "Host=localhost;database=sgci;Port=5432;Username=postgres;Password=juan1374;Pooling=true";
            var factory = new ConexDBFactory(connStr);
            _service = new CashMovementService(factory.CrearCashMovementRepository());
        }

        public override void ShowMenu()
        {
            while (true)
            {
                ShowHeader("GESTIÓN DE MOVIMIENTOS DE CAJA");
                Console.WriteLine("1. Crear Movimiento");
                Console.WriteLine("2. Listar Movimientos");
                Console.WriteLine("3. Actualizar Movimiento");
                Console.WriteLine("4. Eliminar Movimiento");
                Console.WriteLine("5. Buscar por Sesión");
                Console.WriteLine("6. Buscar por Fecha");
                Console.WriteLine("7. Buscar por Tipo");
                Console.WriteLine("8. Buscar por Tercero");
                Console.WriteLine("0. Volver al menú principal");
                DrawSeparator();

                int option = GetValidatedIntInput("Seleccione una opción: ", 0, 8);

                switch (option)
                {
                    case 1:
                        CrearMovimiento();
                        break;
                    case 2:
                        ListarMovimientos();
                        break;
                    case 3:
                        ActualizarMovimiento();
                        break;
                    case 4:
                        EliminarMovimiento();
                        break;
                    case 5:
                        BuscarPorSesion();
                        break;
                    case 6:
                        BuscarPorFecha();
                        break;
                    case 7:
                        BuscarPorTipo();
                        break;
                    case 8:
                        BuscarPorTercero();
                        break;
                    case 0:
                        return;
                }
            }
        }

        private void CrearMovimiento()
        {
            ShowHeader("CREAR MOVIMIENTO DE CAJA");

            try
            {
                var movimiento = new CashMovement();
                movimiento.Fecha = GetValidatedDateInput("Fecha (YYYY-MM-DD): ");
                movimiento.TipoMovimiento_Id = GetValidatedIntInput("ID del tipo de movimiento: ");
                movimiento.Valor = GetValidatedIntInput("Valor: ");
                movimiento.Concepto = GetValidatedInput("Concepto (opcional): ", allowEmpty: true);
                movimiento.Tercero_Id = GetValidatedInput("ID del tercero: ", allowEmpty: true);
                movimiento.Sesion_Id = GetValidatedIntInput("ID de la sesión: ");

                _service.CrearMovimiento(movimiento);
                ShowSuccessMessage("Movimiento creado exitosamente.");
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Error al crear el movimiento: {ex.Message}");
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

        private void ActualizarMovimiento()
        {
            ShowHeader("ACTUALIZAR MOVIMIENTO DE CAJA");

            try
            {
                var movimiento = new CashMovement();
                movimiento.Id = GetValidatedIntInput("ID del movimiento a actualizar: ");

                string fechaInput = GetValidatedInput("Nueva fecha (YYYY-MM-DD, dejar en blanco para mantener): ", allowEmpty: true);
                if (!string.IsNullOrEmpty(fechaInput) && DateTime.TryParse(fechaInput, out DateTime fecha))
                    movimiento.Fecha = fecha;

                string tipoInput = GetValidatedInput("Nuevo ID del tipo de movimiento (dejar en blanco para mantener): ", allowEmpty: true);
                if (!string.IsNullOrEmpty(tipoInput) && int.TryParse(tipoInput, out int tipoId))
                    movimiento.TipoMovimiento_Id = tipoId;

                string valorInput = GetValidatedInput("Nuevo valor (dejar en blanco para mantener): ", allowEmpty: true);
                if (!string.IsNullOrEmpty(valorInput) && int.TryParse(valorInput, out int valor))
                    movimiento.Valor = valor;

                movimiento.Concepto = GetValidatedInput("Nuevo concepto (dejar en blanco para mantener): ", allowEmpty: true);
                movimiento.Tercero_Id = GetValidatedInput("Nuevo ID del tercero (dejar en blanco para mantener): ", allowEmpty: true);

                string sesionInput = GetValidatedInput("Nuevo ID de la sesión (dejar en blanco para mantener): ", allowEmpty: true);
                if (!string.IsNullOrEmpty(sesionInput) && int.TryParse(sesionInput, out int sesionId))
                    movimiento.Sesion_Id = sesionId;

                _service.ActualizarMovimiento(movimiento);
                ShowSuccessMessage("Movimiento actualizado exitosamente.");
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Error al actualizar el movimiento: {ex.Message}");
            }
        }

        private void EliminarMovimiento()
        {
            ShowHeader("ELIMINAR MOVIMIENTO DE CAJA");

            try
            {
                int id = GetValidatedIntInput("ID del movimiento a eliminar: ");
                _service.EliminarMovimiento(id);
                ShowSuccessMessage("Movimiento eliminado exitosamente.");
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Error al eliminar el movimiento: {ex.Message}");
            }
        }

        private void BuscarPorSesion()
        {
            ShowHeader("BUSCAR MOVIMIENTOS POR SESIÓN");

            try
            {
                int sesionId = GetValidatedIntInput("ID de la sesión: ");
                var movimientos = _service.ObtenerPorSesion(sesionId);
                
                foreach (var m in movimientos)
                {
                    Console.WriteLine(
                        $"ID: {m.Id}, Fecha: {m.Fecha.ToShortDateString()}, Tipo ID: {m.TipoMovimiento_Id}, " +
                        $"Valor: {m.Valor}, Concepto: {(string.IsNullOrEmpty(m.Concepto) ? "--" : m.Concepto)}, " +
                        $"Tercero ID: {(string.IsNullOrEmpty(m.Tercero_Id) ? "--" : m.Tercero_Id)}, Sesión ID: {m.Sesion_Id}");
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Error al buscar los movimientos: {ex.Message}");
            }
            Console.WriteLine();
            Console.Write("Presione cualquier tecla para continuar...");
            Console.ReadKey(true);
        }

        private void BuscarPorFecha()
        {
            ShowHeader("BUSCAR MOVIMIENTOS POR FECHA");

            try
            {
                DateTime fecha = GetValidatedDateInput("Fecha (YYYY-MM-DD): ");
                var movimientos = _service.ObtenerPorFecha(fecha);
                
                foreach (var m in movimientos)
                {
                    Console.WriteLine(
                        $"ID: {m.Id}, Fecha: {m.Fecha.ToShortDateString()}, Tipo ID: {m.TipoMovimiento_Id}, " +
                        $"Valor: {m.Valor}, Concepto: {(string.IsNullOrEmpty(m.Concepto) ? "--" : m.Concepto)}, " +
                        $"Tercero ID: {(string.IsNullOrEmpty(m.Tercero_Id) ? "--" : m.Tercero_Id)}, Sesión ID: {m.Sesion_Id}");
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Error al buscar los movimientos: {ex.Message}");
            }
            Console.WriteLine();
            Console.Write("Presione cualquier tecla para continuar...");
            Console.ReadKey(true);
        }

        private void BuscarPorTipo()
        {
            ShowHeader("BUSCAR MOVIMIENTOS POR TIPO");

            try
            {
                int tipoId = GetValidatedIntInput("ID del tipo de movimiento: ");
                var movimientos = _service.ObtenerPorTipo(tipoId);
                
                foreach (var m in movimientos)
                {
                    Console.WriteLine(
                        $"ID: {m.Id}, Fecha: {m.Fecha.ToShortDateString()}, Tipo ID: {m.TipoMovimiento_Id}, " +
                        $"Valor: {m.Valor}, Concepto: {(string.IsNullOrEmpty(m.Concepto) ? "--" : m.Concepto)}, " +
                        $"Tercero ID: {(string.IsNullOrEmpty(m.Tercero_Id) ? "--" : m.Tercero_Id)}, Sesión ID: {m.Sesion_Id}");
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Error al buscar los movimientos: {ex.Message}");
            }
            Console.WriteLine();
            Console.Write("Presione cualquier tecla para continuar...");
            Console.ReadKey(true);
        }

        private void BuscarPorTercero()
        {
            ShowHeader("BUSCAR MOVIMIENTOS POR TERCERO");

            try
            {
                string terceroId = GetValidatedInput("ID del tercero: ");
                var movimientos = _service.ObtenerPorTercero(terceroId);
                
                foreach (var m in movimientos)
                {
                    Console.WriteLine(
                        $"ID: {m.Id}, Fecha: {m.Fecha.ToShortDateString()}, Tipo ID: {m.TipoMovimiento_Id}, " +
                        $"Valor: {m.Valor}, Concepto: {(string.IsNullOrEmpty(m.Concepto) ? "--" : m.Concepto)}, " +
                        $"Tercero ID: {(string.IsNullOrEmpty(m.Tercero_Id) ? "--" : m.Tercero_Id)}, Sesión ID: {m.Sesion_Id}");
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Error al buscar los movimientos: {ex.Message}");
            }
            Console.WriteLine();
            Console.Write("Presione cualquier tecla para continuar...");
            Console.ReadKey(true);
        }

        private DateTime GetValidatedDateInput(string prompt)
        {
            while (true)
            {
                string input = GetValidatedInput(prompt);
                if (DateTime.TryParse(input, out DateTime date))
                    return date;
                ShowErrorMessage("Formato de fecha inválido. Use YYYY-MM-DD.");
            }
        }
    }
}
