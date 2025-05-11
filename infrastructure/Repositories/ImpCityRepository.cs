using SGCI_app.domain.Entities;
using SGCI_app.domain.Ports;
using SGCI_app.infrastructure.postgres;
using Npgsql;

namespace SGCI_app.infrastructure.Repositories;

public class ImpCityRepository : IGenericRepository<City>, ICityRepository
{
    private readonly ConexionSingleton _conexion;

    public ImpCityRepository(string connectionString)
    {
        _conexion = ConexionSingleton.Instancia(connectionString);
    }
    public List<City> ObtenerTodos()
    {
        var citys = new List<City>();
        var connection = _conexion.ObtenerConexion();

        string query = "SELECT id, nombre, region_id FROM ciudad ORDER BY id ASC;";
        using var cmd = new NpgsqlCommand(query, connection);
        using var reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            citys.Add(new City
            {
                Id = reader.GetInt32(reader.GetOrdinal("id")),
                Nombre = reader.GetString(reader.GetOrdinal("nombre")),
                Region_Id = reader.GetInt32(reader.GetOrdinal("region_id"))
            });
        }
        return citys;
    }
    public void Actualizar(City entity)
    {
        var connection = _conexion.ObtenerConexion();
        string query = "UPDATE ciudad SET nombre = @nombre, region_id = @region_id WHERE id = @id";

        using var cmd = new NpgsqlCommand(query, connection);
        cmd.Parameters.AddWithValue("@id", entity.Id);
        cmd.Parameters.AddWithValue("@nombre", entity.Nombre ?? (object)DBNull.Value);
        cmd.Parameters.AddWithValue("@region_id", entity.Region_Id);

        cmd.ExecuteNonQuery();
    }

    public void Crear(City entity)
    {
        var connection = _conexion.ObtenerConexion();
        string query = "INSERT INTO ciudad (nombre, region_id) VALUES (@nombre, @region_id)";

        using var cmd = new NpgsqlCommand(query, connection);
        cmd.Parameters.AddWithValue("@nombre", entity.Nombre ?? (object)DBNull.Value);
        cmd.Parameters.AddWithValue("@region_id", entity.Region_Id);

        cmd.ExecuteNonQuery();
    }

    public void Eliminar(int id)
    {
        var connection = _conexion.ObtenerConexion();
        string query = "DELETE FROM ciudad WHERE id = @id";

        using var cmd = new NpgsqlCommand(query, connection);
        cmd.Parameters.AddWithValue("@id", id);
        
        cmd.ExecuteNonQuery();
    }
}
