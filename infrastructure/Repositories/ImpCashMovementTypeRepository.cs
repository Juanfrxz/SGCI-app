using System;
using System.Collections.Generic;
using Npgsql;
using SGCI_app.domain.Entities;
using SGCI_app.domain.Ports;
using SGCI_app.infrastructure.postgres;

namespace SGCI_app.infrastructure.Repositories
{
    public class ImpCashMovementTypeRepository : ICashMovementTypeRepository
    {
        private readonly ConexionSingleton _conexion;

        public ImpCashMovementTypeRepository(string connectionString)
        {
            _conexion = ConexionSingleton.Instancia(connectionString);
        }

        public void Crear(CashMovementType entity)
        {
            var conn = _conexion.ObtenerConexion();
            const string sql = @"
INSERT INTO tipo_mov_caja (nombre, tipo)
VALUES (@nombre, @tipo);
";
            using var cmd = new NpgsqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@nombre", entity.Nombre ?? string.Empty);
            cmd.Parameters.AddWithValue("@tipo", entity.Tipo ?? string.Empty);
            cmd.ExecuteNonQuery();
        }

        public void Actualizar(CashMovementType entity)
        {
            var conn = _conexion.ObtenerConexion();
            const string sql = @"
UPDATE tipo_mov_caja
SET nombre = COALESCE(@nombre, nombre),
    tipo   = COALESCE(@tipo, tipo)
WHERE id = @id;
";
            using var cmd = new NpgsqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@id", entity.Id);
            if (string.IsNullOrWhiteSpace(entity.Nombre))
                cmd.Parameters.AddWithValue("@nombre", DBNull.Value);
            else
                cmd.Parameters.AddWithValue("@nombre", entity.Nombre);

            if (string.IsNullOrWhiteSpace(entity.Tipo))
                cmd.Parameters.AddWithValue("@tipo", DBNull.Value);
            else
                cmd.Parameters.AddWithValue("@tipo", entity.Tipo);

            var rows = cmd.ExecuteNonQuery();
            if (rows == 0)
                throw new InvalidOperationException($"No se encontró tipo_mov_caja con id={entity.Id} para actualizar.");
        }

        public void Eliminar(int id)
        {
            var conn = _conexion.ObtenerConexion();
            const string sql = @"
DELETE FROM tipo_mov_caja
WHERE id = @id;
";
            using var cmd = new NpgsqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@id", id);
            var rows = cmd.ExecuteNonQuery();
            if (rows == 0)
                throw new InvalidOperationException($"No se encontró tipo_mov_caja con id={id} para eliminar.");
        }

        public List<CashMovementType> ObtenerTodos()
        {
            var list = new List<CashMovementType>();
            var conn = _conexion.ObtenerConexion();
            const string sql = @"
SELECT id, nombre, tipo
FROM tipo_mov_caja
ORDER BY id;
";
            using var cmd = new NpgsqlCommand(sql, conn);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new CashMovementType
                {
                    Id     = reader.GetInt32(reader.GetOrdinal("id")),
                    Nombre = reader.GetString(reader.GetOrdinal("nombre")),
                    Tipo   = reader.IsDBNull(reader.GetOrdinal("tipo"))
                                ? null
                                : reader.GetString(reader.GetOrdinal("tipo"))
                });
            }
            return list;
        }
    }
}
