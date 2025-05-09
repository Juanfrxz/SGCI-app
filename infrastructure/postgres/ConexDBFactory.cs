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
    public IRegionRepository CrearRegionRepository()
    {
        return new ImpRegionRepository(_connectionString);
    }

    public ICountryRepository CrearCountryRepository()
    {
        return new ImpCountryRepository(_connectionString);
    }

    public ICityRepository CrearCityRepository()
    {
        return new ImpCityRepository(_connectionString);
    }
    public IDtoEmployeeRepository CrearDtoEmployeeRepository()
    {
        return new ImpDtoEmployeeRepository(_connectionString);
    }
        public IClientRepository CrearClientRepository()
    {
        return new ImpClientRepository(_connectionString);
    }
    public IEmployeeRepository CrearEmployeeRepository()
    {
        return new ImpEmployeeRepository(_connectionString);
    }
}
