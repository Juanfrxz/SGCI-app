using SGCI_app.domain.Ports;

namespace SGCI_app.domain.Factory;

public interface IDbfactory
{
    IRegionRepository CrearRegionRepository();
    ICountryRepository CrearCountryRepository();
    ICityRepository CrearCityRepository();
    IClientRepository CrearClientRepository();
    IEmployeeRepository CrearEmployeeRepository();
    IProviderRepository CrearProviderRepository();
    IPromotionalPlanRepository CrearPromoPlanRepository();
    IEpsRepository CrearEpsRepository();
    IArlRepository CrearArlRepository();
    ISaleRepository CrearSaleRepository();
    ISaleDetailRepository CrearSaleDetailRepository();
    IPurchaseRepository CrearPurchaseRepository();
    IPurchaseDetailRepository CrearPurchaseDetailRepository();
    ICompanyRepository CrearCompanyRepository();
    IProductRepository CrearProductRepository();

}
