using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SGCI_app.domain.Ports;
using SGCI_app.domain.Entities;

namespace SGCI_app.application.services;

public class CountryService
{
    private readonly ICountryRepository _repo;
    public CountryService(ICountryRepository repo)
    {
        _repo = repo;
    }

    public void MostrarTodos()
    {
        var lista = _repo.ObtenerTodos();
        foreach (var c in lista)
        {
            Console.WriteLine($"ID: {c.Id}, Nombre: {c.Nombre}");
        }
    }

    public void CrearCountry(Country country)
    {
        _repo.Crear(country);
    }

    public void EliminarCountry(int id)
    {
        _repo.Eliminar(id);
    }

    public void ActualizarCountry(Country country)
    {
        _repo.Actualizar(country);
    }
}
