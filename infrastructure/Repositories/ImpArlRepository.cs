using System;
using System.Collections.Generic;
using SGCI_app.domain.Entities;
using SGCI_app.domain.Ports;
using SGCI_app.infrastructure.postgres;
using Npgsql;

namespace SGCI_app.infrastructure.Repositories
{
    public class ImpArlRepository : IGenericRepository<ARL>, IArlRepository
    {
        private readonly ConexionSingleton _conexion;

        public ImpArlRepository(string connectionString)
        {
            _conexion = ConexionSingleton.Instancia(connectionString);
        }

        public List<ARL> ObtenerTodos()
        {
            var arls = new List<ARL>();
            var connection = _conexion.ObtenerConexion();

            string query = "SELECT id, nombre FROM arl";
            using var cmd = new NpgsqlCommand(query, connection);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                arls.Add(new ARL
                {
                    Id     = reader.GetInt32(reader.GetOrdinal("id")),
                    Nombre = reader.IsDBNull(reader.GetOrdinal("nombre"))
                                ? null
                                : reader.GetString(reader.GetOrdinal("nombre"))
                });
            }

            return arls;
        }

        public void Crear(ARL entity)
        {
            var connection = _conexion.ObtenerConexion();
            string query = "INSERT INTO arl (nombre) VALUES (@nombre)";

            using var cmd = new NpgsqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@nombre", entity.Nombre ?? (object)DBNull.Value);
            cmd.ExecuteNonQuery();
        }

        public void Actualizar(ARL entity)
        {
            var connection = _conexion.ObtenerConexion();
            string query = "UPDATE arl SET nombre = @nombre WHERE id = @id";

            using var cmd = new NpgsqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@id", entity.Id);
            cmd.Parameters.AddWithValue("@nombre", entity.Nombre ?? (object)DBNull.Value);
            cmd.ExecuteNonQuery();
        }

        public void Actualizar(int id, ARL entity)
        {
            var connection = _conexion.ObtenerConexion();
            string query = "UPDATE arl SET nombre = @nombre WHERE id = @id";

            using var cmd = new NpgsqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@id", id);
            cmd.Parameters.AddWithValue("@nombre", entity.Nombre ?? (object)DBNull.Value);
            cmd.ExecuteNonQuery();
        }

        public void Eliminar(int id)
        {
            var connection = _conexion.ObtenerConexion();
            string query = "DELETE FROM arl WHERE id = @id";

            using var cmd = new NpgsqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@id", id);
            cmd.ExecuteNonQuery();
        }
    }
}
