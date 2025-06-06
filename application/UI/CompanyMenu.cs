using System;
using SGCI_app.domain.DTO;
using SGCI_app.domain.Factory;
using SGCI_app.infrastructure.postgres;
using SGCI_app.application.services;

namespace SGCI_app.application.UI
{
    public class CompanyMenu : BaseMenu
    {
        private readonly CompanyService _service;

        public CompanyMenu() : base(showIntro: false)
        {
            string connStr = "Host=localhost;database=sgci;Port=5432;Username=postgres;Password=1219;Pooling=true";
            var factory = new ConexDBFactory(connStr);
            _service = new CompanyService(factory.CrearCompanyRepository());
        }

        public override void ShowMenu()
        {
            while (true)
            {
                ShowHeader("GESTIÓN DE EMPRESAS");
                Console.WriteLine("1. Crear Empresa");
                Console.WriteLine("2. Listar Empresas");
                Console.WriteLine("3. Actualizar Empresa");
                Console.WriteLine("4. Eliminar Empresa");
                Console.WriteLine("0. Volver al menú principal");
                DrawSeparator();

                int option = GetValidatedIntInput("Seleccione una opción: ", 0, 4);
                switch (option)
                {
                    case 1:
                        CrearCompany();
                        break;
                    case 2:
                        ListarCompanies();
                        break;
                    case 3:
                        ActualizarCompany();
                        break;
                    case 4:
                        EliminarCompany();
                        break;
                    case 0:
                        return;
                }
            }
        }

        private void CrearCompany()
        {
            ShowHeader("CREAR NUEVA EMPRESA");
            
            Console.Write("ID (opcional, presione Enter para omitir): ");
            string? id = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(id)) id = null;
            
            Console.Write("Nombre (opcional, presione Enter para omitir): ");
            string? nombre = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(nombre)) nombre = null;
            
            Console.WriteLine("\nDatos de Dirección:");
            Console.Write("Calle (opcional, presione Enter para omitir): ");
            string? calle = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(calle)) calle = null;
            
            Console.Write("Número de Edificio (opcional, presione Enter para omitir): ");
            string? numeroEdificio = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(numeroEdificio)) numeroEdificio = null;
            
            Console.Write("Código Postal (opcional, presione Enter para omitir): ");
            string? codigoPostal = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(codigoPostal)) codigoPostal = null;
            
            Console.Write("ID de Ciudad: ");
            if (!int.TryParse(Console.ReadLine(), out int ciudadId))
            {
                ShowErrorMessage("ID de ciudad inválido.");
                return;
            }
            
            Console.Write("Información Adicional (opcional, presione Enter para omitir): ");
            string? infoAdicional = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(infoAdicional)) infoAdicional = null;
            
            Console.Write("Fecha de Registro (YYYY-MM-DD, opcional, presione Enter para omitir): ");
            string? fechaStr = Console.ReadLine();
            DateTime? fechaRegistro = null;
            if (!string.IsNullOrWhiteSpace(fechaStr))
            {
                if (!DateTime.TryParse(fechaStr, out DateTime fecha))
                {
                    ShowErrorMessage("Fecha inválida.");
                    return;
                }
                fechaRegistro = fecha;
            }

            var company = new DtoCompany 
            { 
                Id = id,
                Nombre = nombre,
                FechaRegistro = fechaRegistro,
                Address = new DtoAddress
                {
                    Calle = calle,
                    NumeroEdificio = numeroEdificio,
                    CodigoPostal = codigoPostal,
                    Ciudad_Id = ciudadId,
                    InfoAdicional = infoAdicional
                }
            };
            
            try
            {
                _service.CrearCompany(company);
                ShowSuccessMessage("Empresa creada exitosamente.");
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Error al crear la empresa: {ex.Message}");
            }
        }

        private void ListarCompanies()
        {
            ShowHeader("LISTA DE EMPRESAS");
            try
            {
                _service.MostrarTodos();
                ShowInfoMessage("Listado de empresas completado.");
                Console.WriteLine();
                Console.Write("Presione cualquier tecla para continuar...");
                Console.ReadKey(true);
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Error al listar las empresas: {ex.Message}");
            }
        }

        private void ActualizarCompany()
        {
            ShowHeader("ACTUALIZAR EMPRESA");
            
            Console.Write("ID de la empresa a actualizar: ");
            string? id = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(id))
            {
                ShowErrorMessage("El ID es requerido.");
                return;
            }
            
            Console.Write("Nuevo nombre (opcional, presione Enter para mantener el actual): ");
            string? nuevoNombre = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(nuevoNombre)) nuevoNombre = null;
            
            Console.WriteLine("\nNuevos datos de Dirección:");
            Console.Write("Nueva calle (opcional, presione Enter para mantener la actual): ");
            string? nuevaCalle = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(nuevaCalle)) nuevaCalle = null;
            
            Console.Write("Nuevo número de edificio (opcional, presione Enter para mantener el actual): ");
            string? nuevoNumeroEdificio = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(nuevoNumeroEdificio)) nuevoNumeroEdificio = null;
            
            Console.Write("Nuevo código postal (opcional, presione Enter para mantener el actual): ");
            string? nuevoCodigoPostal = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(nuevoCodigoPostal)) nuevoCodigoPostal = null;
            
            Console.Write("Nuevo ID de ciudad: ");
            if (!int.TryParse(Console.ReadLine(), out int nuevaCiudadId))
            {
                ShowErrorMessage("ID de ciudad inválido.");
                return;
            }
            
            Console.Write("Nueva información adicional (opcional, presione Enter para mantener la actual): ");
            string? nuevaInfoAdicional = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(nuevaInfoAdicional)) nuevaInfoAdicional = null;
            
            Console.Write("Nueva fecha de registro (YYYY-MM-DD, opcional, presione Enter para mantener la actual): ");
            string? fechaStr = Console.ReadLine();
            DateTime? nuevaFechaRegistro = null;
            if (!string.IsNullOrWhiteSpace(fechaStr))
            {
                if (!DateTime.TryParse(fechaStr, out DateTime fecha))
                {
                    ShowErrorMessage("Fecha inválida.");
                    return;
                }
                nuevaFechaRegistro = fecha;
            }

            var company = new DtoCompany 
            { 
                Id = id,
                Nombre = nuevoNombre,
                FechaRegistro = nuevaFechaRegistro,
                Address = new DtoAddress
                {
                    Calle = nuevaCalle,
                    NumeroEdificio = nuevoNumeroEdificio,
                    CodigoPostal = nuevoCodigoPostal,
                    Ciudad_Id = nuevaCiudadId,
                    InfoAdicional = nuevaInfoAdicional
                }
            };
            
            try
            {
                _service.ActualizarCompany(company);
                ShowSuccessMessage("Empresa actualizada exitosamente.");
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Error al actualizar la empresa: {ex.Message}");
            }
        }

        private void EliminarCompany()
        {
            ShowHeader("ELIMINAR EMPRESA");
            
            Console.Write("ID de la empresa a eliminar: ");
            string? id = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(id))
            {
                ShowErrorMessage("El ID es requerido.");
                return;
            }
            
            try
            {
                _service.EliminarCompany(id);
                ShowSuccessMessage("Empresa eliminada exitosamente.");
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Error al eliminar la empresa: {ex.Message}");
            }
        }
    }
}