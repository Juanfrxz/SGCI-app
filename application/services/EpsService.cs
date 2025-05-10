using System;
using System.Collections.Generic;
using SGCI_app.domain.Entities;
using SGCI_app.domain.Ports;

namespace SGCI_app.application.services
{
    public class EpsService
    {
        private readonly IEpsRepository _epsRepository;

        public EpsService(IEpsRepository epsRepository)
        {
            _epsRepository = epsRepository;
        }

        public void CrearEps(EPS eps)
        {
            _epsRepository.Crear(eps);
        }

        public void ActualizarEps(int id, EPS eps)
        {
            _epsRepository.Actualizar(id, eps);
        }

        public void EliminarEps(int id)
        {
            _epsRepository.Eliminar(id);
        }

        public void ObtenerTodasEps()
        {
            var lista = _epsRepository.ObtenerTodos();
            foreach (var c in lista)
            {
                Console.WriteLine($"ID: {c.Id}, EPS: {c.Nombre}");
            }
        }
    }
} 