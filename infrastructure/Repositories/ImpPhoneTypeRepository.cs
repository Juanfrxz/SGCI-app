using System;
using System.Collections.Generic;
using Npgsql;
using SGCI_app.domain.Entities;
using SGCI_app.domain.Ports;
using SGCI_app.infrastructure.postgres;

namespace SGCI_app.infrastructure.Repositories
{
    public class ImpPhoneTypeRepository : IPhoneTypeRepository
    {
        private readonly ConexionSingleton _conexion;

        public ImpPhoneTypeRepository(string connectionString)
        {
            _conexion = ConexionSingleton.Instancia(connectionString);
        }

        public void Crear(PhoneType entity)
        {
            var conn = _conexion.ObtenerConexion();
            const string sql = @"
INSERT INTO tipo_telefonos (description)
VALUES (@descripcion);
";
            using var cmd = new NpgsqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@descripcion", entity.Descripcion ?? string.Empty);
            cmd.ExecuteNonQuery();
        }

        public void Actualizar(PhoneType entity)
        {
            var conn = _conexion.ObtenerConexion();
            const string sql = @"
UPDATE tipo_telefonos
SET description = COALESCE(@descripcion, description)
WHERE id = @id;
";
            using var cmd = new NpgsqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@id", entity.Id);
            if (string.IsNullOrWhiteSpace(entity.Descripcion))
                cmd.Parameters.AddWithValue("@descripcion", DBNull.Value);
            else
                cmd.Parameters.AddWithValue("@descripcion", entity.Descripcion);

            var rows = cmd.ExecuteNonQuery();
            if (rows == 0)
                throw new InvalidOperationException($"No se encontró el tipo de teléfono con id={entity.Id} para actualizar.");
        }

        public void Eliminar(int id)
        {
            var conn = _conexion.ObtenerConexion();
            const string sql = @"
DELETE FROM tipo_telefonos
WHERE id = @id;
";
            using var cmd = new NpgsqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@id", id);
            var rows = cmd.ExecuteNonQuery();
            if (rows == 0)
                throw new InvalidOperationException($"No se encontró el tipo de teléfono con id={id} para eliminar.");
        }

        public List<PhoneType> ObtenerTodos()
        {
            var list = new List<PhoneType>();
            var conn = _conexion.ObtenerConexion();
            const string sql = @"
SELECT id, description
FROM tipo_telefonos
ORDER BY id ASC;
";
            using var cmd = new NpgsqlCommand(sql, conn);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new PhoneType
                {
                    Id = reader.GetInt32(reader.GetOrdinal("id")),
                    Descripcion = reader.GetString(reader.GetOrdinal("description"))
                });
            }
            return list;
        }
    }
}
