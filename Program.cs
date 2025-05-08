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
        var servicioDtoCliente = new DtoClientService(factory.CrearDtoClienteRepository());
        Console.WriteLine("Hello, World!");
    }
}