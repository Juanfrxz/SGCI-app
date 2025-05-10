using System;
using System.Collections.Generic;
using SGCI_app.domain.Entities;
using SGCI_app.domain.Ports;
using SGCI_app.infrastructure.postgres;
using Npgsql;
using SGCI_app.domain.DTO;

namespace SGCI_app.infrastructure.Repositories
{
    public class ImpCompanyRepository : IGenericRepository<DtoCompany>, ICompanyRepository
    {
        private readonly ConexionSingleton _conexion;

        public ImpCompanyRepository(string connectionString)
        {
            _conexion = ConexionSingleton.Instancia(connectionString);
        }

        public void Actualizar(DtoCompany entity)
        {
            var connection = _conexion.ObtenerConexion();
            const string sql = @"
            CALL public.sp_update_company(
                p_company_id      => @p_company_id,
                p_nombre          => @p_nombre,
                p_calle           => @p_calle,
                p_numero_edificio => @p_numero_edificio,
                p_codigo_postal   => @p_codigo_postal,
                p_ciudad_id       => @p_ciudad_id,
                p_info_adicional  => @p_info_adicional,
                p_fecha_registro  => @p_fecha_registro
            );";

            using var cmd = new NpgsqlCommand(sql, connection)
            {
                CommandType = System.Data.CommandType.Text
            };

            cmd.Parameters.AddWithValue("p_company_id", entity.Id ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("p_nombre", entity.Nombre ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("p_calle", entity.Address.Calle ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("p_numero_edificio", entity.Address.NumeroEdificio ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("p_codigo_postal", entity.Address.CodigoPostal ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("p_ciudad_id", entity.Address.Ciudad_Id);
            cmd.Parameters.AddWithValue("p_info_adicional", entity.Address.InfoAdicional ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("p_fecha_registro", entity.FechaRegistro ?? (object)DBNull.Value);

            var rows = cmd.ExecuteNonQuery();
            if (rows == 0)
                throw new InvalidOperationException($"No se encontró empresa con id = {entity.Id} para actualizar.");
        }


        public void Crear(DtoCompany entity)
        {
            var connection = _conexion.ObtenerConexion();
            const string sql = @"
            CALL public.sp_create_company(
                p_id              := @p_id,
                p_nombre          := @p_nombre,
                p_calle           := @p_calle,
                p_numero_edificio := @p_numero_edificio,
                p_codigo_postal   := @p_codigo_postal,
                p_ciudad_id       := @p_ciudad_id,
                p_info_adicional  := @p_info_adicional,
                p_fecha_registro  := @p_fecha_registro
            );";

            using var cmd = new NpgsqlCommand(sql, connection)
            {
                CommandType = System.Data.CommandType.Text
            };

            cmd.Parameters.AddWithValue("p_id", entity.Id ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("p_nombre", entity.Nombre ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("p_calle", entity.Address.Calle ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("p_numero_edificio", entity.Address.NumeroEdificio ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("p_codigo_postal", entity.Address.CodigoPostal ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("p_ciudad_id", entity.Address.Ciudad_Id);
            cmd.Parameters.AddWithValue("p_info_adicional", entity.Address.InfoAdicional ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("p_fecha_registro", entity.FechaRegistro ?? (object)DBNull.Value);

            cmd.ExecuteNonQuery();
        }

        public void Eliminar(string id)
        {
            var connection = _conexion.ObtenerConexion();
            const string sql = "DELETE FROM empresa WHERE id = @id;";

            using var cmd = new NpgsqlCommand(sql, connection)
            {
                CommandType = System.Data.CommandType.Text
            };
            cmd.Parameters.AddWithValue("id", id);

            var rows = cmd.ExecuteNonQuery();
            if (rows == 0)
                throw new InvalidOperationException($"No se encontró empresa con id = {id} para eliminar.");
        }

        public void Eliminar(int id)
        {
            throw new NotImplementedException();
        }

        public List<DtoCompany> ObtenerTodos()
        {
            var connection = _conexion.ObtenerConexion();
            const string sql = @"
            SELECT 
                e.id,
                e.nombre,
                e.fecha_reg
            FROM empresa e;";

            using var cmd = new NpgsqlCommand(sql, connection);
            using var reader = cmd.ExecuteReader();

            var lista = new List<DtoCompany>();
            while (reader.Read())
            {
                var dto = new DtoCompany
                {
                    Id = reader.GetString(reader.GetOrdinal("id")),
                    Nombre = reader.GetString(reader.GetOrdinal("nombre")),
                    FechaRegistro = reader.IsDBNull(reader.GetOrdinal("fecha_reg"))
                                        ? (DateTime?)null
                                        : reader.GetDateTime(reader.GetOrdinal("fecha_reg"))
                };
                lista.Add(dto);
            }

            return lista;
        }

    }
}
