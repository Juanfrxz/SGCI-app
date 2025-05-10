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
}
