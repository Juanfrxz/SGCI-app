using System;
using System.Collections.Generic;
using Npgsql;
using SGCI_app.domain.Entities;
using SGCI_app.domain.Ports;
using SGCI_app.infrastructure.postgres;

namespace SGCI_app.infrastructure.Repositories
{
    public class ImpProductSupplierRepository : IGenericRepository<ProductSupplier>, IProductSupplierRepository
    {
        private readonly ConexionSingleton _conexion;

        public ImpProductSupplierRepository(string connectionString)
        {
            _conexion = ConexionSingleton.Instancia(connectionString);
        }

        public void Crear(ProductSupplier entity)
        {
            var conn = _conexion.ObtenerConexion();
            const string sql = @"
INSERT INTO productos_proveedor (tercero_id, producto_id)
VALUES (@tercero_id, @producto_id);
";
            using var cmd = new NpgsqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@tercero_id", entity.Tercero_Id ?? throw new ArgumentNullException(nameof(entity.Tercero_Id)));
            cmd.Parameters.AddWithValue("@producto_id", entity.Producto_Id ?? throw new ArgumentNullException(nameof(entity.Producto_Id)));
            cmd.ExecuteNonQuery();
        }

        public void Actualizar(ProductSupplier entity)
        {
            throw new NotSupportedException(
                "Actualizar directo no soportado, use la sobrecarga que recibe claves antiguas y nuevas.");
        }

        public void Actualizar(string oldTerceroId, string oldProductoId, ProductSupplier entity)
        {
            // elimina asociación antigua
            Eliminar(oldTerceroId, oldProductoId);
            // inserta nueva
            Crear(entity);
        }

        public List<ProductSupplier> ObtenerTodos()
        {
            var list = new List<ProductSupplier>();
            var conn = _conexion.ObtenerConexion();
            const string sql = @"
SELECT tercero_id, producto_id
FROM productos_proveedor
ORDER BY tercero_id, producto_id;
";
            using var cmd = new NpgsqlCommand(sql, conn);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new ProductSupplier
                {
                    Tercero_Id = reader.GetString(reader.GetOrdinal("tercero_id")),
                    Producto_Id = reader.GetString(reader.GetOrdinal("producto_id"))
                });
            }
            return list;
        }

        public void Eliminar(int id)
        {
            throw new NotSupportedException(
                "Eliminar por id no soportado. Use la sobrecarga que recibe keys compuestas.");
        }

        public void Eliminar(string terceroId, string productoId)
        {
            var conn = _conexion.ObtenerConexion();
            const string sql = @"
DELETE FROM productos_proveedor
WHERE tercero_id = @tercero_id
  AND producto_id = @producto_id;
";
            using var cmd = new NpgsqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@tercero_id", terceroId ?? throw new ArgumentNullException(nameof(terceroId)));
            cmd.Parameters.AddWithValue("@producto_id", productoId ?? throw new ArgumentNullException(nameof(productoId)));
            var rows = cmd.ExecuteNonQuery();
            if (rows == 0)
                throw new InvalidOperationException(
                    $"No se encontró la asociación proveedor {terceroId} / producto {productoId} a eliminar.");
        }
    }
}
