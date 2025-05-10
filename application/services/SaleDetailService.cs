using System;
using System.Collections.Generic;
using SGCI_app.domain.Entities;
using SGCI_app.domain.Ports;

namespace SGCI_app.application.services
{
    public class SaleDetailService
    {
        private readonly ISaleDetailRepository _repo;

        public SaleDetailService(ISaleDetailRepository repo)
        {
            _repo = repo;
        }

        public void MostrarTodos()
        {
            var lista = _repo.ObtenerTodos();
            foreach (var d in lista)
            {
                Console.WriteLine($"ID: {d.Id}, Factura ID: {d.FactId}, Producto ID: {d.Producto_Id}, Cantidad: {d.Cantidad}, Valor: {d.Valor}");
            }
        }

        public void CrearDetalleVenta(SaleDetail detalle)
        {
            _repo.Crear(detalle);
        }

        public void EliminarDetalleVenta(int id)
        {
            _repo.Eliminar(id);
        }

        public void ActualizarDetalleVenta(SaleDetail detalle)
        {
            _repo.Actualizar(detalle);
        }
    }
} 