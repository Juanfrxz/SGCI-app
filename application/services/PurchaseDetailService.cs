using System;
using System.Collections.Generic;
using SGCI_app.domain.Entities;
using SGCI_app.domain.Ports;

namespace SGCI_app.application.services
{
    public class PurchaseDetailService
    {
        private readonly IPurchaseDetailRepository _repo;

        public PurchaseDetailService(IPurchaseDetailRepository repo)
        {
            _repo = repo;
        }

        public void MostrarTodos()
        {
            var lista = _repo.ObtenerTodos();
            foreach (var d in lista)
            {
                Console.WriteLine($"ID: {d.Id}, Fecha: {d.Fecha}, Producto ID: {d.Producto_Id}, Cantidad: {d.Cantidad}, Valor: {d.Valor}, Compra ID: {d.Compra_Id}");
            }
        }

        public void CrearDetalleCompra(PurchaseDetail detalle)
        {
            _repo.Crear(detalle);
        }

        public void EliminarDetalleCompra(int id)
        {
            _repo.Eliminar(id);
        }

        public void ActualizarDetalleCompra(PurchaseDetail detalle)
        {
            _repo.Actualizar(detalle);
        }
    }
} 