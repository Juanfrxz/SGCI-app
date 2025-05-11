using Npgsql;
using SGCI_app.domain.DTO;
using SGCI_app.domain.Ports;
using SGCI_app.infrastructure.postgres;

namespace SGCI_app.infrastructure.Repositories
{
    public class ImpEmployeeRepository : IGenericRepository<DtoEmployee>, IEmployeeRepository
    {
        private readonly ConexionSingleton _conexion;
        public ImpEmployeeRepository(string connectionString)
        {
            _conexion = ConexionSingleton.Instancia(connectionString);
        }
        // Implementation of the repository methods goes here
        public void Actualizar(int id, DtoEmployee entity)
        {
            // Obtenemos la conexión y la abrimos si hace falta
            var connection = _conexion.ObtenerConexion();

            // Llamada al procedimiento
            const string sql = @"
        CALL public.sp_update_employee(
            p_empleado_id          => @p_empleado_id,
            p_nombre               => @p_nombre,
            p_apellidos            => @p_apellidos,
            p_email                => @p_email,
            p_tipo_tercero_id      => @p_tipo_tercero_id,
            p_tipo_documento_id    => @p_tipo_documento_id,
            p_salario_base         => @p_salario_base,
            p_eps_id               => @p_eps_id,
            p_arl_id               => @p_arl_id
        );
    ";

            using var cmd = new NpgsqlCommand(sql, connection)
            {
                CommandType = System.Data.CommandType.Text
            };

            // 1) Parámetro clave: id del empleado (cliente.id)
            cmd.Parameters.AddWithValue("p_empleado_id", id);

            // 2) Datos en 'terceros'
            cmd.Parameters.AddWithValue("p_nombre", entity.Nombre ?? (object) DBNull.Value);
            cmd.Parameters.AddWithValue("p_apellidos", entity.Apellidos ?? (object) DBNull.Value);
            cmd.Parameters.AddWithValue("p_email", entity.Email ?? (object) DBNull.Value);
            cmd.Parameters.AddWithValue("p_tipo_tercero_id", entity.TipoTercero_id);
            cmd.Parameters.AddWithValue("p_tipo_documento_id", entity.TipoDoc_id);

            // 3) Datos en 'empleado' (si no tienes estas propiedades en tu DTO,
            //    crea entity.SalarioBase, entity.EpsId y entity.ArlId como double? e int?)
            cmd.Parameters.AddWithValue("p_salario_base", entity.Employee.SalarioBase);
            cmd.Parameters.AddWithValue("p_eps_id", entity.Employee.Eps_id);
            cmd.Parameters.AddWithValue("p_arl_id", entity.Employee.Arl_id);

            // 4) Ejecutar
            var rows = cmd.ExecuteNonQuery();
            if (rows == 0)
                throw new InvalidOperationException($"No se encontró empleado con id = {id} para actualizar.");

            Console.WriteLine("Datos del empleado actualizados exitosamente.");
        }

        public void Actualizar(DtoEmployee entity)
        {
            throw new NotImplementedException();
        }

        public void Crear(DtoEmployee entity)
        {
            // Obtener la conexión
            var connection = _conexion.ObtenerConexion();

            var sql = @"CALL public.sp_create_employee(
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
                        p_fecha_ingreso         := @p_fecha_ingreso ,
                        p_salario_base  := @p_salario_base,
                        p_Eps_id := @p_Eps_id,
                        p_Arl_id := @p_Arl_id
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
            cmd.Parameters.AddWithValue("p_fecha_ingreso", entity.Employee.FechaIngreso ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("p_salario_base", entity.Employee.SalarioBase);
            cmd.Parameters.AddWithValue("p_Eps_id", entity.Employee.Eps_id);
            cmd.Parameters.AddWithValue("p_Arl_id", entity.Employee.Arl_id);

            // Ejecutar
            cmd.ExecuteNonQuery();
        }

        public void Eliminar(int id)
        {
            var connection = _conexion.ObtenerConexion();
            string query = "DELETE FROM empleado WHERE id = @id;";
            using var cmd = new NpgsqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@id", id);
            cmd.ExecuteNonQuery();
        }

        public List<DtoEmployee> ObtenerTodos()
        {
            var empleados = new List<DtoEmployee>();

            // Aquí obtienes la conexión abierta de tu singleton…
            var connection = _conexion.ObtenerConexion();

            string query = "SELECT e.id AS id, t.nombre AS nombre FROM empleado e JOIN terceros t ON e.tercero_id = t.id ORDER BY e.id ASC;";

            // Usamos using sólo en el comando y el reader, no en la conexión singleton
            using var cmd = new NpgsqlCommand(query, connection);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                empleados.Add(new DtoEmployee
                {
                    Id = reader.GetInt32(reader.GetOrdinal("id")),
                    Nombre = reader.GetString(reader.GetOrdinal("nombre"))
                });
            }

            return empleados;
        }
    }
}