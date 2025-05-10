using SGCI_app.domain.Entities;
using SGCI_app.domain.Ports;
using SGCI_app.infrastructure.postgres;
using Npgsql;

namespace SGCI_app.infrastructure.Repositories;

public class ImpPurchaseRepository : IGenericRepository<Purchase>, IPurchaseRepository
{
    private readonly ConexionSingleton _conexion;

    public ImpPurchaseRepository(string connectionString)
    {
        _conexion = ConexionSingleton.Instancia(connectionString);
    }

    public List<Purchase> ObtenerTodos()
    {
        var purchases = new List<Purchase>();
        var connection = _conexion.ObtenerConexion();

        string query = "SELECT id, tercero_proveedor_id, fecha, tercero_empleado_id, doc_compra FROM compra";
        using var cmd = new NpgsqlCommand(query, connection);
        using var reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            purchases.Add(new Purchase
            {
                Id = reader.GetInt32(reader.GetOrdinal("id")),
                TerceroProveedor_Id = reader.GetString(reader.GetOrdinal("tercero_proveedor_id")),
                Fecha = reader.GetDateTime(reader.GetOrdinal("fecha")),
                TerceroEmpleado_Id = reader.GetString(reader.GetOrdinal("tercero_empleado_id")),
                DocCompra = reader.GetString(reader.GetOrdinal("doc_compra"))
            });
        }

        return purchases;
    }

    public void Crear(Purchase entity)
    {
        var connection = _conexion.ObtenerConexion();
        string query = "INSERT INTO compra (tercero_proveedor_id, fecha, tercero_empleado_id, doc_compra) VALUES (@tercero_proveedor_id, @fecha, @tercero_empleado_id, @doc_compra)";
        
        using var cmd = new NpgsqlCommand(query, connection);
        cmd.Parameters.AddWithValue("@tercero_proveedor_id", entity.TerceroProveedor_Id ?? (object)DBNull.Value);
        cmd.Parameters.AddWithValue("@fecha", entity.Fecha ?? (object)DBNull.Value);
        cmd.Parameters.AddWithValue("@tercero_empleado_id", entity.TerceroEmpleado_Id ?? (object)DBNull.Value);
        cmd.Parameters.AddWithValue("@doc_compra", entity.DocCompra ?? (object)DBNull.Value);
        
        cmd.ExecuteNonQuery();
    }

    public void Actualizar(Purchase entity)
    {
        var connection = _conexion.ObtenerConexion();
        string query = "UPDATE compra SET tercero_proveedor_id = @tercero_proveedor_id, fecha = @fecha, tercero_empleado_id = @tercero_empleado_id, doc_compra = @doc_compra WHERE id = @id";
        
        using var cmd = new NpgsqlCommand(query, connection);
        cmd.Parameters.AddWithValue("@id", entity.Id);
        cmd.Parameters.AddWithValue("@tercero_proveedor_id", entity.TerceroProveedor_Id ?? (object)DBNull.Value);
        cmd.Parameters.AddWithValue("@fecha", entity.Fecha ?? (object)DBNull.Value);
        cmd.Parameters.AddWithValue("@tercero_empleado_id", entity.TerceroEmpleado_Id ?? (object)DBNull.Value);
        cmd.Parameters.AddWithValue("@doc_compra", entity.DocCompra ?? (object)DBNull.Value);
        
        cmd.ExecuteNonQuery();
    }

    public void Eliminar(int id)
    {
        var connection = _conexion.ObtenerConexion();
        string query = "DELETE FROM compra WHERE id = @id";
        
        using var cmd = new NpgsqlCommand(query, connection);
        cmd.Parameters.AddWithValue("@id", id);
        
        cmd.ExecuteNonQuery();
    }
} 