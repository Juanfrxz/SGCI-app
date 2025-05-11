using SGCI_app.domain.Entities;
using SGCI_app.domain.Ports;
using SGCI_app.infrastructure.postgres;
using Npgsql;

namespace SGCI_app.infrastructure.Repositories;

public class ImpSaleDetailRepository : IGenericRepository<SaleDetail>, ISaleDetailRepository
{
    private readonly ConexionSingleton _conexion;

    public ImpSaleDetailRepository(string connectionString)
    {
        _conexion = ConexionSingleton.Instancia(connectionString);
    }

    public List<SaleDetail> ObtenerTodos()
    {
        var saleDetails = new List<SaleDetail>();
        var connection = _conexion.ObtenerConexion();

        string query = "SELECT id, fact_id, producto_id, cantidad, valor FROM detalle_venta ORDER BY id ASC;";
        using var cmd = new NpgsqlCommand(query, connection);
        using var reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            saleDetails.Add(new SaleDetail
            {
                Id = reader.GetInt32(reader.GetOrdinal("id")),
                FactId = reader.GetInt32(reader.GetOrdinal("fact_id")),
                Producto_Id = reader.GetString(reader.GetOrdinal("producto_id")),
                Cantidad = reader.GetInt32(reader.GetOrdinal("cantidad")),
                Valor = reader.GetInt32(reader.GetOrdinal("valor"))
            });
        }

        return saleDetails;
    }

    public void Crear(SaleDetail entity)
    {
        var connection = _conexion.ObtenerConexion();
        string query = "INSERT INTO detalle_venta (fact_id, producto_id, cantidad, valor) VALUES (@fact_id, @producto_id, @cantidad, @valor)";
        
        using var cmd = new NpgsqlCommand(query, connection);
        cmd.Parameters.AddWithValue("@fact_id", entity.FactId);
        cmd.Parameters.AddWithValue("@producto_id", entity.Producto_Id ?? (object)DBNull.Value);
        cmd.Parameters.AddWithValue("@cantidad", entity.Cantidad);
        cmd.Parameters.AddWithValue("@valor", entity.Valor);
        
        cmd.ExecuteNonQuery();
    }

    public void Actualizar(SaleDetail entity)
    {
        var connection = _conexion.ObtenerConexion();
        string query = "UPDATE detalle_venta SET fact_id = @fact_id, producto_id = @producto_id, cantidad = @cantidad, valor = @valor WHERE id = @id";
        
        using var cmd = new NpgsqlCommand(query, connection);
        cmd.Parameters.AddWithValue("@id", entity.Id);
        cmd.Parameters.AddWithValue("@fact_id", entity.FactId);
        cmd.Parameters.AddWithValue("@producto_id", entity.Producto_Id ?? (object)DBNull.Value);
        cmd.Parameters.AddWithValue("@cantidad", entity.Cantidad);
        cmd.Parameters.AddWithValue("@valor", entity.Valor);
        
        cmd.ExecuteNonQuery();
    }

    public void Eliminar(int id)
    {
        var connection = _conexion.ObtenerConexion();
        string query = "DELETE FROM detalle_venta WHERE id = @id";
        
        using var cmd = new NpgsqlCommand(query, connection);
        cmd.Parameters.AddWithValue("@id", id);
        
        cmd.ExecuteNonQuery();
    }
} 