using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SGCI_app.domain.Entities
{
    public class PromotionalPlan
    {
        public int Id { get; set; }
        public string? Nombre { get; set; }
        public DateTime? Inicio { get; set; }
        public DateTime? Fin { get; set; }
        public double Descuento { get; set; }
    }
}