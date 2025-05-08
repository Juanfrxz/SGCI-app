using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SGCI_app.domain.DTO
{
    public class DtoEmp
    {
        public int Id { get; set; }
        public DateTime? FechaIngreso { get; set; }
        public double SalarioBase { get; set; }
        public int Eps_id { get; set; }
        public int Arl_id { get; set; }
    }
}