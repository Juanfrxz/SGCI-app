using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SGCI_app.domain.Entities
{
    public class Employee : ThirdParty
    {
        public int Id { get; set; }
        public DateTime? FechaIngreso { get; set; }
        public double SalarioBase { get; set; }
        public int Eps_id { get; set; }
        public int Arl_id { get; set; }

        public Employee(string nombre, string apellidos, string email, int tipodoc_id, int tipotercero_id, int direccion_id, DateTime? fechaingreso, double salariobase, int eps_id, int arl_id) : base(nombre, apellidos, email, tipodoc_id, tipotercero_id, direccion_id)
        {
            FechaIngreso = fechaingreso;
            SalarioBase = salariobase;
            Eps_id = eps_id;
            Arl_id = arl_id;
        }
    }
}