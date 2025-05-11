using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SGCI_app.domain.Entities
{
    public class ThirdPartyPhone
    {
        public int Id { get; set; }
        public string? Numero { get; set; }
        public string? Tercer_Id { get; set; }
        public int Tipo_Telefono_Id { get; set; }
    }
}