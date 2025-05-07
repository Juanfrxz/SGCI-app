using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SGCI_app.domain.Entities
{
    public class Billing
    {
        public int Id { get; set; }
        public DateTime? FechaResolucion { get; set; }
        public int NumInicio { get; set; }
        public int NumFinal { get; set; }
        public int FacturaActual { get; set; }
    }
}