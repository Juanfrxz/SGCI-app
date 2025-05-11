using System;
using System.Collections.Generic;
using SGCI_app.domain.Entities;
using SGCI_app.domain.Ports;

namespace SGCI_app.application.Services
{
    public class ProductSupplierService
    {
        private readonly IProductSupplierRepository _repository;

        public ProductSupplierService(IProductSupplierRepository repository)
        {
            _repository = repository;
        }

        public void CrearProductSupplier(ProductSupplier entity)
        {
            _repository.Crear(entity);
        }

        public void ActualizarProductSupplier(string oldTerceroId, string oldProductoId, ProductSupplier entity)
        {
            _repository.Actualizar(oldTerceroId, oldProductoId, entity);
        }

        public void EliminarProductSupplier(string terceroId, string productoId)
        {
            _repository.Eliminar(terceroId, productoId);
        }

        public void MostrarTodos()
        {
            var lista = _repository.ObtenerTodos();
            foreach (var item in lista)
            {
                Console.WriteLine($"Proveedor ID: {item.Tercero_Id}, Producto ID: {item.Producto_Id}");
            }
        }
    }
}