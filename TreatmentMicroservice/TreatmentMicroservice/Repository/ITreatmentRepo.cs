using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TreatmentMicroservice.Models;

namespace TreatmentMicroservice.Repository
{
    public interface ITreatmentRepo
    {
        public Task<List<PackageDetail>> GetPackageList();
        public Task<List<SpecialistDetail>> GetSpecialists();
        public Task<bool> SaveAll(Patient patientDetails, TreatmentPlan plan);
       public Task<TreatmentPlan> GeneratePlan(Patient patientDetails);
    }
}
