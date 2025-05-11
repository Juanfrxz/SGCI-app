using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Npgsql;
using SGCI_app.domain.Entities;
using SGCI_app.domain.Ports;
using SGCI_app.infrastructure.postgres;

namespace SGCI_app.infrastructure.Repositories
{
    public class ImpCashMovementRepository : IGenericRepository<CashMovement>, ICashMovementRepository
    {
        private readonly ConexionSingleton _conexion;
        public ImpCashMovementRepository(string connectionString)
        {
            _conexion = ConexionSingleton.Instancia(connectionString);
        }

        public void Actualizar(CashMovement entity)
        {
            throw new NotImplementedException();
        }

        public void Crear(CashMovement entity)
        {
            throw new NotImplementedException();
        }

        public void Eliminar(int id)
        {
            throw new NotImplementedException();
        }

        public List<CashMovement> ObtenerTodos()
        {
            var list = new List<CashMovement>();
            var conn = _conexion.ObtenerConexion();
            const string sql = @"
SELECT id,
       fecha,
       tipo_mov_id,
       valor,
       concepto,
       tercero_id,
       sesion_id
FROM mov_caja
ORDER BY id;
";
            using var cmd = new NpgsqlCommand(sql, conn);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new CashMovement
                {
                    Id = reader.GetInt32(reader.GetOrdinal("id")),
                    Fecha = reader.GetDateTime(reader.GetOrdinal("fecha")),
                    TipoMovimiento_Id = reader.GetInt32(reader.GetOrdinal("tipo_mov_id")),
                    Valor = (int)reader.GetDouble(reader.GetOrdinal("valor")),
                    Concepto = reader.IsDBNull(reader.GetOrdinal("concepto")) ? null : reader.GetString(reader.GetOrdinal("concepto")),
                    Tercero_Id = reader.IsDBNull(reader.GetOrdinal("tercero_id")) ? null : reader.GetString(reader.GetOrdinal("tercero_id")),
                    Sesion_Id = reader.GetInt32(reader.GetOrdinal("sesion_id"))
                });
            }
            return list;
        }
    }
}