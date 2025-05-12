using SGCI_app.domain.Entities;
using SGCI_app.domain.Ports;
using SGCI_app.infrastructure.postgres;
using Npgsql;
using System;

namespace SGCI_app.infrastructure.Repositories;

public class ImpPurchaseRepository : IGenericRepository<Purchase>, IPurchaseRepository
{
    private readonly ConexionSingleton _conexion;

    public ImpPurchaseRepository(string connectionString)
    {
        _conexion = ConexionSingleton.Instancia(connectionString);
    }

    public List<Purchase> ObtenerTodos()
    {
        try
        {
            var purchases = new List<Purchase>();
            var connection = _conexion.ObtenerConexion();

            string query = "SELECT c.id, c.proveedor_id, c.fecha, c.empleado_id, c.doc_compra, " +
                          "p.tercero_id as proveedor_tercero_id, e.tercero_id as empleado_tercero_id " +
                          "FROM compras c " +
                          "JOIN proveedor p ON c.proveedor_id = p.id " +
                          "JOIN empleado e ON c.empleado_id = e.id " +
                          "ORDER BY c.id ASC;";
            using var cmd = new NpgsqlCommand(query, connection);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                purchases.Add(new Purchase
                {
                    Id = reader.GetInt32(reader.GetOrdinal("id")),
                    TerceroProveedor_Id = reader.GetString(reader.GetOrdinal("proveedor_tercero_id")),
                    Fecha = reader.GetDateTime(reader.GetOrdinal("fecha")),
                    TerceroEmpleado_Id = reader.GetString(reader.GetOrdinal("empleado_tercero_id")),
                    DocCompra = reader.GetString(reader.GetOrdinal("doc_compra"))
                });
            }

            return purchases;
        }
        catch (PostgresException ex)
        {
            switch (ex.SqlState)
            {
                case "42P01": // relation does not exist
                    throw new Exception("La tabla de compras no existe en la base de datos.");
                case "28P01": // password authentication failed
                    throw new Exception("Error de autenticación con la base de datos. Verifique las credenciales.");
                default:
                    throw new Exception($"Error de base de datos: {ex.Message}");
            }
        }
    }

    public void Crear(Purchase entity)
    {
        try
        {
            var connection = _conexion.ObtenerConexion();

            // Primero obtener los IDs de proveedor y empleado
            string queryProveedor = "SELECT id FROM proveedor WHERE id = @id";
            string queryEmpleado = "SELECT id FROM empleado WHERE id = @id";

            int proveedorId = 0;
            int empleadoId = 0;

            using (var cmd = new NpgsqlCommand(queryProveedor, connection))
            {
                cmd.Parameters.AddWithValue("@id", Convert.ToInt32(entity.TerceroProveedor_Id));
                var result = cmd.ExecuteScalar();
                if (result == null || result == DBNull.Value)
                {
                    throw new Exception($"No se encontró un proveedor con el ID: {entity.TerceroProveedor_Id}");
                }
                proveedorId = Convert.ToInt32(result);
            }

            using (var cmd = new NpgsqlCommand(queryEmpleado, connection))
            {
                cmd.Parameters.AddWithValue("@id", Convert.ToInt32(entity.TerceroEmpleado_Id));
                var result = cmd.ExecuteScalar();
                if (result == null || result == DBNull.Value)
                {
                    throw new Exception($"No se encontró un empleado con el ID: {entity.TerceroEmpleado_Id}");
                }
                empleadoId = Convert.ToInt32(result);
            }

            // Ahora insertar la compra
            string query = "INSERT INTO compras (proveedor_id, fecha, empleado_id, doc_compra) VALUES (@proveedor_id, @fecha, @empleado_id, @doc_compra)";

            using var cmdInsert = new NpgsqlCommand(query, connection);
            cmdInsert.Parameters.AddWithValue("@proveedor_id", proveedorId);
            cmdInsert.Parameters.AddWithValue("@fecha", entity.Fecha ?? (object)DBNull.Value);
            cmdInsert.Parameters.AddWithValue("@empleado_id", empleadoId);
            cmdInsert.Parameters.AddWithValue("@doc_compra", entity.DocCompra ?? (object)DBNull.Value);

            cmdInsert.ExecuteNonQuery();
        }
        catch (PostgresException ex)
        {
            switch (ex.SqlState)
            {
                case "42P01": // relation does not exist
                    throw new Exception("La tabla de compras no existe en la base de datos.");
                case "28P01": // password authentication failed
                    throw new Exception("Error de autenticación con la base de datos. Verifique las credenciales.");
                case "42804": // column type mismatch
                    throw new Exception("Error de tipo de datos en la base de datos.");
                case "P0001": // custom exception from trigger
                    throw new Exception(ex.Message);
                default:
                    throw new Exception($"Error de base de datos: {ex.Message}");
            }
        }
    }

    public void Actualizar(Purchase entity)
    {
        try
        {
            var connection = _conexion.ObtenerConexion();
            
            // Primero obtener los IDs de proveedor y empleado
            string queryProveedor = "SELECT id FROM proveedor WHERE tercero_id = @tercero_id";
            string queryEmpleado = "SELECT id FROM empleado WHERE tercero_id = @tercero_id";
            
            int proveedorId = 0;
            int empleadoId = 0;

            using (var cmd = new NpgsqlCommand(queryProveedor, connection))
            {
                cmd.Parameters.AddWithValue("@tercero_id", entity.TerceroProveedor_Id ?? (object)DBNull.Value);
                var result = cmd.ExecuteScalar();
                if (result == null || result == DBNull.Value)
                {
                    throw new Exception($"No se encontró un proveedor con el ID: {entity.TerceroProveedor_Id}");
                }
                proveedorId = Convert.ToInt32(result);
            }

            using (var cmd = new NpgsqlCommand(queryEmpleado, connection))
            {
                cmd.Parameters.AddWithValue("@tercero_id", entity.TerceroEmpleado_Id ?? (object)DBNull.Value);
                var result = cmd.ExecuteScalar();
                if (result == null || result == DBNull.Value)
                {
                    throw new Exception($"No se encontró un empleado con el ID: {entity.TerceroEmpleado_Id}");
                }
                empleadoId = Convert.ToInt32(result);
            }

            string query = "UPDATE compras SET proveedor_id = @proveedor_id, fecha = @fecha, empleado_id = @empleado_id, doc_compra = @doc_compra WHERE id = @id";
            
            using var cmdUpdate = new NpgsqlCommand(query, connection);
            cmdUpdate.Parameters.AddWithValue("@id", entity.Id);
            cmdUpdate.Parameters.AddWithValue("@proveedor_id", proveedorId);
            cmdUpdate.Parameters.AddWithValue("@fecha", entity.Fecha ?? (object)DBNull.Value);
            cmdUpdate.Parameters.AddWithValue("@empleado_id", empleadoId);
            cmdUpdate.Parameters.AddWithValue("@doc_compra", entity.DocCompra ?? (object)DBNull.Value);
            
            cmdUpdate.ExecuteNonQuery();
        }
        catch (PostgresException ex)
        {
            switch (ex.SqlState)
            {
                case "42P01": // relation does not exist
                    throw new Exception("La tabla de compras no existe en la base de datos.");
                case "28P01": // password authentication failed
                    throw new Exception("Error de autenticación con la base de datos. Verifique las credenciales.");
                case "42804": // column type mismatch
                    throw new Exception("Error de tipo de datos en la base de datos.");
                case "P0001": // custom exception from trigger
                    throw new Exception(ex.Message);
                default:
                    throw new Exception($"Error de base de datos: {ex.Message}");
            }
        }
    }

    public void Eliminar(int id)
    {
        try
        {
            var connection = _conexion.ObtenerConexion();
            string query = "DELETE FROM compras WHERE id = @id";
            
            using var cmd = new NpgsqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@id", id);
            
            cmd.ExecuteNonQuery();
        }
        catch (PostgresException ex)
        {
            switch (ex.SqlState)
            {
                case "42P01": // relation does not exist
                    throw new Exception("La tabla de compras no existe en la base de datos.");
                case "28P01": // password authentication failed
                    throw new Exception("Error de autenticación con la base de datos. Verifique las credenciales.");
                default:
                    throw new Exception($"Error de base de datos: {ex.Message}");
            }
        }
    }
} 