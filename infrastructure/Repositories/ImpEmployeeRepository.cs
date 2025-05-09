using SGCI_app.domain.DTO;
using SGCI_app.domain.Ports;
using SGCI_app.infrastructure.postgres;

namespace SGCI_app.infrastructure.Repositories
{
    public class ImpEmployeeRepository : IGenericRepository<DtoEmployee>, IEmployeeRepository
    {
        private readonly ConexionSingleton _conexion;
        public ImpEmployeeRepository(string connectionString)
        {
            _conexion = ConexionSingleton.Instancia(connectionString);
        }
        // Implementation of the repository methods goes here
        public void Actualizar(int id, DtoEmployee entity)
        {
            throw new NotImplementedException();
        }

        public void Actualizar(DtoEmployee entity)
        {
            throw new NotImplementedException();
        }

        public void Crear(DtoEmployee entity)
        {
            throw new NotImplementedException();
        }

        public void Eliminar(int id)
        {
            throw new NotImplementedException();
        }

        public List<DtoEmployee> ObtenerTodos()
        {
            throw new NotImplementedException();
        }
    }
}