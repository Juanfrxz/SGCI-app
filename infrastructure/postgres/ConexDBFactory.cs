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
        public IClientRepository CrearClientRepository()
    {
        return new ImpClientRepository(_connectionString);
    }
    public IEmployeeRepository CrearEmployeeRepository()
    {
        return new ImpEmployeeRepository(_connectionString);
    }
    public IProviderRepository CrearProviderRepository()
    {
        return new ImpProviderRepository(_connectionString);
    }
    public IPromotionalPlanRepository CrearPromoPlanRepository()
    {
        return new ImpPromotionalPlanRepository(_connectionString);
    }
}
