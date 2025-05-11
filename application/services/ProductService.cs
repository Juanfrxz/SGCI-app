using System;
using System.Collections.Generic;
using SGCI_app.domain.Entities;
using SGCI_app.domain.Ports;

namespace SGCI_app.application.services
{
    public class ProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public void GetAllProducts()
        {
            var lista = _productRepository.ObtenerTodos();
            foreach (var c in lista)
            {
                Console.WriteLine($"ID: {c.Id}, Nombre: {c.Nombre}, Stock: {c.Stock}, Fecha Actualizacion: {c.FechaActualizacion}");
            }
        }

        public void CreateProduct(Product product)
        {
            _productRepository.Crear(product);
        }

        public void UpdateProduct(Product product)
        {
            _productRepository.Actualizar(product);
        }

        public void DeleteProduct(string id)
        {
            _productRepository.Eliminar(id);
        }
    }
}
