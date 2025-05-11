using System;
using SGCI_app.application.Services;
using SGCI_app.domain.Entities;
using SGCI_app.domain.Factory;
using SGCI_app.infrastructure.postgres;

namespace SGCI_app.application.UI
{
    public class CashMovementTypeMenu : BaseMenu
    {
        private readonly CashMovementTypeService _service;

        public CashMovementTypeMenu() : base(showIntro: false)
        {
            string connStr = "Host=localhost;database=sgci;Port=5432;Username=postgres;Password=1219;Pooling=true";
            var factory = new ConexDBFactory(connStr);
            _service = new CashMovementTypeService(factory.CrearCashMovementTypeRepository());
        }

        public override void ShowMenu()
        {
            while (true)
            {
                ShowHeader("GESTIÓN DE TIPOS DE MOVIMIENTO DE CAJA");
                Console.WriteLine("1. Crear Tipo de Movimiento");
                Console.WriteLine("2. Listar Tipos de Movimiento");
                Console.WriteLine("3. Actualizar Tipo de Movimiento");
                Console.WriteLine("4. Eliminar Tipo de Movimiento");
                Console.WriteLine("0. Volver al menú principal");
                DrawSeparator();

                int option = GetValidatedIntInput("Seleccione una opción: ", 0, 4);

                switch (option)
                {
                    case 1:
                        CrearTipo();
                        break;
                    case 2:
                        ListarTipos();
                        break;
                    case 3:
                        ActualizarTipo();
                        break;
                    case 4:
                        EliminarTipo();
                        break;
                    case 0:
                        return;
                }
            }
        }

        private void CrearTipo()
        {
            ShowHeader("CREAR TIPO DE MOVIMIENTO DE CAJA");

            string nombre = GetValidatedInput("Nombre: ");
            string tipo = GetValidatedInput("Tipo (p.ej. compra/venta): ");

            var entity = new CashMovementType { Nombre = nombre, Tipo = tipo };
            try
            {
                _service.CrearCashMovementType(entity);
                ShowSuccessMessage("Tipo de movimiento creado exitosamente.");
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Error al crear tipo de movimiento: {ex.Message}");
            }
        }

        private void ListarTipos()
        {
            ShowHeader("LISTA DE TIPOS DE MOVIMIENTO DE CAJA");
            try
            {
                _service.MostrarTodos();
                ShowInfoMessage("Listado de tipos completado.");
                Console.WriteLine();
                Console.Write("Presione cualquier tecla para continuar...");
                Console.ReadKey(true);
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Error al listar tipos: {ex.Message}");
            }
        }

        private void ActualizarTipo()
        {
            ShowHeader("ACTUALIZAR TIPO DE MOVIMIENTO DE CAJA");
            int id = GetValidatedIntInput("ID a actualizar: ");

            string nombre = GetValidatedInput("Nuevo nombre (dejar en blanco para mantener): ", allowEmpty: true);
            string tipo = GetValidatedInput("Nuevo tipo (dejar en blanco para mantener): ", allowEmpty: true);

            var entity = new CashMovementType
            {
                Nombre = string.IsNullOrWhiteSpace(nombre) ? null : nombre,
                Tipo = string.IsNullOrWhiteSpace(tipo) ? null : tipo
            };
            try
            {
                _service.ActualizarCashMovementType(id, entity);
                ShowSuccessMessage("Tipo actualizado exitosamente.");
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Error al actualizar tipo: {ex.Message}");
            }
        }

        private void EliminarTipo()
        {
            ShowHeader("ELIMINAR TIPO DE MOVIMIENTO DE CAJA");
            int id = GetValidatedIntInput("ID a eliminar: ");

            try
            {
                _service.EliminarCashMovementType(id);
                ShowSuccessMessage("Tipo eliminado exitosamente.");
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Error al eliminar tipo: {ex.Message}");
            }
        }
    }
}
