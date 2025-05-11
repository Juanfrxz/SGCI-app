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
    public IEpsRepository CrearEpsRepository()
    {
        return new ImpEpsRepository(_connectionString);
    }
    public IArlRepository CrearArlRepository()
    {
        return new ImpArlRepository(_connectionString);
    }
    public ISaleRepository CrearSaleRepository()
    {
        return new ImpSaleRepository(_connectionString);
    }

    public ISaleDetailRepository CrearSaleDetailRepository()
    {
        return new ImpSaleDetailRepository(_connectionString);
    }

    public IPurchaseRepository CrearPurchaseRepository()
    {
        return new ImpPurchaseRepository(_connectionString);
    }

    public IPurchaseDetailRepository CrearPurchaseDetailRepository()
    {
        return new ImpPurchaseDetailRepository(_connectionString);
    }
    public ICompanyRepository CrearCompanyRepository()
    {
        return new ImpCompanyRepository(_connectionString);
    }
    public IProductRepository CrearProductRepository()
    {
        return new ImpProductRepository(_connectionString);
    }
    public IPromotionalPlanProductRepository CrearPromoPlanProdRepository()
    {
        return new ImpPromotionalPlanProductRepository(_connectionString);
    }
    public IDocTypeRepository CrearDocTypeRepository()
    {
        return new ImpDocTypeRepository(_connectionString);
    }
    public IThirdPartyTypeRepository CrearThirdPartyTypeRepository()
    {
        return new ImpThirdPartyTypeRepopsitory (_connectionString);
    }
    public IPhoneTypeRepository CrearPhoneTypeRepository()
    {
        return new ImpPhoneTypeRepository (_connectionString);
    }
    public IProductSupplierRepository CrearProductSupplierRepository()
    {
        return new ImpProductSupplierRepository (_connectionString);
    }
    public ICashMovementTypeRepository CrearCashMovementTypeRepository()
    {
        return new ImpCashMovementTypeRepository (_connectionString);
    }
    public ICashSessionRepository CrearCashSessionRepository()
    {
        return new ImpCashSessionRepository(_connectionString);
    }
}
