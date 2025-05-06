using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SGCI_app.domain.Entities
{
    public class Employee : ThirdParty
    {
        /*Duda: se debe poder ingresar toda la informacion (nombre, apellido... y la info especifica de la clase) o primero debe hacerse el registro de ThirdParty
        y luego pasarle el id de este*/
        // Se correlaciona mediante LinQ
        public string? Tercero_id {get; set;}

        public DateTime? FechaIngreso {get; set;}
        public double SalarioBase {get; set;}

        // Se correlaciona mediante LinQ
        public int Eps_id {get; set;}
        public int Arl_id {get; set;}
    }
}