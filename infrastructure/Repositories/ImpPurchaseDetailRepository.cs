using SGCI_app.domain.Entities;
using SGCI_app.domain.Ports;
using SGCI_app.infrastructure.postgres;
using Npgsql;
using System;

namespace SGCI_app.infrastructure.Repositories;

public class ImpPurchaseDetailRepository : IGenericRepository<PurchaseDetail>, IPurchaseDetailRepository
{
    private readonly ConexionSingleton _conexion;

    public ImpPurchaseDetailRepository(string connectionString)
    {
        _conexion = ConexionSingleton.Instancia(connectionString);
    }

    public List<PurchaseDetail> ObtenerTodos()
    {
        try
        {
            var purchaseDetails = new List<PurchaseDetail>();
            var connection = _conexion.ObtenerConexion();

            string query = "SELECT id, fecha, producto_id, cantidad, valor, compra_id FROM detalle_compra ORDER BY id ASC;";
            using var cmd = new NpgsqlCommand(query, connection);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                purchaseDetails.Add(new PurchaseDetail
                {
                    Id = reader.GetInt32(reader.GetOrdinal("id")),
                    Fecha = reader.GetDateTime(reader.GetOrdinal("fecha")),
                    Producto_Id = reader.GetString(reader.GetOrdinal("producto_id")),
                    Cantidad = reader.GetInt32(reader.GetOrdinal("cantidad")),
                    Valor = reader.GetInt32(reader.GetOrdinal("valor")),
                    Compra_Id = reader.GetInt32(reader.GetOrdinal("compra_id"))
                });
            }

            return purchaseDetails;
        }
        catch (PostgresException ex)
        {
            switch (ex.SqlState)
            {
                case "42P01": // relation does not exist
                    throw new Exception("La tabla de detalles de compra no existe en la base de datos.");
                case "28P01": // password authentication failed
                    throw new Exception("Error de autenticación con la base de datos. Verifique las credenciales.");
                default:
                    throw new Exception($"Error de base de datos: {ex.Message}");
            }
        }
    }

    public void Crear(PurchaseDetail entity)
    {
        try
        {
            var connection = _conexion.ObtenerConexion();

            // Verificar si existe la compra
            string queryCompra = "SELECT id FROM compras WHERE id = @compra_id";
            using (var cmd = new NpgsqlCommand(queryCompra, connection))
            {
                cmd.Parameters.AddWithValue("@compra_id", entity.Compra_Id);
                var result = cmd.ExecuteScalar();
                if (result == null || result == DBNull.Value)
                {
                    throw new Exception($"No existe una compra con el ID: {entity.Compra_Id}");
                }
            }

            // Verificar si existe el producto
            string queryProducto = "SELECT id FROM productos WHERE id = @producto_id";
            using (var cmd = new NpgsqlCommand(queryProducto, connection))
            {
                cmd.Parameters.AddWithValue("@producto_id", entity.Producto_Id ?? (object)DBNull.Value);
                var result = cmd.ExecuteScalar();
                if (result == null || result == DBNull.Value)
                {
                    throw new Exception($"No existe un producto con el ID: {entity.Producto_Id}");
                }
            }

            string query = "INSERT INTO detalle_compra (fecha, producto_id, cantidad, valor, compra_id) VALUES (@fecha, @producto_id, @cantidad, @valor, @compra_id)";
            
            using var cmdInsert = new NpgsqlCommand(query, connection);
            cmdInsert.Parameters.AddWithValue("@fecha", entity.Fecha);
            cmdInsert.Parameters.AddWithValue("@producto_id", entity.Producto_Id ?? (object)DBNull.Value);
            cmdInsert.Parameters.AddWithValue("@cantidad", entity.Cantidad);
            cmdInsert.Parameters.AddWithValue("@valor", entity.Valor);
            cmdInsert.Parameters.AddWithValue("@compra_id", entity.Compra_Id);
            
            cmdInsert.ExecuteNonQuery();
        }
        catch (PostgresException ex)
        {
            switch (ex.SqlState)
            {
                case "42P01": // relation does not exist
                    throw new Exception("La tabla de detalles de compra no existe en la base de datos.");
                case "28P01": // password authentication failed
                    throw new Exception("Error de autenticación con la base de datos. Verifique las credenciales.");
                case "23503": // foreign key violation
                    throw new Exception("Error de integridad referencial. Verifique que la compra y el producto existan.");
                case "42804": // column type mismatch
                    throw new Exception("Error de tipo de datos en la base de datos.");
                default:
                    throw new Exception($"Error de base de datos: {ex.Message}");
            }
        }
    }

    public void Actualizar(PurchaseDetail entity)
    {
        try
        {
            var connection = _conexion.ObtenerConexion();

            // Verificar si existe la compra
            string queryCompra = "SELECT id FROM compras WHERE id = @compra_id";
            using (var cmd = new NpgsqlCommand(queryCompra, connection))
            {
                cmd.Parameters.AddWithValue("@compra_id", entity.Compra_Id);
                var result = cmd.ExecuteScalar();
                if (result == null || result == DBNull.Value)
                {
                    throw new Exception($"No existe una compra con el ID: {entity.Compra_Id}");
                }
            }

            // Verificar si existe el producto
            string queryProducto = "SELECT id FROM productos WHERE id = @producto_id";
            using (var cmd = new NpgsqlCommand(queryProducto, connection))
            {
                cmd.Parameters.AddWithValue("@producto_id", entity.Producto_Id ?? (object)DBNull.Value);
                var result = cmd.ExecuteScalar();
                if (result == null || result == DBNull.Value)
                {
                    throw new Exception($"No existe un producto con el ID: {entity.Producto_Id}");
                }
            }

            string query = "UPDATE detalle_compra SET fecha = @fecha, producto_id = @producto_id, cantidad = @cantidad, valor = @valor, compra_id = @compra_id WHERE id = @id";
            
            using var cmdUpdate = new NpgsqlCommand(query, connection);
            cmdUpdate.Parameters.AddWithValue("@id", entity.Id);
            cmdUpdate.Parameters.AddWithValue("@fecha", entity.Fecha);
            cmdUpdate.Parameters.AddWithValue("@producto_id", entity.Producto_Id ?? (object)DBNull.Value);
            cmdUpdate.Parameters.AddWithValue("@cantidad", entity.Cantidad);
            cmdUpdate.Parameters.AddWithValue("@valor", entity.Valor);
            cmdUpdate.Parameters.AddWithValue("@compra_id", entity.Compra_Id);
            
            cmdUpdate.ExecuteNonQuery();
        }
        catch (PostgresException ex)
        {
            switch (ex.SqlState)
            {
                case "42P01": // relation does not exist
                    throw new Exception("La tabla de detalles de compra no existe en la base de datos.");
                case "28P01": // password authentication failed
                    throw new Exception("Error de autenticación con la base de datos. Verifique las credenciales.");
                case "23503": // foreign key violation
                    throw new Exception("Error de integridad referencial. Verifique que la compra y el producto existan.");
                case "42804": // column type mismatch
                    throw new Exception("Error de tipo de datos en la base de datos.");
                default:
                    throw new Exception($"Error de base de datos: {ex.Message}");
            }
        }
    }

    public void Eliminar(int id)
    {
        try
        {
            var connection = _conexion.ObtenerConexion();
            string query = "DELETE FROM detalle_compra WHERE id = @id";
            
            using var cmd = new NpgsqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@id", id);
            
            cmd.ExecuteNonQuery();
        }
        catch (PostgresException ex)
        {
            switch (ex.SqlState)
            {
                case "42P01": // relation does not exist
                    throw new Exception("La tabla de detalles de compra no existe en la base de datos.");
                case "28P01": // password authentication failed
                    throw new Exception("Error de autenticación con la base de datos. Verifique las credenciales.");
                default:
                    throw new Exception($"Error de base de datos: {ex.Message}");
            }
        }
    }
} 