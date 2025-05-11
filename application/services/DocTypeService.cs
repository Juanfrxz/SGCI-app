using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SGCI_app.domain.Entities;
using SGCI_app.domain.Ports;

namespace SGCI_app.application.services
{
    public class DocTypeService
    {
        private readonly IDocTypeRepository _repository;

        public DocTypeService(IDocTypeRepository repository)
        {
            _repository = repository;
        }

        public void CrearDocType(DocType docType)
        {
            _repository.Crear(docType);
        }

        public void ActualizarDocType(int id, DocType docType)
        {
            _repository.Actualizar(id, docType);
        }

        public void EliminarDocType(int id)
        {
            _repository.Eliminar(id);
        }

        public void MostrarTodos()
        {
            var lista = _repository.ObtenerTodos();
            foreach (var c in lista)
            {
                Console.WriteLine($"ID: {c.Id}, Nombre: {c.Descripcion}");
            }
        }
    }
}