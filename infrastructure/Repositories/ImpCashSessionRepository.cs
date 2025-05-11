using System;
using System.Collections.Generic;
using Npgsql;
using SGCI_app.domain.Entities;
using SGCI_app.domain.Ports;
using SGCI_app.infrastructure.postgres;

namespace SGCI_app.infrastructure.Repositories
{
    public class ImpCashSessionRepository : IGenericRepository<CashSession>, ICashSessionRepository
    {
        private readonly ConexionSingleton _conexion;

        public ImpCashSessionRepository(string connectionString)
        {
            _conexion = ConexionSingleton.Instancia(connectionString);
        }

        public void Crear(CashSession entity)
        {
            var conn = _conexion.ObtenerConexion();
            const string sql = @"
INSERT INTO sesion_caja (balance_apertura, balance_cierre)
VALUES (@balance_apertura, @balance_cierre);
";
            using var cmd = new NpgsqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@balance_apertura", entity.BalanceApertura);
            cmd.Parameters.AddWithValue("@balance_cierre", DBNull.Value);
            cmd.ExecuteNonQuery();
        }

        public void Actualizar(CashSession entity)
        {
            var conn = _conexion.ObtenerConexion();
            const string sql = @"
UPDATE sesion_caja
SET cerrado          = COALESCE(@cerrado, cerrado),
    balance_cierre    = COALESCE(@balance_cierre, balance_cierre)
WHERE id = @id;
";
            using var cmd = new NpgsqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@id", entity.Id);
            cmd.Parameters.AddWithValue("@cerrado", entity.CierreCaja.HasValue ? (object)entity.CierreCaja.Value : DBNull.Value);
            cmd.Parameters.AddWithValue("@balance_cierre", (object?)entity.BalaceCierre ?? DBNull.Value);
            var rows = cmd.ExecuteNonQuery();
            if (rows == 0)
                throw new InvalidOperationException($"No se encontró la sesión con id={entity.Id} para actualizar.");
        }

        public void Eliminar(int id)
        {
            var conn = _conexion.ObtenerConexion();
            const string sql = @"
DELETE FROM sesion_caja
WHERE id = @id;
";
            using var cmd = new NpgsqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@id", id);
            var rows = cmd.ExecuteNonQuery();
            if (rows == 0)
                throw new InvalidOperationException($"No se encontró la sesión con id={id} para eliminar.");
        }

        public List<CashSession> ObtenerTodos()
        {
            var list = new List<CashSession>();
            var conn = _conexion.ObtenerConexion();
            const string sql = @"
SELECT id,
       abierto,
       cerrado,
       balance_apertura,
       balance_cierre
FROM sesion_caja
ORDER BY id;
";
            using var cmd = new NpgsqlCommand(sql, conn);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new CashSession
                {
                    Id = reader.GetInt32(reader.GetOrdinal("id")),
                    AperturaCaja = reader.GetDateTime(reader.GetOrdinal("abierto")),
                    CierreCaja = reader.IsDBNull(reader.GetOrdinal("cerrado")) ? null : reader.GetDateTime(reader.GetOrdinal("cerrado")),
                    BalanceApertura = reader.GetInt32(reader.GetOrdinal("balance_apertura")),
                    BalaceCierre = reader.IsDBNull(reader.GetOrdinal("balance_cierre"))
                                          ? 0
                                          : (int)reader.GetDecimal(reader.GetOrdinal("balance_cierre"))
                });
            }
            return list;
        }

        public void Cerrar(int id)
        {
            // Obtenemos la conexión (singleton)
            var conn = _conexion.ObtenerConexion();

            // Sentencia UPDATE para marcar el cierre de la sesión con la fecha actual
            const string sql = @"
UPDATE sesion_caja
SET cerrado = NOW()
WHERE id = @id;
";

            using var cmd = new NpgsqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@id", id);

            // Ejecutamos y comprobamos que efectivamente se cerró una sesión
            var rows = cmd.ExecuteNonQuery();
            if (rows == 0)
            {
                throw new InvalidOperationException($"No se encontró la sesión con id={id} para cerrar.");
            }
        }

    }
}
