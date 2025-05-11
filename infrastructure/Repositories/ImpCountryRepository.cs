using SGCI_app.domain.Entities;
using SGCI_app.domain.Ports;
using SGCI_app.infrastructure.postgres;
using Npgsql;

namespace SGCI_app.infrastructure.Repositories;

public class ImpCountryRepository : IGenericRepository<Country>, ICountryRepository
{
    private readonly ConexionSingleton _conexion;
    public ImpCountryRepository(string connectionString)
    {
        _conexion = ConexionSingleton.Instancia(connectionString);
    }
    public List<Country> ObtenerTodos()
    {
        var countries = new List<Country>();
        var connection = _conexion.ObtenerConexion();

        string query = "SELECT id, nombre FROM pais ORDER BY id ASC;";
        using var cmd = new NpgsqlCommand(query, connection);
        using var reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            countries.Add(new Country
            {
                Id = reader.GetInt32(reader.GetOrdinal("id")),
                Nombre = reader.GetString(reader.GetOrdinal("nombre"))
            });
        }

        return countries;
    }

    public void Actualizar(Country entity)
    {
        var connection = _conexion.ObtenerConexion();
        string query = "UPDATE pais SET nombre = @nombre WHERE id = @id";

        using var cmd = new NpgsqlCommand(query, connection);
        cmd.Parameters.AddWithValue("@id", entity.Id);
        cmd.Parameters.AddWithValue("@nombre", entity.Nombre ?? (object)DBNull.Value);
        cmd.ExecuteNonQuery();
    }

    public void Crear(Country entity)
    {
        var connection = _conexion.ObtenerConexion();
        string query = "INSERT INTO pais (nombre) VALUES (@nombre)";

        using var cmd = new NpgsqlCommand(query, connection);
        cmd.Parameters.AddWithValue("@nombre", entity.Nombre ?? (object)DBNull.Value);
        cmd.ExecuteNonQuery();
    }

    public void Eliminar(int id)
    {
        var connection = _conexion.ObtenerConexion();
        string query = "DELETE FROM pais WHERE id = @id";

        using var cmd = new NpgsqlCommand(query, connection);
        cmd.Parameters.AddWithValue("@id", id);
        cmd.ExecuteNonQuery();
    }
}
