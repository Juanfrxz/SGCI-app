using System;
using System.Collections.Generic;
using SGCI_app.domain.Entities;
using SGCI_app.domain.Ports;

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
            foreach (var v in lista)
            {
                Console.WriteLine($"ID: {v.FactId}, Fecha: {v.Fecha}, Empleado ID: {v.TerceroEmpleado_Id}, Cliente ID: {v.TerceroCliente_Id}");
            }
        }

        public void CrearVenta(Sale venta)
        {
            _repo.Crear(venta);
        }

        public void EliminarVenta(int id)
        {
            _repo.Eliminar(id);
        }

        public void ActualizarVenta(Sale venta)
        {
            _repo.Actualizar(venta);
        }
    }
} 