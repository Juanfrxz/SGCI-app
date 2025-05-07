using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SGCI_app.domain.Entities
{
    public class Provider : ThirdParty
    {
        public int Id { get; set; }
        public double Descuento { get; set; }
        public int DiaPago { get; set; }

        public Provider(string nombre, string apellidos, string email, int tipodoc_id, int tipotercero_id, int direccion_id, double descuento, int diapago) : base(nombre, apellidos, email, tipodoc_id, tipotercero_id, direccion_id)
        {
            Descuento = descuento;
            DiaPago = diapago;
        }
    }
}