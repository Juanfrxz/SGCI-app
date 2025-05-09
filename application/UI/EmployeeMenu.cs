using System;
using SGCI_app.domain.DTO;
using SGCI_app.domain.Factory;
using SGCI_app.infrastructure.postgres;
using SGCI_app.application.services;

namespace SGCI_app.application.UI
{
    public class EmployeeMenu
    {
        private readonly EmployeeService _service;

        public EmployeeMenu()
        {
            string connStr = "Host=localhost;database=sgci;Port=5432;Username=postgres;Password=campus2023;Pooling=true";
            var factory = new ConexDBFactory(connStr);
            _service = new EmployeeService(factory.CrearEmployeeRepository());
        }

        public void ShowMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== GESTIÓN DE EMPLEADOS ===");
                Console.WriteLine("1. Crear Empleado");
                Console.WriteLine("2. Listar Empleados");
                Console.WriteLine("3. Actualizar Empleado");
                Console.WriteLine("4. Eliminar Empleado");
                Console.WriteLine("5. Gestión de Empleados");
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
                        CrearEmpleado();
                        break;
                    case "2":
                        ListarEmpleados();
                        break;
                    case "3":
                        ActualizarEmpleado();
                        break;
                    case "4":
                        EliminarEmpleado();
                        break;
                    case "5":
                        var employeeMenu = new EmployeeMenu();
                        employeeMenu.ShowMenu();
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

        private void CrearEmpleado()
        {
            Console.Clear();
            Console.WriteLine("=== CREAR NUEVO EMPLEADO ===");
            
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
                Console.WriteLine("ID de ciudad inválido.");
                Console.ReadKey();
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
                Console.WriteLine("ID de tipo de tercero inválido.");
                Console.ReadKey();
                return;
            }
            
            Console.Write("ID del tipo de documento: ");
            if (!int.TryParse(Console.ReadLine(), out int tipoDocId))
            {
                Console.WriteLine("ID de tipo de documento inválido.");
                Console.ReadKey();
                return;
            }

            // Datos de empleado
            Console.WriteLine("\nDatos de empleado:");
            Console.Write("Fecha de ingreso (YYYY-MM-DD): ");
            if (!DateTime.TryParse(Console.ReadLine(), out DateTime fechaIngreso))
            {
                Console.WriteLine("Fecha inválida.");
                Console.ReadKey();
                return;
            }
            
            Console.Write("Salario base: ");
            if (!double.TryParse(Console.ReadLine(), out double salarioBase))
            {
                Console.WriteLine("Salario inválido.");
                Console.ReadKey();
                return;
            }
            
            Console.Write("ID de EPS: ");
            if (!int.TryParse(Console.ReadLine(), out int epsId))
            {
                Console.WriteLine("ID de EPS inválido.");
                Console.ReadKey();
                return;
            }
            
            Console.Write("ID de ARL: ");
            if (!int.TryParse(Console.ReadLine(), out int arlId))
            {
                Console.WriteLine("ID de ARL inválido.");
                Console.ReadKey();
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
                Console.WriteLine("Empleado creado exitosamente.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al crear el empleado: {ex.Message}");
            }
            
            Console.ReadKey();
        }

        private void ListarEmpleados()
        {
            Console.Clear();
            Console.WriteLine("=== LISTA DE EMPLEADOS ===");
            
            try
            {
                _service.MostrarTodos();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al listar los empleados: {ex.Message}");
            }
            
            Console.WriteLine("\nPresione cualquier tecla para continuar...");
            Console.ReadKey();
        }

        private void ActualizarEmpleado()
        {
            Console.Clear();
            Console.WriteLine("=== ACTUALIZAR EMPLEADO ===");
            
            Console.Write("ID del empleado a actualizar: ");
            if (!int.TryParse(Console.ReadLine(), out int empleadoId))
            {
                Console.WriteLine("ID de empleado inválido.");
                Console.ReadKey();
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
                Console.WriteLine("ID de tipo de tercero inválido.");
                Console.ReadKey();
                return;
            }
            
            Console.Write("Nuevo ID del tipo de documento: ");
            if (!int.TryParse(Console.ReadLine(), out int tipoDocId))
            {
                Console.WriteLine("ID de tipo de documento inválido.");
                Console.ReadKey();
                return;
            }

            // Datos de empleado
            Console.WriteLine("\nDatos de empleado:");
            Console.Write("Nuevo salario base: ");
            if (!double.TryParse(Console.ReadLine(), out double salarioBase))
            {
                Console.WriteLine("Salario inválido.");
                Console.ReadKey();
                return;
            }
            
            Console.Write("Nuevo ID de EPS: ");
            if (!int.TryParse(Console.ReadLine(), out int epsId))
            {
                Console.WriteLine("ID de EPS inválido.");
                Console.ReadKey();
                return;
            }
            
            Console.Write("Nuevo ID de ARL: ");
            if (!int.TryParse(Console.ReadLine(), out int arlId))
            {
                Console.WriteLine("ID de ARL inválido.");
                Console.ReadKey();
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
                Console.WriteLine("Empleado actualizado exitosamente.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al actualizar el empleado: {ex.Message}");
            }
            
            Console.ReadKey();
        }

        private void EliminarEmpleado()
        {
            Console.Clear();
            Console.WriteLine("=== ELIMINAR EMPLEADO ===");
            
            Console.Write("ID del empleado a eliminar: ");
            if (!int.TryParse(Console.ReadLine(), out int empleadoId))
            {
                Console.WriteLine("ID de empleado inválido.");
                Console.ReadKey();
                return;
            }
            
            try
            {
                _service.EliminarEmpleado(empleadoId);
                Console.WriteLine("Empleado eliminado exitosamente.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al eliminar el empleado: {ex.Message}");
            }
            
            Console.ReadKey();
        }
    }
} 