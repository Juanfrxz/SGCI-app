using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SGCI_app.domain.Entities
{
    public class CashSession
    {
        public int Id { get; set; }
        public DateTime? AperturaCaja { get; set; }
        public DateTime? CierreCaja { get; set; }
        public int BalanceApertura { get; set; }
        public int BalaceCierre { get; set; }
    }
}