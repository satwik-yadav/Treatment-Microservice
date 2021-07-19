using log4net;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using TreatmentMicroservice.Data;
using TreatmentMicroservice.Models;
namespace TreatmentMicroservice.Repository
{
    public class TreatmentRepo:ITreatmentRepo
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private TreatmentContext _context;
        private HttpClient _client;
        private HttpResponseMessage _response;
        public TreatmentRepo(TreatmentContext context)
        {
            this._context = context;
            if (_context.Plans.Any())
            {
                return;
            }

            _context.Plans.AddRange(
                  new TreatmentPlan{PlanId = 1,AilmentName = "Orthopaedics",PackageName = "Package 1",TestDetails = "OPT1,OPT2",Cost = 2500,
                      SpecialistName = "Shriny",
                      
                  },
                  new TreatmentPlan{PlanId = 2,AilmentName = "Orthopaedics",PackageName = "Package 2",
                      TestDetails = "OPT3,OPT4",  Cost = 3000,
                      SpecialistName = "Joey",
                      
                  },
                    new TreatmentPlan
                    {
                        PlanId = 3,
                        AilmentName = "Urology",
                        PackageName = "Package 1",
                        TestDetails = "UPT1,UPT2",
                        Cost = 4000,
                        SpecialistName = "Chandler",
                       
                    },

                    new TreatmentPlan
                    {
                        PlanId = 4,
                        AilmentName = "Urology",
                        PackageName = "Package 2",
                        TestDetails = "UPT3,UPT4",
                        Cost = 5000,
                        SpecialistName = "Rachel",
                        
                    }
                 );

            _context.SaveChanges();
            _client = new HttpClient();
            _client.BaseAddress = new Uri(@"https://localhost:44375/");
        }
        public async Task<List<PackageDetail>> GetPackageList()
        {
            log.Info("Pateinsts Package List from Offering Microservice is being invoked");
            List<PackageDetail> packageList;
            _response = new HttpResponseMessage();
            _response = _client.GetAsync("api/offering/IpTreatmentPackages").Result;
            string apiResponse = await _response.Content.ReadAsStringAsync();
            packageList = JsonConvert.DeserializeObject<List<PackageDetail>>(apiResponse);
            return packageList;
        }
        public async Task<List<SpecialistDetail>> GetSpecialists()
        {
            log.Info("Pateinsts Specialist List from Offering Microservice is being invoked");
            List<SpecialistDetail> specialists;
            _response = new HttpResponseMessage();
            _response = _client.GetAsync("api/offering/Specialists").Result;
            string apiResponse = await _response.Content.ReadAsStringAsync();
            specialists = JsonConvert.DeserializeObject<List<SpecialistDetail>>(apiResponse);
            return specialists;
        }
        public async Task<bool> SaveAll(Patient patientDetails, TreatmentPlan plan)
        {
            log.Info("Patient details along with plan is being saved");
            try
            {
                _context.Patients.Add(patientDetails);
                _context.Plans.Add(plan);
                await Task.Run(() => _context.SaveChanges());
                return true;
            }
            catch (Exception ex)
            {
                log.Error("There is no patient details available" + ex.Message);
                return false;
            }
        }
        public async Task<TreatmentPlan> GeneratePlan(Patient patientDetails)
        {
            int patientCount = (from x in _context.Patients select x).Count();
            patientDetails.Id = ++patientCount;

            //Receiving package and specialist lists            
            //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "tokenNo");            
            List<PackageDetail> packageList = await GetPackageList();
            List<SpecialistDetail> specialistList = await GetSpecialists();

            TreatmentPlan plan = new TreatmentPlan();
            if (packageList != null && specialistList != null)
            {
               PackageDetail package = (from x in packageList
                                                 where x.TreatmentPackageName == patientDetails.TreatmentPackageName
                          && x.Ailment == patientDetails.Ailment
                                                 select x).SingleOrDefault<PackageDetail>();
                SpecialistDetail specialist;
                if (patientDetails.TreatmentPackageName == "Package 2")
                {
                    specialist = (from x in specialistList
                                  where x.AreaOfExpertise == patientDetails.Ailment
           && x.ExperienceInYears >= 8
                                  select x).SingleOrDefault<SpecialistDetail>();
                }
                else
                {
                    specialist = (from x in specialistList
                                  where x.AreaOfExpertise == patientDetails.Ailment
           && x.ExperienceInYears < 8
                                  select x).SingleOrDefault<SpecialistDetail>();
                }
                int plansCount = (from x in _context.Plans select x).Count();
                plan = new TreatmentPlan()
                {
                    PlanId = ++plansCount,
                    Patient = patientDetails,
                    PatientId = patientDetails.Id,
                    AilmentName = patientDetails.Ailment,
                    PackageName = package.TreatmentPackageName,
                    TestDetails = package.TestDetails,
                    Cost = package.Cost,
                    SpecialistName = specialist.Name,
                    TreatmentCommencementDate = DateTime.Parse(patientDetails.CommencementDate),
                    TreatmentEndDate = DateTime.Parse(patientDetails.CommencementDate).AddDays(package.TreatmentDuration * 7),
                    Status = "In-Progress"
                };
            }
            if (!await SaveAll(patientDetails, plan))
            {
                plan = null;
            }
            return plan;
        }



    }
}
