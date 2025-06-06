using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SGCI_app.domain.Entities;

namespace SGCI_app.domain.DTO
{
    public class DtoEmployee
    {
        public int Id { get; set; }
        public string? Nombre { get; set; }
        public string? Apellidos { get; set; }
        public string? Email { get; set; }

        // Se correlaciona mediante LinQ
        public int TipoDoc_id { get; set; }
        public int TipoTercero_id { get; set; }

        public string? Tercero_Id { get; set; }

        public DtoAddress Address { get; set; } = new DtoAddress();
        public DtoEmp Employee { get; set; } = new DtoEmp();
    }
}