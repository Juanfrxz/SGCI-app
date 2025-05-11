using System;
using System.Collections.Generic;
using SGCI_app.domain.Entities;
using SGCI_app.domain.Ports;
using SGCI_app.infrastructure.postgres;
using Npgsql;

namespace SGCI_app.infrastructure.Repositories
{
    public class ImpEpsRepository : IGenericRepository<EPS>, IEpsRepository
    {
        private readonly ConexionSingleton _conexion;

        public ImpEpsRepository(string connectionString)
        {
            _conexion = ConexionSingleton.Instancia(connectionString);
        }

        // Crear nueva EPS mediante INSERT
        public void Crear(EPS entity)
        {
            const string sql = @"INSERT INTO eps (nombre)
                                 VALUES (@nombre)
                                 RETURNING id;";

            var connection = _conexion.ObtenerConexion();
            using var cmd = new NpgsqlCommand(sql, connection);

            cmd.Parameters.AddWithValue("nombre", entity.Nombre ?? (object)DBNull.Value);

            // Ejecutar y capturar el ID generado
            entity.Id = Convert.ToInt32(cmd.ExecuteScalar());
        }

        // Obtener todas las EPS
        public List<EPS> ObtenerTodos()
        {
            const string sql = @"SELECT id, nombre FROM eps ORDER BY id ASC;";
            var results = new List<EPS>();

            var connection = _conexion.ObtenerConexion();
            using var cmd = new NpgsqlCommand(sql, connection);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                results.Add(new EPS
                {
                    Id = reader.GetInt32(reader.GetOrdinal("id")),
                    Nombre = reader.IsDBNull(reader.GetOrdinal("nombre"))
                             ? null
                             : reader.GetString(reader.GetOrdinal("nombre"))
                });
            }

            return results;
        }

        // Actualizar EPS existente mediante UPDATE
        public void Actualizar(int id, EPS entity)
        {
            const string sql = @"UPDATE eps
                                 SET nombre = @nombre
                                 WHERE id = @id;";

            var connection = _conexion.ObtenerConexion();
            using var cmd = new NpgsqlCommand(sql, connection);

            cmd.Parameters.AddWithValue("nombre", entity.Nombre ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("id", id);

            var rows = cmd.ExecuteNonQuery();
            if (rows == 0)
                throw new InvalidOperationException($"No se encontró EPS con id = {id} para actualizar.");
        }

        // Eliminar EPS por ID
        public void Eliminar(int id)
        {
            const string sql = @"DELETE FROM eps WHERE id = @id;";
            var connection = _conexion.ObtenerConexion();
            using var cmd = new NpgsqlCommand(sql, connection);

            cmd.Parameters.AddWithValue("id", id);
            var rows = cmd.ExecuteNonQuery();
            if (rows == 0)
                throw new InvalidOperationException($"No se encontró EPS con id = {id} para eliminar.");
        }

        public void Actualizar(EPS entity)
        {
            throw new NotImplementedException();
        }
    }
}
