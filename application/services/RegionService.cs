using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Org.BouncyCastle.Asn1.Misc;
using SGCI_app.domain.DTO;
using SGCI_app.domain.Entities;
using SGCI_app.domain.Ports;

namespace SGCI_app.application.services;

public class RegionService
{
    private readonly IRegionRepository _repo;
    public RegionService(IRegionRepository repo)
    {
        _repo = repo;
    }
    public void MostrarTodos()
    {
        var lista = _repo.ObtenerTodos();
        foreach (var r in lista)
        {
            Console.WriteLine($"ID: {r.Id}, Nombre: {r.Nombre}");
        }
    }
    public void CrearRegion(Region region)
    {
        _repo.Crear(region);
    }
    public void EliminarRegion(int id)
    {
        _repo.Eliminar(id);
    }
    public void ActualuizarRegion (Region region)
    {
        _repo.Actualizar(region);
    }
}
