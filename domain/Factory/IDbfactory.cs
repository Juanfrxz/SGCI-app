using SGCI_app.domain.Ports;

namespace SGCI_app.domain.Factory;

public interface IDbfactory
{
    IDtoClientRepository CrearDtoClienteRepository();
    IRegionRepository CrearRegionRepository();
    ICountryRepository CrearCountryRepository();
    ICityRepository CrearCityRepository();
    
}
