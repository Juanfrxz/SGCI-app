using SGCI_app.domain.Entities;
using SGCI_app.domain.Ports;
using SGCI_app.infrastructure.postgres;
using Npgsql;

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
                    Valor = reader.GetDouble(reader.GetOrdinal("valor")),
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
            string query = "INSERT INTO detalle_compra (fecha, producto_id, cantidad, valor, compra_id) VALUES (@fecha, @producto_id, @cantidad, @valor, @compra_id)";
            
            using var cmd = new NpgsqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@fecha", entity.Fecha);
            cmd.Parameters.AddWithValue("@producto_id", entity.Producto_Id ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@cantidad", entity.Cantidad);
            cmd.Parameters.AddWithValue("@valor", entity.Valor);
            cmd.Parameters.AddWithValue("@compra_id", entity.Compra_Id);
            
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
                case "23503": // foreign key violation
                    throw new Exception("Error: La compra o el producto referenciado no existe.");
                case "23505": // unique violation
                    throw new Exception("Error: Ya existe un detalle de compra con estos datos.");
                case "23514": // check violation
                    throw new Exception("Error: Los datos no cumplen con las restricciones de la base de datos.");
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
            string query = "UPDATE detalle_compra SET fecha = @fecha, producto_id = @producto_id, cantidad = @cantidad, valor = @valor, compra_id = @compra_id WHERE id = @id";
            
            using var cmd = new NpgsqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@id", entity.Id);
            cmd.Parameters.AddWithValue("@fecha", entity.Fecha);
            cmd.Parameters.AddWithValue("@producto_id", entity.Producto_Id ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@cantidad", entity.Cantidad);
            cmd.Parameters.AddWithValue("@valor", entity.Valor);
            cmd.Parameters.AddWithValue("@compra_id", entity.Compra_Id);
            
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
                case "23503": // foreign key violation
                    throw new Exception("Error: La compra o el producto referenciado no existe.");
                case "23505": // unique violation
                    throw new Exception("Error: Ya existe un detalle de compra con estos datos.");
                case "23514": // check violation
                    throw new Exception("Error: Los datos no cumplen con las restricciones de la base de datos.");
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
                case "23503": // foreign key violation
                    throw new Exception("Error: No se puede eliminar el detalle porque está siendo referenciado por otros registros.");
                default:
                    throw new Exception($"Error de base de datos: {ex.Message}");
            }
        }
    }
} 