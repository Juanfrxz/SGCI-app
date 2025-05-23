using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SGCI_app.domain.Entities
{
    public class ThirdParty
    {
        public string? Id { get; set; }
        public string? Nombre { get; set; }
        public string? Apellidos { get; set; }
        public string? Email { get; set; }

        // Se correlaciona mediante LinQ
        public int TipoDoc_id { get; set; }
        public int TipoTercero_id { get; set; }
        public int Direccion_id { get; set; }
    }
}