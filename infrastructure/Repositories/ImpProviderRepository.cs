using Npgsql;
using SGCI_app.domain.DTO;
using SGCI_app.domain.Ports;
using SGCI_app.infrastructure.postgres;
using SGCI_app.domain.Entities;
using System.Collections.Generic;

namespace SGCI_app.infrastructure.Repositories
{
    public class ImpProviderRepository : IGenericRepository<DtoProvider>, IProviderRepository
    {
        private readonly ConexionSingleton _conexion;
        public ImpProviderRepository(string connectionString)
        {
            _conexion = ConexionSingleton.Instancia(connectionString);
        }

        public void Actualizar(DtoProvider entity)
        {
            throw new NotImplementedException();
        }

        public void Actualizar(int id, DtoProvider entity)
        {
            // Obtenemos la conexión (singleton)
            var connection = _conexion.ObtenerConexion();

            // Llamada al procedimiento de actualización de proveedor
            const string sql = @"
        CALL public.sp_update_provider(
            p_provider_id         => @p_provider_id,
            p_nombre              => @p_nombre,
            p_apellidos           => @p_apellidos,
            p_email               => @p_email,
            p_tipo_tercero_id     => @p_tipo_tercero_id,
            p_tipo_documento_id   => @p_tipo_documento_id,
            p_descuento           => @p_descuento,
            p_dia_pago            => @p_dia_pago
        );
    ";

            using var cmd = new NpgsqlCommand(sql, connection)
            {
                CommandType = System.Data.CommandType.Text
            };

            // 1) Parámetro clave: id del proveedor
            cmd.Parameters.AddWithValue("p_provider_id", id);

            // 2) Datos comunes en 'terceros'
            cmd.Parameters.AddWithValue("p_nombre", entity.Nombre ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("p_apellidos", entity.Apellidos ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("p_email", entity.Email ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("p_tipo_tercero_id", entity.TipoTercero_id);
            cmd.Parameters.AddWithValue("p_tipo_documento_id", entity.TipoDoc_id);

            // 3) Datos específicos de proveedor (DtoProv)
            cmd.Parameters.AddWithValue("p_descuento", entity.Provider.Descuento);
            cmd.Parameters.AddWithValue("p_dia_pago", entity.Provider.DiaPago);

            // 4) Ejecutar y validar
            var rows = cmd.ExecuteNonQuery();
            if (rows == 0)
                throw new InvalidOperationException($"No se encontró proveedor con id = {id} para actualizar.");

            Console.WriteLine("Datos del proveedor actualizados exitosamente.");
        }


        public void Crear(DtoProvider entity)
        {
            // Obtener la conexión
            var connection = _conexion.ObtenerConexion();

            var sql = @"CALL public.sp_create_provider(
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
                    p_descuento         := @p_descuento,
                    p_dia_pago          := @p_dia_pago
                );";

            using var cmd = new NpgsqlCommand(sql, connection)
            {
                CommandType = System.Data.CommandType.Text
            };

            // Parámetros de Dirección (DtoAddress)
            cmd.Parameters.AddWithValue("p_calle", entity.Address.Calle ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("p_numero_edificio", entity.Address.NumeroEdificio ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("p_codigo_postal", entity.Address.CodigoPostal ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("p_ciudad_id", entity.Address.Ciudad_Id);
            cmd.Parameters.AddWithValue("p_info_adicional", entity.Address.InfoAdicional ?? (object)DBNull.Value);

            // Parámetros de Tercero (parte común de DtoProvider)
            cmd.Parameters.AddWithValue("p_nombre", entity.Nombre ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("p_apellidos", entity.Apellidos ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("p_email", entity.Email ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("p_tipo_tercero_id", entity.TipoTercero_id);
            cmd.Parameters.AddWithValue("p_tipo_documento_id", entity.TipoDoc_id);

            // Parámetros específicos de Proveedor (DtoProv)
            cmd.Parameters.AddWithValue("p_descuento", entity.Provider.Descuento);
            cmd.Parameters.AddWithValue("p_dia_pago", entity.Provider.DiaPago);

            // Ejecutar el SP
            cmd.ExecuteNonQuery();
        }


        public void Eliminar(int id)
        {
            var connection = _conexion.ObtenerConexion();
            string query = "DELETE FROM proveedor WHERE id = @id;";
            using var cmd = new NpgsqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@id", id);
            cmd.ExecuteNonQuery();
        }

        public List<DtoProvider> ObtenerTodos()
        {
            var resultados = new List<DtoProvider>();

            // Conexión (singleton)
            var connection = _conexion.ObtenerConexion();

            // Solo traemos: el id de proveedor, el nombre del tercero, descuento y día de pago
            string sql = @"
        SELECT
            p.id           AS provider_id,
            t.nombre       AS nombre,
            t.id AS id_tercero,
            p.dto    AS descuento,
            p.dia_pago     AS dia_pago
        FROM proveedor p
        JOIN terceros t ON p.tercero_id = t.id
        ORDER BY p.id ASC;
    ";

            using var cmd = new NpgsqlCommand(sql, connection);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                resultados.Add(new DtoProvider
                {
                    // Id del proveedor
                    Id = reader.GetInt32(reader.GetOrdinal("provider_id")),

                    // Nombre del tercero
                    Nombre = reader.GetString(reader.GetOrdinal("nombre")),

                    // Mapeamos sólo la parte de DtoProv con Descuento y DiaPago
                    Provider = new DtoProv
                    {
                        Descuento = reader.GetDouble(reader.GetOrdinal("descuento")),
                        DiaPago = reader.GetInt32(reader.GetOrdinal("dia_pago")),
                        Tercero_Id = reader.GetString(reader.GetOrdinal("id_tercero"))
                    }
                    // Las otras propiedades (Apellidos, Email, Address, TipoDoc_id, etc.)
                    // permanecerán con sus valores por defecto o null
                });
            }

            return resultados;
        }

    }
}