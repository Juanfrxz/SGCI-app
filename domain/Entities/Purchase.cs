using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SGCI_app.domain.Entities
{
    public class Purchase
    {
        public int Id { get; set; }
        public string? TerceroProveedor_Id { get; set; }
        public DateTime? Fecha { get; set; }
        public string? TerceroEmpleado_Id { get; set; }
        public string? DocCompra { get; set; }
    }
}