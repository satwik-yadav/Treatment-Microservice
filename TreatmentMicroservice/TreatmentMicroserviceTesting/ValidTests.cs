using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using TreatmentMicroservice.Models;
using TreatmentMicroservice.Repository;
using TreatmentMicroservice.Data;
using TreatmentMicroservice.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace TreatmentMicroserviceTest
{
    class TestServices
    {
        PackageDetail package = new PackageDetail();
        List<PackageDetail> packageDetails;
        SpecialistDetail specialistDetail = new SpecialistDetail();
        List<SpecialistDetail> specialists;
        Patient patient = new Patient();
        List<Patient> patients;
        Mock<ITreatmentRepo> mockRepo;
        TreatmentRepo treatmentRepo;
        TreatmentPlan planDetails = new TreatmentPlan();
        List<TreatmentPlan> plan;
        [SetUp]
        public void Setup()
        {
            var option = new DbContextOptionsBuilder<TreatmentContext>().UseInMemoryDatabase(databaseName: "TreatmentMicroservice").Options;
            var context = new TreatmentContext(option);
            mockRepo = new Mock<ITreatmentRepo>();
            treatmentRepo = new TreatmentRepo(context);
            packageDetails = new List<PackageDetail>()
           {
               new PackageDetail { PackageId = 1, Ailment = "Orthopaedics", TreatmentPackageName = "Package 1", TestDetails = "OPT1, OPT2", Cost = 2500, TreatmentDuration = 4 },
               new PackageDetail { PackageId = 2, Ailment = "Orthopaedics", TreatmentPackageName = "Package 2", TestDetails = "OPT3,OPT4", Cost = 3000, TreatmentDuration = 6 },
               new PackageDetail { PackageId = 3, Ailment ="Urology", TreatmentPackageName = "Package 1", TestDetails = "UPT1,UPT2", Cost = 4000, TreatmentDuration = 4 },
               new PackageDetail { PackageId = 4, Ailment = "Urology", TreatmentPackageName = "Package 2", TestDetails = "UPT3,UPT4", Cost = 5000, TreatmentDuration =6 }
           };
            specialists = new List<SpecialistDetail>()
           {

                new SpecialistDetail { Id = 1, Name = "Shriny", ExperienceInYears = 4, ContactNumber = 8514235879, AreaOfExpertise = "Orthopaedics" },
                new SpecialistDetail { Id = 2, Name = "Joey", ExperienceInYears = 13, ContactNumber = 9875214000, AreaOfExpertise = "Orthopaedics" },
                new SpecialistDetail { Id = 3, Name = "Chandler", ExperienceInYears = 2, ContactNumber = 9965231470, AreaOfExpertise = "Urology" },
                new SpecialistDetail { Id = 4, Name = "Rachel", ExperienceInYears = 15, ContactNumber = 6358290001, AreaOfExpertise = "Urology" }

            };
            plan = new List<TreatmentPlan>()
            {
                new TreatmentPlan{PlanId = 1,AilmentName = "Orthopaedics",PackageName = "Package 1",TestDetails = "OPT1,OPT2",Cost = 2500,SpecialistName = "Shriny"},
                new TreatmentPlan{PlanId = 2,AilmentName = "Orthopaedics",PackageName = "Package 2",TestDetails = "OPT3,OPT4",  Cost = 3000,SpecialistName = "Joey"},
                new TreatmentPlan{PlanId = 3,AilmentName = "Urology",PackageName = "Package 1",TestDetails = "UPT1,UPT2",Cost = 4000,SpecialistName = "Chandler"},
                new TreatmentPlan{PlanId = 4,AilmentName = "Urology",PackageName = "Package 2",TestDetails = "UPT3,UPT4",Cost = 5000,SpecialistName = "Rachel"}
            };
           
        }

        [Test]
        public void TestingGetPackageList_Returns_ValidPackageList()
        {
            mockRepo.Setup(x => x.GetPackageList()).ReturnsAsync(packageDetails);
            var result = treatmentRepo.GetPackageList();
            Assert.IsNotNull(result);
        }
        [Test]
        public void TestingGetSpecialists_Returns_SpecialistsList()
        {
            mockRepo.Setup(x => x.GetSpecialists()).ReturnsAsync(specialists);
            var result = treatmentRepo.GetSpecialists();
            Assert.IsNotNull(result);
        }
        [TestCase]
        public void   GeneratePlanDetails_ValidData()
        {
            TreatmentController obj = new TreatmentController(mockRepo.Object);
            var result = obj.GeneratePlanDetails

              (new Patient
              {
                  Id = 1,
                  Name = "Harry",
                  Age = 20,
                  Ailment = "Orthopaedics",
                  TreatmentPackageName = "Package 1",
                  CommencementDate = "2021-07-16",
              });
            Assert.IsNotNull(result);
        }
        [TestCase]
        public void SaveAll_ValidData()
        {
            var obj=mockRepo.Setup(x => x.SaveAll(patient,planDetails)).ReturnsAsync(true);

            var result = treatmentRepo.SaveAll(new Patient
              {
                  Id = 1,
                  Name = "Harry",
                  Age = 20,
                  Ailment = "Orthopaedics",
                  TreatmentPackageName = "Package 1",
                  CommencementDate = "2021-07-16",
              },planDetails);
            Assert.IsNotNull(result);
        }


    }
}