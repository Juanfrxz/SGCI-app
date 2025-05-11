using SGCI_app.domain.Entities;
using SGCI_app.domain.Ports;
using SGCI_app.infrastructure.postgres;
using Npgsql;

namespace SGCI_app.infrastructure.Repositories;

public class ImpSaleRepository : IGenericRepository<Sale>, ISaleRepository
{
    private readonly ConexionSingleton _conexion;

    public ImpSaleRepository(string connectionString)
    {
        _conexion = ConexionSingleton.Instancia(connectionString);
    }

    public NpgsqlConnection GetConnection()
    {
        return _conexion.ObtenerConexion();
    }

    public List<Sale> ObtenerTodos()
    {
        var sales = new List<Sale>();
        var connection = _conexion.ObtenerConexion();

        string query = @"
            SELECT v.fact_id, v.fecha, 
                   e.tercero_id as tercero_empleado_id, 
                   c.tercero_id as tercero_cliente_id 
            FROM venta v
            LEFT JOIN empleado e ON v.empleado_id = e.id
            LEFT JOIN cliente c ON v.cliente_id = c.id
            ORDER BY v.fact_id ASC;";
            
        using var cmd = new NpgsqlCommand(query, connection);
        using var reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            sales.Add(new Sale
            {
                FactId = reader.GetInt32(reader.GetOrdinal("fact_id")),
                Fecha = reader.GetDateTime(reader.GetOrdinal("fecha")),
                TerceroEmpleado_Id = reader.IsDBNull(reader.GetOrdinal("tercero_empleado_id")) ? null : reader.GetString(reader.GetOrdinal("tercero_empleado_id")),
                TerceroCliente_Id = reader.IsDBNull(reader.GetOrdinal("tercero_cliente_id")) ? null : reader.GetString(reader.GetOrdinal("tercero_cliente_id"))
            });
        }

        return sales;
    }

    public void Crear(Sale entity)
    {
        var connection = _conexion.ObtenerConexion();
        
        // Primero obtenemos los IDs de empleado y cliente
        int? empleadoId = null;
        int? clienteId = null;
        
        if (!string.IsNullOrEmpty(entity.TerceroEmpleado_Id))
        {
            string queryEmpleado = "SELECT id FROM empleado WHERE tercero_id = @tercero_id";
            using var cmdEmpleado = new NpgsqlCommand(queryEmpleado, connection);
            cmdEmpleado.Parameters.AddWithValue("@tercero_id", entity.TerceroEmpleado_Id);
            var result = cmdEmpleado.ExecuteScalar();
            if (result != null)
            {
                empleadoId = Convert.ToInt32(result);
            }
        }
        
        if (!string.IsNullOrEmpty(entity.TerceroCliente_Id))
        {
            string queryCliente = "SELECT id FROM cliente WHERE tercero_id = @tercero_id";
            using var cmdCliente = new NpgsqlCommand(queryCliente, connection);
            cmdCliente.Parameters.AddWithValue("@tercero_id", entity.TerceroCliente_Id);
            var result = cmdCliente.ExecuteScalar();
            if (result != null)
            {
                clienteId = Convert.ToInt32(result);
            }
        }
        
        string query = "INSERT INTO venta (fecha, empleado_id, cliente_id) VALUES (@fecha, @empleado_id, @cliente_id)";
        
        using var cmd = new NpgsqlCommand(query, connection);
        cmd.Parameters.AddWithValue("@fecha", entity.Fecha ?? (object)DBNull.Value);
        cmd.Parameters.AddWithValue("@empleado_id", empleadoId ?? (object)DBNull.Value);
        cmd.Parameters.AddWithValue("@cliente_id", clienteId ?? (object)DBNull.Value);
        
        cmd.ExecuteNonQuery();
    }

    public void Actualizar(Sale entity)
    {
        var connection = _conexion.ObtenerConexion();
        
        // Primero obtenemos los IDs de empleado y cliente
        int? empleadoId = null;
        int? clienteId = null;
        
        if (!string.IsNullOrEmpty(entity.TerceroEmpleado_Id))
        {
            string queryEmpleado = "SELECT id FROM empleado WHERE tercero_id = @tercero_id";
            using var cmdEmpleado = new NpgsqlCommand(queryEmpleado, connection);
            cmdEmpleado.Parameters.AddWithValue("@tercero_id", entity.TerceroEmpleado_Id);
            var result = cmdEmpleado.ExecuteScalar();
            if (result != null)
            {
                empleadoId = Convert.ToInt32(result);
            }
        }
        
        if (!string.IsNullOrEmpty(entity.TerceroCliente_Id))
        {
            string queryCliente = "SELECT id FROM cliente WHERE tercero_id = @tercero_id";
            using var cmdCliente = new NpgsqlCommand(queryCliente, connection);
            cmdCliente.Parameters.AddWithValue("@tercero_id", entity.TerceroCliente_Id);
            var result = cmdCliente.ExecuteScalar();
            if (result != null)
            {
                clienteId = Convert.ToInt32(result);
            }
        }
        
        string query = "UPDATE venta SET fecha = @fecha, empleado_id = @empleado_id, cliente_id = @cliente_id WHERE fact_id = @fact_id";
        
        using var cmd = new NpgsqlCommand(query, connection);
        cmd.Parameters.AddWithValue("@fact_id", entity.FactId);
        cmd.Parameters.AddWithValue("@fecha", entity.Fecha ?? (object)DBNull.Value);
        cmd.Parameters.AddWithValue("@empleado_id", empleadoId ?? (object)DBNull.Value);
        cmd.Parameters.AddWithValue("@cliente_id", clienteId ?? (object)DBNull.Value);
        
        cmd.ExecuteNonQuery();
    }

    public void Eliminar(int id)
    {
        var connection = _conexion.ObtenerConexion();
        string query = "DELETE FROM venta WHERE fact_id = @id";
        
        using var cmd = new NpgsqlCommand(query, connection);
        cmd.Parameters.AddWithValue("@id", id);
        
        cmd.ExecuteNonQuery();
    }

    public decimal ObtenerTotalVenta(int factId)
    {
        decimal total = 0;
        var connection = _conexion.ObtenerConexion();
        
        string query = @"
            SELECT SUM(cantidad * valor) as total
            FROM detalle_venta
            WHERE fact_id = @fact_id";
            
        using var cmd = new NpgsqlCommand(query, connection);
        cmd.Parameters.AddWithValue("@fact_id", factId);
        
        var result = cmd.ExecuteScalar();
        if (result != null && result != DBNull.Value)
        {
            total = Convert.ToDecimal(result);
        }
        
        return total;
    }

    public int ObtenerCantidadDetalles(int factId)
    {
        var connection = _conexion.ObtenerConexion();
        string query = "SELECT COUNT(*) FROM detalle_venta WHERE fact_id = @fact_id";
        
        using var cmd = new NpgsqlCommand(query, connection);
        cmd.Parameters.AddWithValue("@fact_id", factId);
        
        return Convert.ToInt32(cmd.ExecuteScalar());
    }
}
