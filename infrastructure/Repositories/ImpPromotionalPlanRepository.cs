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
    public class ImpPromotionalPlanRepository : IGenericRepository<PromotionalPlan>, IPromotionalPlanRepository
    {
        private readonly ConexionSingleton _conexion;
        public ImpPromotionalPlanRepository(string connectionString)
        {
            _conexion = ConexionSingleton.Instancia(connectionString);
        }

        public void Actualizar(PromotionalPlan entity)
        {
            var connection = _conexion.ObtenerConexion();

            // Primero obtenemos los valores actuales
            string sqlSelect = @"
        SELECT
            nombre,
            tecnico_id,
            descuento,
            fecha_inicio,
            fecha_fin,
            datos_extra
        FROM planes
        WHERE id = @id;
    ";
            using var cmdSelect = new NpgsqlCommand(sqlSelect, connection);
            cmdSelect.Parameters.AddWithValue("@id", entity.Id);
            using var reader = cmdSelect.ExecuteReader();

            if (!reader.Read())
                throw new InvalidOperationException($"No se encontró un plan con id={entity.Id}");

            var oldNombre = reader.GetString(reader.GetOrdinal("nombre"));
            var oldTecnicoId = reader.GetInt32(reader.GetOrdinal("tecnico_id"));
            var oldDescuento = reader.GetDouble(reader.GetOrdinal("descuento"));
            var oldInicio = reader.IsDBNull(reader.GetOrdinal("fecha_inicio"))
                                    ? (DateTime?)null
                                    : reader.GetDateTime(reader.GetOrdinal("fecha_inicio"));
            var oldFin = reader.IsDBNull(reader.GetOrdinal("fecha_fin"))
                                    ? (DateTime?)null
                                    : reader.GetDateTime(reader.GetOrdinal("fecha_fin"));
            var oldDatosExtra = reader.IsDBNull(reader.GetOrdinal("datos_extra"))
                                    ? null
                                    : reader.GetString(reader.GetOrdinal("datos_extra"));
            reader.Close();

            // Determinamos los nuevos valores, usando el antiguo si viene nulo o vacío
            var newNombre = string.IsNullOrWhiteSpace(entity.Nombre) ? oldNombre : entity.Nombre!;
            var newTecnicoId = entity.Tecnico_Id == 0 ? oldTecnicoId : entity.Tecnico_Id;
            var newDescuento = entity.Descuento.Equals(default(double)) ? oldDescuento : entity.Descuento;
            var newInicio = entity.Inicio.HasValue ? entity.Inicio : oldInicio;
            var newFin = entity.Fin.HasValue ? entity.Fin : oldFin;
            var newDatosExtra = string.IsNullOrWhiteSpace(entity.Datos_Extra) ? oldDatosExtra : entity.Datos_Extra;

            // Ejecutamos el UPDATE con los valores fusionados
            string sqlUpdate = @"
        UPDATE planes
        SET
            nombre       = @nombre,
            tecnico_id   = @tecnico_id,
            descuento    = @descuento,
            fecha_inicio = @fecha_inicio,
            fecha_fin    = @fecha_fin,
            datos_extra  = @datos_extra
        WHERE id = @id;
    ";
            using var cmdUpdate = new NpgsqlCommand(sqlUpdate, connection);
            cmdUpdate.Parameters.AddWithValue("@nombre", newNombre);
            cmdUpdate.Parameters.AddWithValue("@tecnico_id", newTecnicoId);
            cmdUpdate.Parameters.AddWithValue("@descuento", newDescuento);
            cmdUpdate.Parameters.AddWithValue("@fecha_inicio", newInicio.HasValue
                ? (object)newInicio.Value.Date
                : DBNull.Value);
            cmdUpdate.Parameters.AddWithValue("@fecha_fin", newFin.HasValue
                ? (object)newFin.Value.Date
                : DBNull.Value);
            cmdUpdate.Parameters.AddWithValue("@datos_extra", newDatosExtra ?? (object) DBNull.Value);
            cmdUpdate.Parameters.AddWithValue("@id", entity.Id);

            cmdUpdate.ExecuteNonQuery();
        }


        public void Crear(PromotionalPlan entity)
        {
            // Obtenemos la conexión (singleton)
            var connection = _conexion.ObtenerConexion();

            // Sentencia INSERT para la tabla planes
            string sql = @"
        INSERT INTO planes (
            nombre,
            tecnico_id,
            descuento,
            fecha_inicio,
            fecha_fin,
            datos_extra
        ) VALUES (
            @nombre,
            @tecnico_id,
            @descuento,
            @fecha_inicio,
            @fecha_fin,
            @datos_extra
        );
    ";

            using var cmd = new NpgsqlCommand(sql, connection);

            // Parámetros
            cmd.Parameters.AddWithValue("@nombre", entity.Nombre ?? string.Empty);
            cmd.Parameters.AddWithValue("@tecnico_id", entity.Tecnico_Id);
            cmd.Parameters.AddWithValue("@descuento", entity.Descuento);
            cmd.Parameters.AddWithValue("@fecha_inicio", entity.Inicio.HasValue
                ? (object)entity.Inicio.Value.Date
                : DBNull.Value);
            cmd.Parameters.AddWithValue("@fecha_fin", entity.Fin.HasValue
                ? (object)entity.Fin.Value.Date
                : DBNull.Value);
            cmd.Parameters.AddWithValue("@datos_extra", entity.Datos_Extra ?? (object)DBNull.Value);

            // Ejecutamos
            cmd.ExecuteNonQuery();
        }


        public void Eliminar(int id)
        {
            // Obtenemos la conexión (singleton)
            var connection = _conexion.ObtenerConexion();

            // Sentencia DELETE para la tabla planes
            string sql = @"
        DELETE FROM planes
        WHERE id = @id;
    ";

            using var cmd = new NpgsqlCommand(sql, connection);
            cmd.Parameters.AddWithValue("@id", id);
            cmd.ExecuteNonQuery();
        }


        public List<PromotionalPlan> ObtenerTodos()
        {
            var resultados = new List<PromotionalPlan>();

            // Conexión (singleton)
            var connection = _conexion.ObtenerConexion();

            // Solo traemos: id, nombre, descuento, fecha_inicio y fecha_fin
            string sql = @"
        SELECT
            id,
            nombre,
            descuento,
            fecha_inicio,
            fecha_fin
        FROM planes
        ORDER BY id ASC;
    ";

            using var cmd = new NpgsqlCommand(sql, connection);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                resultados.Add(new PromotionalPlan
                {
                    Id = reader.GetInt32(reader.GetOrdinal("id")),
                    Nombre = reader.GetString(reader.GetOrdinal("nombre")),
                    Descuento = reader.GetDouble(reader.GetOrdinal("descuento")),
                    Inicio = reader.IsDBNull(reader.GetOrdinal("fecha_inicio"))
                        ? (DateTime?)null
                        : reader.GetDateTime(reader.GetOrdinal("fecha_inicio")),
                    Fin = reader.IsDBNull(reader.GetOrdinal("fecha_fin"))
                        ? (DateTime?)null
                        : reader.GetDateTime(reader.GetOrdinal("fecha_fin"))
                });
            }

            return resultados;
        }

    }
}