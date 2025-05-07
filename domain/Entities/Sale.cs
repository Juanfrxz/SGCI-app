using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SGCI_app.domain.Entities
{
    public class Sale
    {
        public int FactId { get; set; }
        public DateTime? Fecha { get; set; }
        public string? TerceroEmpleado_Id { get; set; }
        public string? TerceroCliente_Id { get; set; }
    }
}