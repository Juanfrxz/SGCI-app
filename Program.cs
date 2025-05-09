using SGCI_app.application.services;
using SGCI_app.domain.DTO;
using SGCI_app.domain.Factory;
using SGCI_app.infrastructure.postgres;

internal class Program
{
    private static void Main(string[] args)
    {
        string connStr = "Host=localhost;database=sgci;Port=5432;Username=postgres;Password=campus2023;Pooling=true";
        IDbfactory factory = new ConexDBFactory(connStr);
        var servicioDtoEmployee = new DtoEmployeeService(factory.CrearDtoEmployeeRepository());
        Console.WriteLine("Hello, World!");
        var testClient = new DtoEmployee
        {
            Nombre            = "Juan",
            Apellidos         = "Pérez",
            Email             = "juan.perez@example.com",
            TipoDoc_id        = 1,       // asegúrate de que existan estos fk en tu BD
            TipoTercero_id    = 1,
            Address = new DtoAddress
            {
                Calle               = "Calle Falsa",
                NumeroEdificio      = "123",
                CodigoPostal        = "110111",
                Ciudad_Id           = 1,  // id de ciudad válido
                InfoAdicional       = "Piso 4, apto 402"
            },
            Employee = new DtoEmp
            {
                FechaIngreso     = new DateTime(1985, 7, 20),
                SalarioBase   = 1500.55,
                Eps_id = 1,
                Arl_id = 1
            }
        };

        try
        {
            // 4) Ejecutar la inserción
            servicioDtoEmployee.CrearEmpleado(testClient);
            Console.WriteLine("✅ Cliente creado exitosamente.");
        }
        catch (Exception ex)
        {
            Console.WriteLine("❌ Error al crear cliente: " + ex.Message);
        }
        servicioDtoEmployee.MostrarTodos();
    }
}