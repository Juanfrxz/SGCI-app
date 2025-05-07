using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SGCI_app.domain.Entities
{
    public class City
    {
        public int Id { get; set; }
        public string? Nombre { get; set; }
        public int Region_Id { get; set; }
    }
}