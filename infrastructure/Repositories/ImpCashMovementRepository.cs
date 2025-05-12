using System;
using System.Collections.Generic;
using System.Data;
using Npgsql;
using SGCI_app.domain.Entities;
using SGCI_app.domain.Ports;
using SGCI_app.infrastructure.postgres;

namespace SGCI_app.infrastructure.Repositories
{
    public class ImpCashMovementRepository : IGenericRepository<CashMovement>, ICashMovementRepository
    {
        private readonly string _connectionString;

        public ImpCashMovementRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void Crear(CashMovement entity)
        {
            using (var conn = new NpgsqlConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = @"
                        INSERT INTO mov_caja (fecha, tipo_mov_id, valor, concepto, tercero_id, sesion_id)
                        VALUES (@fecha, @tipoMovId, @valor, @concepto, @terceroId, @sesionId)";
                    
                    cmd.Parameters.AddWithValue("@fecha", entity.Fecha);
                    cmd.Parameters.AddWithValue("@tipoMovId", entity.TipoMovimiento_Id);
                    cmd.Parameters.AddWithValue("@valor", entity.Valor);
                    cmd.Parameters.AddWithValue("@concepto", entity.Concepto is null ? DBNull.Value : entity.Concepto);
                    cmd.Parameters.AddWithValue("@terceroId", entity.Tercero_Id is null ? DBNull.Value : entity.Tercero_Id);
                    cmd.Parameters.AddWithValue("@sesionId", entity.Sesion_Id);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Actualizar(CashMovement entity)
        {
            using (var conn = new NpgsqlConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = @"
                        UPDATE mov_caja 
                        SET fecha = @fecha,
                            tipo_mov_id = @tipoMovId,
                            valor = @valor,
                            concepto = @concepto,
                            tercero_id = @terceroId,
                            sesion_id = @sesionId
                        WHERE id = @id";
                    
                    cmd.Parameters.AddWithValue("@id", entity.Id);
                    cmd.Parameters.AddWithValue("@fecha", entity.Fecha);
                    cmd.Parameters.AddWithValue("@tipoMovId", entity.TipoMovimiento_Id);
                    cmd.Parameters.AddWithValue("@valor", entity.Valor);
                    cmd.Parameters.AddWithValue("@concepto", entity.Concepto is null ? DBNull.Value : entity.Concepto);
                    cmd.Parameters.AddWithValue("@terceroId", entity.Tercero_Id is null ? DBNull.Value : entity.Tercero_Id);
                    cmd.Parameters.AddWithValue("@sesionId", entity.Sesion_Id);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Eliminar(int id)
        {
            using (var conn = new NpgsqlConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "DELETE FROM mov_caja WHERE id = @id";
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public List<CashMovement> ObtenerTodos()
        {
            var movimientos = new List<CashMovement>();
            using (var conn = new NpgsqlConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "SELECT * FROM mov_caja ORDER BY id";

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            movimientos.Add(MapToEntity(reader));
                        }
                    }
                }
            }
            return movimientos;
        }

        public IEnumerable<CashMovement> ObtenerPorSesion(int sesionId)
        {
            var movimientos = new List<CashMovement>();
            using (var conn = new NpgsqlConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "SELECT * FROM mov_caja WHERE sesion_id = @sesionId";
                    cmd.Parameters.AddWithValue("@sesionId", sesionId);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            movimientos.Add(MapToEntity(reader));
                        }
                    }
                }
            }
            return movimientos;
        }

        public IEnumerable<CashMovement> ObtenerPorFecha(DateTime fecha)
        {
            var movimientos = new List<CashMovement>();
            using (var conn = new NpgsqlConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "SELECT * FROM mov_caja WHERE DATE(fecha) = DATE(@fecha)";
                    cmd.Parameters.AddWithValue("@fecha", fecha);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            movimientos.Add(MapToEntity(reader));
                        }
                    }
                }
            }
            return movimientos;
        }

        public IEnumerable<CashMovement> ObtenerPorTipo(int tipoId)
        {
            var movimientos = new List<CashMovement>();
            using (var conn = new NpgsqlConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "SELECT * FROM mov_caja WHERE tipo_mov_id = @tipoId";
                    cmd.Parameters.AddWithValue("@tipoId", tipoId);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            movimientos.Add(MapToEntity(reader));
                        }
                    }
                }
            }
            return movimientos;
        }

        public IEnumerable<CashMovement> ObtenerPorTercero(string terceroId)
        {
            var movimientos = new List<CashMovement>();
            using (var conn = new NpgsqlConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "SELECT * FROM mov_caja WHERE tercero_id = @terceroId";
                    cmd.Parameters.AddWithValue("@terceroId", terceroId);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            movimientos.Add(MapToEntity(reader));
                        }
                    }
                }
            }
            return movimientos;
        }

        private CashMovement MapToEntity(IDataReader reader)
        {
            return new CashMovement
            {
                Id = reader["id"] == DBNull.Value ? 0 : Convert.ToInt32(reader["id"]),
                Fecha = reader["fecha"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(reader["fecha"]),
                TipoMovimiento_Id = reader["tipo_mov_id"] == DBNull.Value ? 0 : Convert.ToInt32(reader["tipo_mov_id"]),
                Valor = reader["valor"] == DBNull.Value ? 0 : Convert.ToInt32(reader["valor"]),
                Concepto = reader["concepto"] == DBNull.Value ? null : Convert.ToString(reader["concepto"]),
                Tercero_Id = reader["tercero_id"] == DBNull.Value ? null : Convert.ToString(reader["tercero_id"]),
                Sesion_Id = reader["sesion_id"] == DBNull.Value ? 0 : Convert.ToInt32(reader["sesion_id"])
            };
        }
    }
}