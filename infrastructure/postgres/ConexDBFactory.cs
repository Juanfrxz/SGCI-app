using SGCI_app.domain.Factory;
using SGCI_app.domain.Ports;
using SGCI_app.infrastructure.Repositories;

namespace SGCI_app.infrastructure.postgres;

public class ConexDBFactory : IDbfactory
{
    private readonly string _connectionString;

    public ConexDBFactory(string connectionString)
    {
        _connectionString = connectionString;
    }
    public IDtoClientRepository CrearDtoClienteRepository()
    {
        return new ImpDtoClientRepository(_connectionString);
    }
}
