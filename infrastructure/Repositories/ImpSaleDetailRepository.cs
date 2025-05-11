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

        string query = @"
            SELECT dv.id, dv.fact_id, dv.producto_id, dv.cantidad, dv.valor,
                   p.nombre as producto_nombre, p.stock
            FROM detalle_venta dv
            JOIN productos p ON dv.producto_id = p.id
            ORDER BY dv.id ASC;";
            
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
                Valor = reader.GetDouble(reader.GetOrdinal("valor"))
            });
        }

        return saleDetails;
    }

    public void Crear(SaleDetail entity)
    {
        var connection = _conexion.ObtenerConexion();
        using var transaction = connection.BeginTransaction();
        
        try
        {
            // Verificar si el producto existe y tiene suficiente stock
            string queryStock = "SELECT stock, nombre FROM productos WHERE id = @producto_id";
            using var cmdStock = new NpgsqlCommand(queryStock, connection, transaction);
            cmdStock.Parameters.AddWithValue("@producto_id", entity.Producto_Id ?? throw new ArgumentNullException(nameof(entity.Producto_Id), "El ID del producto no puede ser nulo"));
            
            using var reader = cmdStock.ExecuteReader();
            if (!reader.Read())
            {
                throw new Exception($"El producto con ID {entity.Producto_Id} no existe");
            }
            
            int stockActual = reader.GetInt32(reader.GetOrdinal("stock"));
            string nombreProducto = reader.GetString(reader.GetOrdinal("nombre"));
            
            if (stockActual < entity.Cantidad)
            {
                throw new Exception($"Stock insuficiente para el producto {nombreProducto}. Stock actual: {stockActual}, Cantidad solicitada: {entity.Cantidad}");
            }
            
            reader.Close();

            // Verificar si la venta existe
            string queryVenta = "SELECT COUNT(*) FROM venta WHERE fact_id = @fact_id";
            using var cmdVenta = new NpgsqlCommand(queryVenta, connection, transaction);
            cmdVenta.Parameters.AddWithValue("@fact_id", entity.FactId);
            
            int ventaExiste = Convert.ToInt32(cmdVenta.ExecuteScalar());
            if (ventaExiste == 0)
            {
                throw new Exception($"La venta con ID {entity.FactId} no existe");
            }

            // Insertar el detalle de venta
            string query = "INSERT INTO detalle_venta (fact_id, producto_id, cantidad, valor) VALUES (@fact_id, @producto_id, @cantidad, @valor)";
            using var cmd = new NpgsqlCommand(query, connection, transaction);
            cmd.Parameters.AddWithValue("@fact_id", entity.FactId);
            cmd.Parameters.AddWithValue("@producto_id", entity.Producto_Id ?? throw new ArgumentNullException(nameof(entity.Producto_Id), "El ID del producto no puede ser nulo"));
            cmd.Parameters.AddWithValue("@cantidad", entity.Cantidad);
            cmd.Parameters.AddWithValue("@valor", entity.Valor);
            
            cmd.ExecuteNonQuery();
            
            transaction.Commit();
        }
        catch (Exception)
        {
            transaction.Rollback();
            throw;
        }
    }

    public void Actualizar(SaleDetail entity)
    {
        var connection = _conexion.ObtenerConexion();
        using var transaction = connection.BeginTransaction();
        
        try
        {
            // Verificar si el detalle existe
            string queryExiste = "SELECT cantidad, producto_id FROM detalle_venta WHERE id = @id";
            using var cmdExiste = new NpgsqlCommand(queryExiste, connection, transaction);
            cmdExiste.Parameters.AddWithValue("@id", entity.Id);
            
            using var reader = cmdExiste.ExecuteReader();
            if (!reader.Read())
            {
                throw new Exception($"El detalle de venta con ID {entity.Id} no existe");
            }
            
            int cantidadAnterior = reader.GetInt32(reader.GetOrdinal("cantidad"));
            string productoId = reader.GetString(reader.GetOrdinal("producto_id"));
            reader.Close();

            // Si se está actualizando la cantidad, verificar stock
            if (entity.Cantidad != cantidadAnterior)
            {
                string queryStock = "SELECT stock, nombre FROM productos WHERE id = @producto_id";
                using var cmdStock = new NpgsqlCommand(queryStock, connection, transaction);
                cmdStock.Parameters.AddWithValue("@producto_id", productoId);
                
                using var readerStock = cmdStock.ExecuteReader();
                if (!readerStock.Read())
                {
                    throw new Exception($"El producto con ID {productoId} no existe");
                }
                
                int stockActual = readerStock.GetInt32(readerStock.GetOrdinal("stock"));
                string nombreProducto = readerStock.GetString(readerStock.GetOrdinal("nombre"));
                
                // Calcular la diferencia de stock necesaria
                int diferenciaStock = entity.Cantidad - cantidadAnterior;
                if (stockActual < diferenciaStock)
                {
                    throw new Exception($"Stock insuficiente para el producto {nombreProducto}. Stock actual: {stockActual}, Cantidad adicional necesaria: {diferenciaStock}");
                }
                
                readerStock.Close();
            }

            string query = "UPDATE detalle_venta SET fact_id = @fact_id, producto_id = @producto_id, cantidad = @cantidad, valor = @valor WHERE id = @id";
            using var cmd = new NpgsqlCommand(query, connection, transaction);
            cmd.Parameters.AddWithValue("@id", entity.Id);
            cmd.Parameters.AddWithValue("@fact_id", entity.FactId);
            cmd.Parameters.AddWithValue("@producto_id", entity.Producto_Id ?? throw new ArgumentNullException(nameof(entity.Producto_Id), "El ID del producto no puede ser nulo"));
            cmd.Parameters.AddWithValue("@cantidad", entity.Cantidad);
            cmd.Parameters.AddWithValue("@valor", entity.Valor);
            
            cmd.ExecuteNonQuery();
            
            transaction.Commit();
        }
        catch (Exception)
        {
            transaction.Rollback();
            throw;
        }
    }

    public void Eliminar(int id)
    {
        var connection = _conexion.ObtenerConexion();
        using var transaction = connection.BeginTransaction();
        
        try
        {
            // Verificar si el detalle existe
            string queryExiste = "SELECT COUNT(*) FROM detalle_venta WHERE id = @id";
            using var cmdExiste = new NpgsqlCommand(queryExiste, connection, transaction);
            cmdExiste.Parameters.AddWithValue("@id", id);
            
            int detalleExiste = Convert.ToInt32(cmdExiste.ExecuteScalar());
            if (detalleExiste == 0)
            {
                throw new Exception($"El detalle de venta con ID {id} no existe");
            }

            string query = "DELETE FROM detalle_venta WHERE id = @id";
            using var cmd = new NpgsqlCommand(query, connection, transaction);
            cmd.Parameters.AddWithValue("@id", id);
            
            cmd.ExecuteNonQuery();
            
            transaction.Commit();
        }
        catch (Exception)
        {
            transaction.Rollback();
            throw;
        }
    }
} 