using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SGCI_app.domain.Entities
{
    public class Provider
    {
        public int Id { get; set; }
        public double Descuento { get; set; }
        public int DiaPago { get; set; }
    }
}