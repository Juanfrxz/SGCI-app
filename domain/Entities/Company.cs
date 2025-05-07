using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SGCI_app.domain.Entities
{
    public class Company
    {
        public string? Id { get; set; }
        public string? Nombre { get; set; }
        public int Direccion_Id { get; set; }
        public DateTime? FechaRegistro { get; set; }
    }
}