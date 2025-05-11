using System;
using System.Collections.Generic;
using SGCI_app.domain.Entities;
using SGCI_app.domain.Ports;
using Npgsql;

namespace SGCI_app.application.services
{
    public class SaleService
    {
        private readonly ISaleRepository _repo;

        public SaleService(ISaleRepository repo)
        {
            _repo = repo;
        }

        public void MostrarTodos()
        {
            var lista = _repo.ObtenerTodos();
            if (lista.Count == 0)
            {
                Console.WriteLine("No hay ventas registradas.");
                return;
            }

            Console.WriteLine("\n{0,-8} {1,-12} {2,-20} {3,-20} {4,-15}", 
                "ID", "Fecha", "Empleado", "Cliente", "Total");
            Console.WriteLine(new string('-', 75));

            foreach (var v in lista)
            {
                string fecha = v.Fecha?.ToString("dd/MM/yyyy") ?? "N/A";
                string empleado = v.TerceroEmpleado_Id ?? "N/A";
                string cliente = v.TerceroCliente_Id ?? "N/A";
                
                decimal total = _repo.ObtenerTotalVenta(v.FactId);
                
                Console.WriteLine("{0,-8} {1,-12} {2,-20} {3,-20} {4,-15:C}", 
                    v.FactId, fecha, empleado, cliente, total);
            }
        }

        public void CrearVenta(Sale venta)
        {
            _repo.Crear(venta);
        }

        public void EliminarVenta(int id)
        {
            int detallesCount = _repo.ObtenerCantidadDetalles(id);
            if (detallesCount > 0)
            {
                throw new Exception($"No se puede eliminar la venta porque tiene {detallesCount} detalles asociados. Elimine primero los detalles.");
            }
            
            _repo.Eliminar(id);
        }

        public void ActualizarVenta(Sale venta)
        {
            _repo.Actualizar(venta);
        }
    }
} 