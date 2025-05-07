using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SGCI_app.domain.Entities
{
    public class CashMovement
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public int TipoMovimiento_Id { get; set; }
        public int Valor { get; set; }
        public string? Concepto { get; set; }
        public string? Tercero_Id { get; set; }
        public int Sesion_Id { get; set; }
    }
}