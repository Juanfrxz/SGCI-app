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
        var purchaseDetails = new List<PurchaseDetail>();
        var connection = _conexion.ObtenerConexion();

        string query = "SELECT id, fecha, producto_id, cantidad, valor, compra_id FROM detalle_compra";
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

    public void Crear(PurchaseDetail entity)
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

    public void Actualizar(PurchaseDetail entity)
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

    public void Eliminar(int id)
    {
        var connection = _conexion.ObtenerConexion();
        string query = "DELETE FROM detalle_compra WHERE id = @id";
        
        using var cmd = new NpgsqlCommand(query, connection);
        cmd.Parameters.AddWithValue("@id", id);
        
        cmd.ExecuteNonQuery();
    }
} 