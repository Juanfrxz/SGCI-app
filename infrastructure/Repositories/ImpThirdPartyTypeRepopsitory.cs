using SGCI_app.domain.Entities;
using SGCI_app.domain.Ports;
using SGCI_app.infrastructure.postgres;
using Npgsql;
using System.Collections.Generic;

namespace SGCI_app.infrastructure.Repositories;

public class ImpThirdPartyTypeRepopsitory : IGenericRepository<ThirdPartyType>, IThirdPartyTypeRepository
{
    private readonly ConexionSingleton _conexion;

    public ImpThirdPartyTypeRepopsitory(string connectionString)
    {
        _conexion = ConexionSingleton.Instancia(connectionString);
    }

    public List<ThirdPartyType> ObtenerTodos()
    {
        var types = new List<ThirdPartyType>();
        var connection = _conexion.ObtenerConexion();

        string query = "SELECT id, descripcion FROM tipo_terceros ORDER BY id ASC;";
        using var cmd = new NpgsqlCommand(query, connection);
        using var reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            types.Add(new ThirdPartyType
            {
                Id = reader.GetInt32(reader.GetOrdinal("id")),
                Descripcion = reader.GetString(reader.GetOrdinal("descripcion"))
            });
        }

        return types;
    }

    public void Crear(ThirdPartyType entity)
    {
        var connection = _conexion.ObtenerConexion();
        string query = "INSERT INTO tipo_tercero (descripcion) VALUES (@descripcion)";
        
        using var cmd = new NpgsqlCommand(query, connection);
        cmd.Parameters.AddWithValue("@descripcion", entity.Descripcion ?? (object)DBNull.Value);
        
        cmd.ExecuteNonQuery();
    }

    public void Actualizar(ThirdPartyType entity)
    {
        var connection = _conexion.ObtenerConexion();
        string query = "UPDATE tipo_tercero SET descripcion = @descripcion WHERE id = @id";
        
        using var cmd = new NpgsqlCommand(query, connection);
        cmd.Parameters.AddWithValue("@id", entity.Id);
        cmd.Parameters.AddWithValue("@descripcion", entity.Descripcion ?? (object)DBNull.Value);
        
        cmd.ExecuteNonQuery();
    }

    public void Eliminar(int id)
    {
        var connection = _conexion.ObtenerConexion();
        string query = "DELETE FROM tipo_tercero WHERE id = @id";
        
        using var cmd = new NpgsqlCommand(query, connection);
        cmd.Parameters.AddWithValue("@id", id);
        
        cmd.ExecuteNonQuery();
    }
}
