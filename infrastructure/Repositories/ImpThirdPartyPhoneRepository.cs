using SGCI_app.domain.Entities;
using SGCI_app.domain.Ports;
using SGCI_app.infrastructure.postgres;
using Npgsql;
using System.Collections.Generic;

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

        const string query = @"
SELECT id,
       tercero_id,
       telefono,
       tipo_telefono_id
FROM tercero_telefono;
";
        using var cmd = new NpgsqlCommand(query, connection);
        using var reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            thirdPartyPhones.Add(new ThirdPartyPhone
            {
                Id = reader.GetInt32(reader.GetOrdinal("id")),
                Tercer_Id = reader.GetString(reader.GetOrdinal("tercero_id")),
                Numero = reader.GetString(reader.GetOrdinal("telefono")),
                Tipo_Telefono_Id = reader.GetInt32(reader.GetOrdinal("tipo_telefono_id"))
            });
        }

        return thirdPartyPhones;
    }

    public void Crear(ThirdPartyPhone entity)
    {
        var connection = _conexion.ObtenerConexion();
        const string query = @"
INSERT INTO tercero_telefono (tercero_id, telefono, tipo_telefono_id)
VALUES (@tercero_id, @telefono, @tipo_telefono_id);
";
        using var cmd = new NpgsqlCommand(query, connection);
        cmd.Parameters.AddWithValue("@tercero_id", entity.Tercer_Id ?? (object) DBNull.Value);
        cmd.Parameters.AddWithValue("@telefono",  entity.Numero ?? (object) DBNull.Value);
        cmd.Parameters.AddWithValue("@tipo_telefono_id", entity.Tipo_Telefono_Id);
        cmd.ExecuteNonQuery();
    }

    public void Actualizar(ThirdPartyPhone entity)
    {
        var connection = _conexion.ObtenerConexion();
        const string query = @"
UPDATE tercero_telefono
SET tercero_id         = @tercero_id,
    telefono           = @telefono,
    tipo_telefono_id   = @tipo_telefono_id
WHERE id = @id;
";
        using var cmd = new NpgsqlCommand(query, connection);
        cmd.Parameters.AddWithValue("@id", entity.Id);
        cmd.Parameters.AddWithValue("@tercero_id", entity.Tercer_Id ?? (object) DBNull.Value);
        cmd.Parameters.AddWithValue("@telefono", entity.Numero ?? (object) DBNull.Value);
        cmd.Parameters.AddWithValue("@tipo_telefono_id", entity.Tipo_Telefono_Id);
        var rows = cmd.ExecuteNonQuery();
        if (rows == 0)
            throw new InvalidOperationException($"No se encontró el registro con id={entity.Id} para actualizar.");
    }

    public void Eliminar(int id)
    {
        var connection = _conexion.ObtenerConexion();
        const string query = @"
DELETE FROM tercero_telefono
WHERE id = @id;
";
        using var cmd = new NpgsqlCommand(query, connection);
        cmd.Parameters.AddWithValue("@id", id);
        var rows = cmd.ExecuteNonQuery();
        if (rows == 0)
            throw new InvalidOperationException($"No se encontró el registro con id={id} para eliminar.");
    }
}