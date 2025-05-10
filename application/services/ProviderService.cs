using SGCI_app.domain.DTO;
using SGCI_app.domain.Ports;
using System.Collections.Generic;

namespace SGCI_app.application.services
{
    public class ProviderService
    {
        private readonly IProviderRepository _providerRepository;

        public ProviderService(IProviderRepository providerRepository)
        {
            _providerRepository = providerRepository;
        }

        public void CreateProvider(DtoProvider provider)
        {
            _providerRepository.Crear(provider);
        }

        public void UpdateProvider(int id, DtoProvider provider)
        {
            _providerRepository.Actualizar(id, provider);
        }

        public void DeleteProvider(int id)
        {
            _providerRepository.Eliminar(id);
        }

        public List<DtoProvider> GetAllProviders()
        {
            return _providerRepository.ObtenerTodos();
        }
    }
} 