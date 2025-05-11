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
    public class ImpClientRepository : IGenericRepository<DtoClient>, IClientRepository
    {
        private readonly ConexionSingleton _conexion;
        public ImpClientRepository(string connectionString)
        {
            _conexion = ConexionSingleton.Instancia(connectionString);
        }

        public void Actualizar(int id, DtoClient entity)
        {
            var connection = _conexion.ObtenerConexion();
            const string sql = @"
                                UPDATE terceros t
                                SET
                                    nombre              = COALESCE(NULLIF(@nombre, ''), t.nombre),
                                    apellidos           = COALESCE(NULLIF(@apellidos, ''), t.apellidos),
                                    email               = COALESCE(NULLIF(@email, ''), t.email),
                                    tipo_terceros_id    = COALESCE(@tipo_tercero_id::int, t.tipo_terceros_id),
                                    tipo_documento_id   = COALESCE(@tipo_documento_id::int, t.tipo_documento_id)
                                FROM cliente c
                                WHERE c.id = @cliente_id
                                AND t.id = c.tercero_id;
                            ";

            using var cmd = new NpgsqlCommand(sql, connection)
            {
                CommandType = System.Data.CommandType.Text
            };

            // 3) Parámetros
            cmd.Parameters.AddWithValue("cliente_id", id);
            cmd.Parameters.AddWithValue("nombre", entity.Nombre ?? (object) DBNull.Value);
            cmd.Parameters.AddWithValue("apellidos", entity.Apellidos ?? (object) DBNull.Value);
            cmd.Parameters.AddWithValue("email", entity.Email ?? (object) DBNull.Value);
            cmd.Parameters.AddWithValue("tipo_tercero_id", entity.TipoTercero_id);
            cmd.Parameters.AddWithValue("tipo_documento_id", entity.TipoDoc_id);

            // 4) Ejecutar
            var rows = cmd.ExecuteNonQuery();
            if (rows == 0)
            {
                throw new InvalidOperationException($"No se encontró cliente con id = {id} para actualizar.");
            } else {
                Console.WriteLine("Datos personales actualizados exitosamente!!!");
            }

        }

        public void Actualizar(DtoClient entity)
        {
            throw new NotImplementedException();
        }

        public void Crear(DtoClient entity)
        {
            // Obtener la conexión
            var connection = _conexion.ObtenerConexion();

            var sql = @"CALL public.sp_create_client(
                        p_calle             := @p_calle,
                        p_numero_edificio   := @p_numero_edificio,
                        p_codigo_postal     := @p_codigo_postal,
                        p_ciudad_id         := @p_ciudad_id,
                        p_info_adicional    := @p_info_adicional,
                        p_nombre            := @p_nombre,
                        p_apellidos         := @p_apellidos,
                        p_email             := @p_email,
                        p_tipo_tercero_id   := @p_tipo_tercero_id,
                        p_tipo_documento_id := @p_tipo_documento_id,
                        p_fecha_nac         := @p_fecha_nac,
                        p_fecha_ult_compra  := @p_fecha_ult_compra
                    );";

            using var cmd = new NpgsqlCommand(sql, connection)
            {
                CommandType = System.Data.CommandType.Text
            };

            // Parámetros de Dirección
            cmd.Parameters.AddWithValue("p_calle", entity.Address.Calle ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("p_numero_edificio", entity.Address.NumeroEdificio ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("p_codigo_postal", entity.Address.CodigoPostal ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("p_ciudad_id", entity.Address.Ciudad_Id);
            cmd.Parameters.AddWithValue("p_info_adicional", entity.Address.InfoAdicional ?? (object)DBNull.Value);

            // Parámetros de Tercero (DtoClient)
            cmd.Parameters.AddWithValue("p_nombre", entity.Nombre ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("p_apellidos", entity.Apellidos ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("p_email", entity.Email ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("p_tipo_tercero_id", entity.TipoTercero_id);
            cmd.Parameters.AddWithValue("p_tipo_documento_id", entity.TipoDoc_id);

            // Parámetros de Cliente (DtoCli)
            cmd.Parameters.AddWithValue("p_fecha_nac", entity.Client.FechaNacimiento ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("p_fecha_ult_compra", entity.Client.FechaUltimaCompra ?? (object)DBNull.Value);

            // Ejecutar
            cmd.ExecuteNonQuery();
        }

        public void Eliminar(int id)
        {
            var connection = _conexion.ObtenerConexion();
            string query = "DELETE FROM cliente WHERE id = @id;";
            using var cmd = new NpgsqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@id", id);
            cmd.ExecuteNonQuery();

        }

        public List<DtoClient> ObtenerTodos()
        {
            var clientes = new List<DtoClient>();

            // Aquí obtienes la conexión abierta de tu singleton…
            var connection = _conexion.ObtenerConexion();

            string query = "SELECT c.id AS id, t.nombre AS nombre FROM cliente c JOIN terceros t ON c.tercero_id = t.id ORDER BY c.id ASC;";

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