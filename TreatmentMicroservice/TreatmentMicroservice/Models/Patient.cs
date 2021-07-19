using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TreatmentMicroservice.Models
{
    public class Patient
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public string Ailment { get; set; }
        public string TreatmentPackageName { get; set; }
        public string CommencementDate { get; set; }
    }
}
