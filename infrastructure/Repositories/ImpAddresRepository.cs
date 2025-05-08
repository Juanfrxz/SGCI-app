using SGCI_app.domain.Entities;
using SGCI_app.domain.Ports;
using System.Collections.Generic;
using SGCI_app.infrastructure.postgres;
using Npgsql;

namespace SGCI_app.infrastructure.Repositories;

public class ImpAddresRepository : IGenericRepository<Address>, IAddressRepository
{
    private readonly ConexionSingleton _conexion;

    public ImpAddresRepository(string connectionString)
    {
        _conexion = ConexionSingleton.Instancia(connectionString);
    }

    public List<Address> ObtenerTodos()
    {
        var addresses = new List<Address>();
        var connection = _conexion.ObtenerConexion();

        string query = "SELECT id, calle, numero_edificio, codigo_postal, ciudad_id, ifo_adicional FROM direcciones";
        using var cmd = new NpgsqlCommand(query, connection);
        using var reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            addresses.Add(new Address
            {
                Id = reader.GetInt32(reader.GetOrdinal("id")),
                Calle = reader.GetString(reader.GetOrdinal("calle")),
                NumeroEdificio = reader.GetString(reader.GetOrdinal("numero_edificio")),
                CodigoPostal = reader.GetString(reader.GetOrdinal("codigo_postal")),
                Ciudad_Id = reader.GetInt32(reader.GetOrdinal("ciudad_id")),
                InfoAdicional = reader.GetString(reader.GetOrdinal("ifo_adicional"))
            });
        }

        return addresses;
    }

    public void Crear(Address entity)
    {
        var connection = _conexion.ObtenerConexion();
        string query = "INSERT INTO direcciones (calle, numero_edificio, codigo_postal, ciudad_id, ifo_adicional) VALUES (@calle, @numero_edificio, @codigo_postal, @ciudad_id, @ifo_adicional)";
        
        using var cmd = new NpgsqlCommand(query, connection);
        cmd.Parameters.AddWithValue("@calle", entity.Calle ?? (object)DBNull.Value);
        cmd.Parameters.AddWithValue("@numero_edificio", entity.NumeroEdificio ?? (object)DBNull.Value);
        cmd.Parameters.AddWithValue("@codigo_postal", entity.CodigoPostal ?? (object)DBNull.Value);
        cmd.Parameters.AddWithValue("@ciudad_id", entity.Ciudad_Id);
        cmd.Parameters.AddWithValue("@ifo_adicional", entity.InfoAdicional ?? (object)DBNull.Value);
        
        cmd.ExecuteNonQuery();
    }

    public void Actualizar(Address entity)
    {
        var connection = _conexion.ObtenerConexion();
        string query = "UPDATE direcciones SET calle = @calle, numero_edificio = @numero_edificio, codigo_postal = @codigo_postal, ciudad_id = @ciudad_id, ifo_adicional = @ifo_adicional WHERE id = @id";
        
        using var cmd = new NpgsqlCommand(query, connection);
        cmd.Parameters.AddWithValue("@id", entity.Id);
        cmd.Parameters.AddWithValue("@calle", entity.Calle ?? (object)DBNull.Value);
        cmd.Parameters.AddWithValue("@numero_edificio", entity.NumeroEdificio ?? (object)DBNull.Value);
        cmd.Parameters.AddWithValue("@codigo_postal", entity.CodigoPostal ?? (object)DBNull.Value);
        cmd.Parameters.AddWithValue("@ciudad_id", entity.Ciudad_Id);
        cmd.Parameters.AddWithValue("@ifo_adicional", entity.InfoAdicional ?? (object)DBNull.Value);
        
        cmd.ExecuteNonQuery();
    }

    public void Eliminar(int id)
    {
        var connection = _conexion.ObtenerConexion();
        string query = "DELETE FROM direcciones WHERE id = @id";
        
        using var cmd = new NpgsqlCommand(query, connection);
        cmd.Parameters.AddWithValue("@id", id);
        cmd.ExecuteNonQuery();
    }
}
