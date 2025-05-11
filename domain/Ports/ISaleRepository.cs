using System;
using SGCI_app.domain.Entities;
using Npgsql;

namespace SGCI_app.domain.Ports
{
    public interface ISaleRepository : IGenericRepository<Sale>
    {
        NpgsqlConnection GetConnection();
        decimal ObtenerTotalVenta(int factId);
        int ObtenerCantidadDetalles(int factId);
    }
}