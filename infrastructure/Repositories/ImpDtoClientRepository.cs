using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Npgsql;
using SGCI_app.domain.DTO;
using SGCI_app.domain.Ports;
using SGCI_app.infrastructure.postgres;

namespace SGCI_app.infrastructure.Repositories
{
    public class ImpDtoClientRepository : IGenericRepository<DtoClient>, IDtoClientRepository
    {
        private readonly ConexionSingleton _conexion;
        public ImpDtoClientRepository(string connectionString)
        {
            _conexion = ConexionSingleton.Instancia(connectionString);
        }
        public void Actualizar(DtoClient entity)
        {
            throw new NotImplementedException();
        }

        public void Crear(DtoClient entity)
        {
            throw new NotImplementedException();
        }

        public void Eliminar(int id)
        {
            throw new NotImplementedException();
        }

        public List<DtoClient> ObtenerTodos()
        {
            var clientes = new List<DtoClient>();

            // Aquí obtienes la conexión abierta de tu singleton…
            var connection = _conexion.ObtenerConexion();

            const string query = "SELECT id, nombre FROM clientes;";

            // Usamos using sólo en el comando y el reader, no en la conexión singleton
            using var cmd = new NpgsqlCommand(query, connection);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                clientes.Add(new DtoClient
                {
                    Id = reader.GetInt32(reader.GetOrdinal("id")),
                    Nombre = reader.GetString(reader.GetOrdinal("nombre"))
                });
            }

            return clientes;
        }
    }
}