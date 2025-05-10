using SGCI_app.domain.Entities;
using SGCI_app.domain.Ports;
using SGCI_app.infrastructure.postgres;
using Npgsql;

namespace SGCI_app.infrastructure.Repositories;

public class ImpRegionRepository : IGenericRepository<Region>, IRegionRepository
{
    private readonly ConexionSingleton _conexion;

    public ImpRegionRepository(string connectionString)
    {
        _conexion = ConexionSingleton.Instancia(connectionString);
    }

    public List<Region> ObtenerTodos()
    {
        var regiones = new List<Region>();
        var connection = _conexion.ObtenerConexion();

        string query = "SELECT id, nombre FROM region";
        using var cmd = new NpgsqlCommand(query, connection);
        using var reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            regiones.Add(new Region
            {
                Id = reader.GetInt32(reader.GetOrdinal("id")),
                Nombre = reader.GetString(reader.GetOrdinal("nombre"))
            });
        }
        return regiones;
    }

    public void Crear(Region entity)
    {
        var connection = _conexion.ObtenerConexion();
        var sql = @"CALL public.sp_create_region(
                        p_nombre := @p_nombre,
                        p_pais_id := @p_pais_id
                    );";

        using var cmd = new NpgsqlCommand(sql, connection)
        {
            CommandType = System.Data.CommandType.Text
        };
        cmd.Parameters.AddWithValue("@p_nombre", entity.Nombre ?? (object)DBNull.Value);
        cmd.Parameters.AddWithValue("@p_pais_id", entity.Pais_Id);

        cmd.ExecuteNonQuery();
    }

    public void Actualizar(Region entity)
    {
        var connection = _conexion.ObtenerConexion();
        var sql = @"CALL public.sp_update_region(
                        p_id := @p_id,
                        p_nombre := @p_nombre,
                        p_pais_id := @p_pais_id
                    );";

        using var cmd = new NpgsqlCommand(sql, connection)
        {
            CommandType = System.Data.CommandType.Text
        };
        cmd.Parameters.AddWithValue("@p_id", entity.Id);
        cmd.Parameters.AddWithValue("@p_nombre", entity.Nombre ?? (object)DBNull.Value);
        cmd.Parameters.AddWithValue("@p_pais_id", entity.Pais_Id);

        cmd.ExecuteNonQuery();
    }

    public void Eliminar(int id)
    {
        var connection = _conexion.ObtenerConexion();
        string query = "DELETE FROM region WHERE id = @id";

        using var cmd = new NpgsqlCommand(query, connection);
        cmd.Parameters.AddWithValue("@id", id);

        cmd.ExecuteNonQuery();
    }
}
