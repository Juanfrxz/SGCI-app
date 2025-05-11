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
    public class ImpPromotionalPlanProductRepository : IGenericRepository<PromotionalPlanProduct>, IPromotionalPlanProductRepository
    {
        private readonly ConexionSingleton _conexion;

        public ImpPromotionalPlanProductRepository(string connectionString)
        {
            _conexion = ConexionSingleton.Instancia(connectionString);
        }
        public void Actualizar(PromotionalPlanProduct entity)
        {
            throw new NotImplementedException();
        }

        public void Actualizar(int planId, string productoId, PromotionalPlanProduct entity)
        {
            // Eliminamos la asociación antigua
            Eliminar(planId, productoId);
            // Insertamos la nueva asociación
            Crear(entity);
        }

        public void Crear(PromotionalPlanProduct entity)
        {
            var conn = _conexion.ObtenerConexion();
            const string sql = @"
INSERT INTO plan_producto (plan_id, producto_id)
VALUES (@plan_id, @producto_id);
";
            using var cmd = new NpgsqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@plan_id", entity.Plan_Id);
            cmd.Parameters.AddWithValue("@producto_id", entity.Producto_Id ?? (object)DBNull.Value);
            cmd.ExecuteNonQuery();
        }

        public void Eliminar(int id)
        {
            throw new NotImplementedException();
        }

        public void Eliminar(int planId, string productoId)
        {
            var connection = _conexion.ObtenerConexion();

            const string sql = @"
                DELETE FROM plan_producto
                WHERE plan_id     = @plan_id
                  AND producto_id = @producto_id;
            ";

            using var cmd = new NpgsqlCommand(sql, connection);
            cmd.Parameters.AddWithValue("@plan_id", planId);
            cmd.Parameters.AddWithValue("@producto_id", productoId ?? throw new ArgumentNullException(nameof(productoId)));
            var filasAfectadas = cmd.ExecuteNonQuery();

            if (filasAfectadas == 0)
            {
                throw new InvalidOperationException($"No se encontró la asociación plan {planId} / producto '{productoId}'");
            }

        }

        public List<PromotionalPlanProduct> ObtenerTodos()
        {
            var list = new List<PromotionalPlanProduct>();
            var conn = _conexion.ObtenerConexion();
            const string sql = @"
SELECT plan_id, producto_id
FROM plan_producto
ORDER BY plan_id ASC;
";
            using var cmd = new NpgsqlCommand(sql, conn);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new PromotionalPlanProduct
                {
                    Plan_Id = reader.GetInt32(reader.GetOrdinal("plan_id")),
                    Producto_Id = reader.GetString(reader.GetOrdinal("producto_id"))
                });
            }

            return list;
        }
    }
}