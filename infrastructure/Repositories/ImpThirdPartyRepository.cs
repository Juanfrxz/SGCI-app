
using SGCI_app.domain.Entities;
using SGCI_app.domain.Ports;
using SGCI_app.infrastructure.postgres;
using Npgsql;

namespace SGCI_app.infrastructure.Repositories;

public class ImpThirdPartyRepository : IGenericRepository<ThirdParty>, IThirdPartyRepository
{
    private readonly ConexionSingleton _conexion;

    public ImpThirdPartyRepository(string connectionString)
    {
        _conexion = ConexionSingleton.Instancia(connectionString);
    }

    public List<ThirdParty> ObtenerTodos()
    {
        var thirdParties = new List<ThirdParty>();
        var connection = _conexion.ObtenerConexion();

        string query = "SELECT id, nombre, apellidos, email, tipo_doc_id, tipo_tercero_id, direccion_id FROM terceros";
        using var cmd = new NpgsqlCommand(query, connection);
        using var reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            thirdParties.Add(new ThirdParty
            {
                Id = reader.GetString(reader.GetOrdinal("id")),
                Nombre = reader.GetString(reader.GetOrdinal("nombre")),
                Apellidos = reader.GetString(reader.GetOrdinal("apellidos")),
                Email = reader.GetString(reader.GetOrdinal("email")),
                TipoDoc_id = reader.GetInt32(reader.GetOrdinal("tipo_doc_id")),
                TipoTercero_id = reader.GetInt32(reader.GetOrdinal("tipo_tercero_id")),
                Direccion_id = reader.GetInt32(reader.GetOrdinal("direccion_id"))
            });
        }

        return thirdParties;
    }

    public void Crear(ThirdParty entity)
    {
        var connection = _conexion.ObtenerConexion();
        string query = "INSERT INTO terceros (id, nombre, apellidos, email, tipo_doc_id, tipo_tercero_id, direccion_id) VALUES (@id, @nombre, @apellidos, @email, @tipo_doc_id, @tipo_tercero_id, @direccion_id)";
        
        using var cmd = new NpgsqlCommand(query, connection);
        cmd.Parameters.AddWithValue("@id", entity.Id ?? (object)DBNull.Value);
        cmd.Parameters.AddWithValue("@nombre", entity.Nombre ?? (object)DBNull.Value);
        cmd.Parameters.AddWithValue("@apellidos", entity.Apellidos ?? (object)DBNull.Value);
        cmd.Parameters.AddWithValue("@email", entity.Email ?? (object)DBNull.Value);
        cmd.Parameters.AddWithValue("@tipo_doc_id", entity.TipoDoc_id);
        cmd.Parameters.AddWithValue("@tipo_tercero_id", entity.TipoTercero_id);
        cmd.Parameters.AddWithValue("@direccion_id", entity.Direccion_id);
        
        cmd.ExecuteNonQuery();
    }

    public void Actualizar(ThirdParty entity)
    {
        var connection = _conexion.ObtenerConexion();
        string query = "UPDATE terceros SET nombre = @nombre, apellidos = @apellidos, email = @email, tipo_doc_id = @tipo_doc_id, tipo_tercero_id = @tipo_tercero_id, direccion_id = @direccion_id WHERE id = @id";
        
        using var cmd = new NpgsqlCommand(query, connection);
        cmd.Parameters.AddWithValue("@id", entity.Id ?? (object)DBNull.Value);
        cmd.Parameters.AddWithValue("@nombre", entity.Nombre ?? (object)DBNull.Value);
        cmd.Parameters.AddWithValue("@apellidos", entity.Apellidos ?? (object)DBNull.Value);
        cmd.Parameters.AddWithValue("@email", entity.Email ?? (object)DBNull.Value);
        cmd.Parameters.AddWithValue("@tipo_doc_id", entity.TipoDoc_id);
        cmd.Parameters.AddWithValue("@tipo_tercero_id", entity.TipoTercero_id);
        cmd.Parameters.AddWithValue("@direccion_id", entity.Direccion_id);
        
        cmd.ExecuteNonQuery();
    }

    public void Eliminar(int id)
    {
        var connection = _conexion.ObtenerConexion();
        string query = "DELETE FROM terceros WHERE id = @id";
        
        using var cmd = new NpgsqlCommand(query, connection);
        cmd.Parameters.AddWithValue("@id", id.ToString());
        cmd.ExecuteNonQuery();
    }
}

