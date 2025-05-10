using SGCI_app.domain.Entities;
using SGCI_app.domain.Ports;
using SGCI_app.infrastructure.postgres;
using Npgsql;

namespace SGCI_app.infrastructure.Repositories;

public class ImpThirdPartyPhoneRepository : IGenericRepository<ThirdPartyPhone>, IThirdPartyPhoneRepository
{
    private readonly ConexionSingleton _conexion;

    public ImpThirdPartyPhoneRepository(string connectionString)
    {
        _conexion = ConexionSingleton.Instancia(connectionString);
    }

    public List<ThirdPartyPhone> ObtenerTodos()
    {
        var thirdPartyPhones = new List<ThirdPartyPhone>();
        var connection = _conexion.ObtenerConexion();

        string query = "SELECT id, tercero_id, telefono, tipo FROM tercero_telefono";
        using var cmd = new NpgsqlCommand(query, connection);
        using var reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            thirdPartyPhones.Add(new ThirdPartyPhone
            {
                Id = reader.GetInt32(reader.GetOrdinal("id")),
                Tercer_Id = reader.GetString(reader.GetOrdinal("tercero_id")),
                Numero = reader.GetString(reader.GetOrdinal("telefono")),
                Tipo = reader.GetString(reader.GetOrdinal("tipo"))
            });
        }

        return thirdPartyPhones;
    }

    public void Crear(ThirdPartyPhone entity)
    {
        var connection = _conexion.ObtenerConexion();
        string query = "INSERT INTO tercero_telefono (tercer_id, numero, tipo) VALUES (@tercer_id, @numero, @tipo)";
        
        using var cmd = new NpgsqlCommand(query, connection);
        cmd.Parameters.AddWithValue("@tercer_id", entity.Tercer_Id ?? (object)DBNull.Value);
        cmd.Parameters.AddWithValue("@numero", entity.Numero ?? (object)DBNull.Value);
        cmd.Parameters.AddWithValue("@tipo", entity.Tipo ?? (object)DBNull.Value);
        
        cmd.ExecuteNonQuery();
    }

    public void Actualizar(ThirdPartyPhone entity)
    {
        var connection = _conexion.ObtenerConexion();
        string query = "UPDATE tercero_telefono SET tercer_id = @tercer_id, numero = @numero, tipo = @tipo WHERE id = @id";
        
        using var cmd = new NpgsqlCommand(query, connection);
        cmd.Parameters.AddWithValue("@id", entity.Id);
        cmd.Parameters.AddWithValue("@tercer_id", entity.Tercer_Id ?? (object)DBNull.Value);
        cmd.Parameters.AddWithValue("@numero", entity.Numero ?? (object)DBNull.Value);
        cmd.Parameters.AddWithValue("@tipo", entity.Tipo ?? (object)DBNull.Value);
        
        cmd.ExecuteNonQuery();
    }

    public void Eliminar(int id)
    {
        var connection = _conexion.ObtenerConexion();
        string query = "DELETE FROM tercero_telefono WHERE id = @id";
        
        using var cmd = new NpgsqlCommand(query, connection);
        cmd.Parameters.AddWithValue("@id", id);
        
        cmd.ExecuteNonQuery();
    }
}
