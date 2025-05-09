using SGCI_app.domain.Entities;
using SGCI_app.domain.Ports;
using SGCI_app.infrastructure.postgres;
using Npgsql;

namespace SGCI_app.infrastructure.Repositories;

public class ImpProductRepository : IGenericRepository<Product>, IProductRepository
{
    private readonly ConexionSingleton _conexion;
    public ImpProductRepository(string connectionString)
    {
        _conexion = ConexionSingleton.Instancia(connectionString);
    }

    public List<Product> ObtenerTodos()
    {
        var products = new List<Product>();
        var connection = _conexion.ObtenerConexion();

        string query = "SELECT id, nombre, stock, stock_min, stock_max, fecha_creacion, fecha_actualizacion, codigo_barras FROM productos";
        using var cmd = new NpgsqlCommand(query, connection);
        using var reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            products.Add(new Product
            {
                Id = reader.GetString(reader.GetOrdinal("id")),
                Nombre = reader.GetString(reader.GetOrdinal("nombre")),
                Stock = reader.GetInt32(reader.GetOrdinal("stock")),
                StockMin = reader.GetInt32(reader.GetOrdinal("stock_min")),
                StockMax = reader.GetInt32(reader.GetOrdinal("stock_max")),
                FechaCreacion = reader.GetDateTime(reader.GetOrdinal("fecha_creacion")),
                FechaActualizacion = reader.GetDateTime(reader.GetOrdinal("fecha_actualizacion")),
                CodigoBarras = reader.GetString(reader.GetOrdinal("codigo_barras"))
            });
        }

        return products;
    }

    public void Crear(Product entity)
    {
        var connection = _conexion.ObtenerConexion();
        string query = "INSERT INTO productos (id, nombre, stock, stock_min, stock_max, fecha_creacion, fecha_actualizacion, codigo_barras) VALUES (@id, @nombre, @stock, @stock_min, @stock_max, @fecha_creacion, @fecha_actualizacion, @codigo_barras)";
        
        using var cmd = new NpgsqlCommand(query, connection);
        cmd.Parameters.AddWithValue("@id", entity.Id ?? (object)DBNull.Value);
        cmd.Parameters.AddWithValue("@nombre", entity.Nombre ?? (object)DBNull.Value);
        cmd.Parameters.AddWithValue("@stock", entity.Stock);
        cmd.Parameters.AddWithValue("@stock_min", entity.StockMin);
        cmd.Parameters.AddWithValue("@stock_max", entity.StockMax);
        cmd.Parameters.AddWithValue("@fecha_creacion", entity.FechaCreacion ?? (object)DBNull.Value);
        cmd.Parameters.AddWithValue("@fecha_actualizacion", entity.FechaActualizacion ?? (object)DBNull.Value);
        cmd.Parameters.AddWithValue("@codigo_barras", entity.CodigoBarras ?? (object)DBNull.Value);
        
        cmd.ExecuteNonQuery();
    }

    public void Actualizar(Product entity)
    {
        var connection = _conexion.ObtenerConexion();
        string query = "UPDATE productos SET nombre = @nombre, stock = @stock, stock_min = @stock_min, stock_max = @stock_max, fecha_creacion = @fecha_creacion, fecha_actualizacion = @fecha_actualizacion, codigo_barras = @codigo_barras WHERE id = @id";
        
        using var cmd = new NpgsqlCommand(query, connection);
        cmd.Parameters.AddWithValue("@id", entity.Id ?? (object)DBNull.Value);
        cmd.Parameters.AddWithValue("@nombre", entity.Nombre ?? (object)DBNull.Value);
        cmd.Parameters.AddWithValue("@stock", entity.Stock);
        cmd.Parameters.AddWithValue("@stock_min", entity.StockMin);
        cmd.Parameters.AddWithValue("@stock_max", entity.StockMax);
        cmd.Parameters.AddWithValue("@fecha_creacion", entity.FechaCreacion ?? (object)DBNull.Value);
        cmd.Parameters.AddWithValue("@fecha_actualizacion", entity.FechaActualizacion ?? (object)DBNull.Value);
        cmd.Parameters.AddWithValue("@codigo_barras", entity.CodigoBarras ?? (object)DBNull.Value);
        
        cmd.ExecuteNonQuery();
    }

    public void Eliminar(int id)
    {
        var connection = _conexion.ObtenerConexion();
        string query = "DELETE FROM productos WHERE id = @id";
        
        using var cmd = new NpgsqlCommand(query, connection);
        cmd.Parameters.AddWithValue("@id", id.ToString());
        cmd.ExecuteNonQuery();
    }
}
