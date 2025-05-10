using System;
using System.Collections.Generic;
using SGCI_app.domain.Entities;
using SGCI_app.domain.Ports;

namespace SGCI_app.application.services
{
    public class PurchaseService
    {
        private readonly IPurchaseRepository _repo;

        public PurchaseService(IPurchaseRepository repo)
        {
            _repo = repo;
        }

        public void MostrarTodos()
        {
            var lista = _repo.ObtenerTodos();
            foreach (var c in lista)
            {
                Console.WriteLine($"ID: {c.Id}, Proveedor ID: {c.TerceroProveedor_Id}, Fecha: {c.Fecha}, Empleado ID: {c.TerceroEmpleado_Id}, Doc Compra: {c.DocCompra}");
            }
        }

        public void CrearCompra(Purchase compra)
        {
            _repo.Crear(compra);
        }

        public void EliminarCompra(int id)
        {
            _repo.Eliminar(id);
        }

        public void ActualizarCompra(Purchase compra)
        {
            _repo.Actualizar(compra);
        }
    }
} 