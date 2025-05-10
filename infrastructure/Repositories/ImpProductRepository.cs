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

        string query = "SELECT id, nombre, stock, updated_at FROM productos";
        using var cmd = new NpgsqlCommand(query, connection);
        using var reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            products.Add(new Product
            {
                Id = reader.GetString(reader.GetOrdinal("id")),
                Nombre = reader.GetString(reader.GetOrdinal("nombre")),
                Stock = reader.GetInt32(reader.GetOrdinal("stock")),
                FechaActualizacion = reader.GetDateTime(reader.GetOrdinal("updated_at"))
            });
        }

        return products;
    }

    public void Crear(Product entity)
    {
        var connection = _conexion.ObtenerConexion();
        string query = @"
        INSERT INTO productos 
            (id, nombre, stock, stock_min, stock_max, created_at, updated_at, barcode) 
        VALUES 
            (@id, @nombre, @stock, @stock_min, @stock_max, @fecha_creacion, @fecha_actualizacion, @codigo_barras);
    ";

        using var cmd = new NpgsqlCommand(query, connection);
        cmd.Parameters.AddWithValue("@id", entity.Id ?? (object)DBNull.Value);
        cmd.Parameters.AddWithValue("@nombre", entity.Nombre ?? (object)DBNull.Value);

        // Si el usuario no proporciona valor, usamos 0 como default
        cmd.Parameters.AddWithValue("@stock", entity.Stock ?? 0);
        cmd.Parameters.AddWithValue("@stock_min", entity.StockMin ?? 0);
        cmd.Parameters.AddWithValue("@stock_max", entity.StockMax ?? 0);

        cmd.Parameters.AddWithValue("@fecha_creacion", entity.FechaCreacion ?? (object)DBNull.Value);
        cmd.Parameters.AddWithValue("@fecha_actualizacion", entity.FechaActualizacion ?? (object)DBNull.Value);
        cmd.Parameters.AddWithValue("@codigo_barras", entity.CodigoBarras ?? (object)DBNull.Value);

        cmd.ExecuteNonQuery();
    }


    public void Actualizar(Product entity)
    {
        var connection = _conexion.ObtenerConexion();
        string query = @"
        UPDATE productos SET 
            nombre = COALESCE(@nombre, nombre),
            stock = COALESCE(@stock, stock),
            stock_min = COALESCE(@stock_min, stock_min),
            stock_max = COALESCE(@stock_max, stock_max),
            updated_at = COALESCE(@fecha_actualizacion, updated_at),
            barcode = COALESCE(@codigo_barras, barcode)
        WHERE id = @id;
    ";

        using var cmd = new NpgsqlCommand(query, connection);
        cmd.Parameters.AddWithValue("@id", entity.Id ?? (object)DBNull.Value);
        cmd.Parameters.AddWithValue("@nombre", string.IsNullOrWhiteSpace(entity.Nombre) ? (object)DBNull.Value : entity.Nombre);
        cmd.Parameters.AddWithValue("@stock", entity.Stock.HasValue ? entity.Stock : (object)DBNull.Value);
        cmd.Parameters.AddWithValue("@stock_min", entity.StockMin.HasValue ? entity.StockMin : (object)DBNull.Value);
        cmd.Parameters.AddWithValue("@stock_max", entity.StockMax.HasValue ? entity.StockMax : (object)DBNull.Value);
        cmd.Parameters.AddWithValue("@fecha_actualizacion", entity.FechaActualizacion ?? (object)DBNull.Value);
        cmd.Parameters.AddWithValue("@codigo_barras", string.IsNullOrWhiteSpace(entity.CodigoBarras) ? (object)DBNull.Value : entity.CodigoBarras);

        cmd.ExecuteNonQuery();
    }

    public void Eliminar(int id)
    {
        throw new NotImplementedException();
    }

    public void Eliminar(string id)
    {
        var connection = _conexion.ObtenerConexion();
        string query = "DELETE FROM productos WHERE id = @id";

        using var cmd = new NpgsqlCommand(query, connection);
        cmd.Parameters.AddWithValue("@id", id);
        cmd.ExecuteNonQuery();
    }
}