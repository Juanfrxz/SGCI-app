using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SGCI_app.domain.DTO
{
    public class DtoCompany
    {
        public string? Id { get; set; }
        public string? Nombre { get; set; }
        public DtoAddress Address { get; set; } = new DtoAddress();
        public DateTime? FechaRegistro { get; set; }
    }
}