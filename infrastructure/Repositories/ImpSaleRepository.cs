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

    public List<Sale> ObtenerTodos()
    {
        var sales = new List<Sale>();
        var connection = _conexion.ObtenerConexion();

        string query = "SELECT fact_id, fecha, tercero_empleado_id, tercero_cliente_id FROM venta";
        using var cmd = new NpgsqlCommand(query, connection);
        using var reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            sales.Add(new Sale
            {
                FactId = reader.GetInt32(reader.GetOrdinal("fact_id")),
                Fecha = reader.GetDateTime(reader.GetOrdinal("fecha")),
                TerceroEmpleado_Id = reader.GetString(reader.GetOrdinal("tercero_empleado_id")),
                TerceroCliente_Id = reader.GetString(reader.GetOrdinal("tercero_cliente_id"))
            });
        }

        return sales;
    }

    public void Crear(Sale entity)
    {
        var connection = _conexion.ObtenerConexion();
        string query = "INSERT INTO venta (fecha, tercero_empleado_id, tercero_cliente_id) VALUES (@fecha, @tercero_empleado_id, @tercero_cliente_id)";
        
        using var cmd = new NpgsqlCommand(query, connection);
        cmd.Parameters.AddWithValue("@fecha", entity.Fecha ?? (object)DBNull.Value);
        cmd.Parameters.AddWithValue("@tercero_empleado_id", entity.TerceroEmpleado_Id ?? (object)DBNull.Value);
        cmd.Parameters.AddWithValue("@tercero_cliente_id", entity.TerceroCliente_Id ?? (object)DBNull.Value);
        
        cmd.ExecuteNonQuery();
    }

    public void Actualizar(Sale entity)
    {
        var connection = _conexion.ObtenerConexion();
        string query = "UPDATE venta SET fecha = @fecha, tercero_empleado_id = @tercero_empleado_id, tercero_cliente_id = @tercero_cliente_id WHERE fact_id = @fact_id";
        
        using var cmd = new NpgsqlCommand(query, connection);
        cmd.Parameters.AddWithValue("@fact_id", entity.FactId);
        cmd.Parameters.AddWithValue("@fecha", entity.Fecha ?? (object)DBNull.Value);
        cmd.Parameters.AddWithValue("@tercero_empleado_id", entity.TerceroEmpleado_Id ?? (object)DBNull.Value);
        cmd.Parameters.AddWithValue("@tercero_cliente_id", entity.TerceroCliente_Id ?? (object)DBNull.Value);
        
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
}
