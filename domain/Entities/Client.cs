using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SGCI_app.domain.Entities
{
    public class Client
    {
        public int Id { get; set; }
        public DateTime? FechaNacimiento { get; set; }
        public DateTime? FechaUltimaCompra { get; set; }
    }
}