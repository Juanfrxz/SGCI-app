using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SGCI_app.domain.Ports;
using SGCI_app.domain.Entities;

namespace SGCI_app.application.services;

public class CityService
{
    private readonly ICityRepository _repo;
    public CityService(ICityRepository repo)
    {
        _repo = repo;
    }

    public void MostrarTodos()
    {
        var lista = _repo.ObtenerTodos();
        foreach (var c in lista)
        {
            Console.WriteLine($"ID: {c.Id}, Nombre: {c.Nombre}, Region ID: {c.Region_Id}");
        }
    }

    public void CrearCity(City city)
    {
        _repo.Crear(city);
    }

    public void EliminarCity(int id)
    {
        _repo.Eliminar(id);
    }

    public void ActualizarCity(City city)
    {
        _repo.Actualizar(city);
    }
}
