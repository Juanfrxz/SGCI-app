using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SGCI_app.domain.Entities
{
    public class SaleDetail
    {
        public int Id { get; set; }
        public int FactId { get; set; }
        public string? Producto_Id { get; set; }
        public int Cantidad { get; set; }
        public double Valor { get; set; }
    }
}