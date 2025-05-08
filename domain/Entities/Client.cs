using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SGCI_app.domain.Entities
{
    public class Client : ThirdParty
    {
        public int Id { get; set; }
        public DateTime? FechaNacimiento { get; set; }
        public DateTime? FechaUltimaCompra { get; set; }

        public Client(string nombre, string apellidos, string email, int tipodoc_id, int tipotercero_id, int direccion_id, DateTime? fechanacimiento, DateTime? fechaultimacompra) : base(nombre, apellidos, email, tipodoc_id, tipotercero_id, direccion_id)
        {
            FechaNacimiento = fechanacimiento;
            FechaUltimaCompra = fechaultimacompra;
        }
    }
}