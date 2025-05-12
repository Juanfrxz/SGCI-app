using System;
using SGCI_app.domain.DTO;
using SGCI_app.domain.Factory;
using SGCI_app.infrastructure.postgres;
using SGCI_app.application.services;

namespace SGCI_app.application.UI
{
    public class EmployeeMenu : BaseMenu
    {
        private readonly EmployeeService _service;

        public EmployeeMenu() : base(showIntro: false)
        {
            string connStr = "Host=localhost;database=sgci;Port=5432;Username=postgres;Password=1219;Pooling=true";
            var factory = new ConexDBFactory(connStr);
            _service = new EmployeeService(factory.CrearEmployeeRepository());
        }

        public override void ShowMenu()
        {
            while (true)
            {
                ShowHeader("GESTIÓN DE EMPLEADOS");
                Console.WriteLine("1. Crear Empleado");
                Console.WriteLine("2. Listar Empleados");
                Console.WriteLine("3. Actualizar Empleado");
                Console.WriteLine("4. Eliminar Empleado");
                Console.WriteLine("0. Volver al menú principal");
                DrawSeparator();

                int option = GetValidatedIntInput("Seleccione una opción: ", 0, 4);
                switch (option)
                {
                    case 1:
                        CrearEmpleado();
                        break;
                    case 2:
                        ListarEmpleados();
                        break;
                    case 3:
                        ActualizarEmpleado();
                        break;
                    case 4:
                        EliminarEmpleado();
                        break;
                    case 0:
                        return;
                }
            }
        }

        private void CrearEmpleado()
        {
            ShowHeader("CREAR NUEVO EMPLEADO");
            
            // Datos de dirección
            Console.WriteLine("\nDatos de dirección:");
            Console.Write("Calle: ");
            string? calle = Console.ReadLine();
            
            Console.Write("Número de edificio: ");
            string? numeroEdificio = Console.ReadLine();
            
            Console.Write("Código postal: ");
            string? codigoPostal = Console.ReadLine();
            
            Console.Write("ID de ciudad: ");
            if (!int.TryParse(Console.ReadLine(), out int ciudadId))
            {
                ShowErrorMessage("ID de ciudad inválido.");
                return;
            }
            
            Console.Write("Información adicional: ");
            string? infoAdicional = Console.ReadLine();

            // Datos personales
            Console.WriteLine("\nDatos personales:");
            Console.Write("Nombre: ");
            string? nombre = Console.ReadLine();
            
            Console.Write("Apellidos: ");
            string? apellidos = Console.ReadLine();
            
            Console.Write("Email: ");
            string? email = Console.ReadLine();
            
            Console.Write("ID del tipo de tercero: ");
            if (!int.TryParse(Console.ReadLine(), out int tipoTerceroId))
            {
                ShowErrorMessage("ID de tipo de tercero inválido.");
                return;
            }
            
            Console.Write("ID del tipo de documento: ");
            if (!int.TryParse(Console.ReadLine(), out int tipoDocId))
            {
                ShowErrorMessage("ID de tipo de documento inválido.");
                return;
            }

            // Datos de empleado
            Console.WriteLine("\nDatos de empleado:");
            Console.Write("Fecha de ingreso (YYYY-MM-DD): ");
            if (!DateTime.TryParse(Console.ReadLine(), out DateTime fechaIngreso))
            {
                ShowErrorMessage("Fecha inválida.");
                return;
            }
            
            Console.Write("Salario base: ");
            if (!double.TryParse(Console.ReadLine(), out double salarioBase))
            {
                ShowErrorMessage("Salario inválido.");
                return;
            }
            
            Console.Write("ID de EPS: ");
            if (!int.TryParse(Console.ReadLine(), out int epsId))
            {
                ShowErrorMessage("ID de EPS inválido.");
                return;
            }
            
            Console.Write("ID de ARL: ");
            if (!int.TryParse(Console.ReadLine(), out int arlId))
            {
                ShowErrorMessage("ID de ARL inválido.");
                return;
            }

            var empleado = new DtoEmployee 
            { 
                Address = new DtoAddress
                {
                    Calle = calle,
                    NumeroEdificio = numeroEdificio,
                    CodigoPostal = codigoPostal,
                    Ciudad_Id = ciudadId,
                    InfoAdicional = infoAdicional
                },
                Nombre = nombre,
                Apellidos = apellidos,
                Email = email,
                TipoTercero_id = tipoTerceroId,
                TipoDoc_id = tipoDocId,
                Employee = new DtoEmp
                {
                    FechaIngreso = fechaIngreso,
                    SalarioBase = salarioBase,
                    Eps_id = epsId,
                    Arl_id = arlId
                }
            };
            
            try
            {
                _service.CrearEmpleado(empleado);
                ShowSuccessMessage("Empleado creado exitosamente.");
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Error al crear el empleado: {ex.Message}");
            }
        }

        private void ListarEmpleados()
        {
            ShowHeader("LISTA DE EMPLEADOS");
            try
            {
                _service.MostrarTodos();
                ShowInfoMessage("Listado de empleados completado.");
                Console.WriteLine();
                Console.Write("Presione cualquier tecla para continuar...");
                Console.ReadKey(true);
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Error al listar los empleados: {ex.Message}");
            }
        }

        private void ActualizarEmpleado()
        {
            ShowHeader("ACTUALIZAR EMPLEADO");
            
            Console.Write("ID del empleado a actualizar: ");
            if (!int.TryParse(Console.ReadLine(), out int empleadoId))
            {
                ShowErrorMessage("ID de empleado inválido.");
                return;
            }

            // Datos personales
            Console.WriteLine("\nDatos personales:");
            Console.Write("Nuevo nombre: ");
            string? nombre = Console.ReadLine();
            
            Console.Write("Nuevos apellidos: ");
            string? apellidos = Console.ReadLine();
            
            Console.Write("Nuevo email: ");
            string? email = Console.ReadLine();
            
            Console.Write("Nuevo ID del tipo de tercero: ");
            if (!int.TryParse(Console.ReadLine(), out int tipoTerceroId))
            {
                ShowErrorMessage("ID de tipo de tercero inválido.");
                return;
            }
            
            Console.Write("Nuevo ID del tipo de documento: ");
            if (!int.TryParse(Console.ReadLine(), out int tipoDocId))
            {
                ShowErrorMessage("ID de tipo de documento inválido.");
                return;
            }

            // Datos de empleado
            Console.WriteLine("\nDatos de empleado:");
            Console.Write("Nuevo salario base: ");
            if (!double.TryParse(Console.ReadLine(), out double salarioBase))
            {
                ShowErrorMessage("Salario inválido.");
                return;
            }
            
            Console.Write("Nuevo ID de EPS: ");
            if (!int.TryParse(Console.ReadLine(), out int epsId))
            {
                ShowErrorMessage("ID de EPS inválido.");
                return;
            }
            
            Console.Write("Nuevo ID de ARL: ");
            if (!int.TryParse(Console.ReadLine(), out int arlId))
            {
                ShowErrorMessage("ID de ARL inválido.");
                return;
            }

            var empleado = new DtoEmployee 
            { 
                Nombre = nombre,
                Apellidos = apellidos,
                Email = email,
                TipoTercero_id = tipoTerceroId,
                TipoDoc_id = tipoDocId,
                Employee = new DtoEmp
                {
                    SalarioBase = salarioBase,
                    Eps_id = epsId,
                    Arl_id = arlId
                }
            };
            
            try
            {
                _service.ActualizarDatosEmpleado(empleadoId, empleado);
                ShowSuccessMessage("Empleado actualizado exitosamente.");
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Error al actualizar el empleado: {ex.Message}");
            }
        }

        private void EliminarEmpleado()
        {
            ShowHeader("ELIMINAR EMPLEADO");
            
            Console.Write("ID del empleado a eliminar: ");
            if (!int.TryParse(Console.ReadLine(), out int empleadoId))
            {
                ShowErrorMessage("ID de empleado inválido.");
                return;
            }
            
            try
            {
                _service.EliminarEmpleado(empleadoId);
                ShowSuccessMessage("Empleado eliminado exitosamente.");
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Error al eliminar el empleado: {ex.Message}");
            }
        }
    }
} 