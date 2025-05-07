using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SGCI_app.domain.Entities
{
    public class Address
    {
        public int Id { get; set; }
        public string? Calle { get; set; }
        public string? NumeroEdificio { get; set; }
        public string? CodigoPostal { get; set; }
        public int Ciudad_Id { get; set; }
        public string InfoAdicional { get; set; }
    }
}