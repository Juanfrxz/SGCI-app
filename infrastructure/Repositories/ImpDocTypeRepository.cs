using System;
using System.Collections.Generic;
using Npgsql;
using SGCI_app.domain.Entities;
using SGCI_app.domain.Ports;
using SGCI_app.infrastructure.postgres;

namespace SGCI_app.infrastructure.Repositories
{
    public class ImpDocTypeRepository : IDocTypeRepository
    {
        private readonly ConexionSingleton _conexion;

        public ImpDocTypeRepository(string connectionString)
        {
            _conexion = ConexionSingleton.Instancia(connectionString);
        }

        public void Crear(DocType entity)
        {
            var conn = _conexion.ObtenerConexion();
            const string sql = @"
INSERT INTO tipo_documentos (descripcion)
VALUES (@descripcion);
";
            using var cmd = new NpgsqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@descripcion", entity.Descripcion ?? string.Empty);
            cmd.ExecuteNonQuery();
        }

        public void Actualizar(int id, DocType entity)
        {
            var conn = _conexion.ObtenerConexion();
            const string sql = @"
UPDATE tipo_documentos
SET descripcion = COALESCE(@descripcion, descripcion)
WHERE id = @id;
";
            using var cmd = new NpgsqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@id", id);
            // If descripcion is null or whitespace, pass DBNull to keep old
            if (string.IsNullOrWhiteSpace(entity.Descripcion))
                cmd.Parameters.AddWithValue("@descripcion", DBNull.Value);
            else
                cmd.Parameters.AddWithValue("@descripcion", entity.Descripcion);
            var rows = cmd.ExecuteNonQuery();
            if (rows == 0)
                throw new InvalidOperationException($"No se encontró el tipo de documento con id={id} para actualizar.");
        }

        public void Actualizar(DocType entity)
        {
            Actualizar(entity.Id, entity);
        }

        public void Eliminar(int id)
        {
            var conn = _conexion.ObtenerConexion();
            const string sql = @"
DELETE FROM tipo_documentos
WHERE id = @id;
";
            using var cmd = new NpgsqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@id", id);
            var rows = cmd.ExecuteNonQuery();
            if (rows == 0)
                throw new InvalidOperationException($"No se encontró el tipo de documento con id={id} para eliminar.");
        }

        public List<DocType> ObtenerTodos()
        {
            var list = new List<DocType>();
            var conn = _conexion.ObtenerConexion();
            const string sql = @"
SELECT id, descripcion
FROM tipo_documentos
ORDER BY id ASC;
";
            using var cmd = new NpgsqlCommand(sql, conn);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new DocType
                {
                    Id = reader.GetInt32(reader.GetOrdinal("id")),
                    Descripcion = reader.GetString(reader.GetOrdinal("descripcion"))
                });
            }
            return list;
        }
    }
}
