using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using TreatmentMicroservice.Controllers;
using TreatmentMicroservice.Data;
using TreatmentMicroservice.Models;
using TreatmentMicroservice.Repository;

namespace TreatmentMicroserviceTesting
{
    class InvalidTests
    {
        PackageDetail package = new PackageDetail();
        List<PackageDetail> packageDetails = new List<PackageDetail>();
        SpecialistDetail specialistDetail = new SpecialistDetail();
        List<SpecialistDetail> specialists = new List<SpecialistDetail>();
        Mock<ITreatmentRepo> mockRepo;
        TreatmentRepo treatmentRepo;
        Patient patient = new Patient();
        List<Patient> patients;
        TreatmentPlan planDetails = new TreatmentPlan();
        List<TreatmentPlan> plan;
        [SetUp]
        public void Setup()
        {
            mockRepo = new Mock<ITreatmentRepo>();
            packageDetails = null;
            specialists = null;
            var option = new DbContextOptionsBuilder<TreatmentContext>().UseInMemoryDatabase(databaseName: "TreatmentMicroservice").Options;
            var context = new TreatmentContext(option);
            treatmentRepo = new TreatmentRepo(context);

        }
        [Test]
        public void TestingGetPackageList_Returns_InValidPackageList()
        {
            mockRepo.Setup(x => x.GetPackageList()).ReturnsAsync(new List<PackageDetail>());
            var result = treatmentRepo.GetPackageList();
            var a = mockRepo.Object.GetPackageList();
            Assert.AreNotEqual(a, result);
        }
        [Test]
        public void TestingGetSpecialists_Returns_InvalidSpecialistsList()
        {
            mockRepo.Setup(x => x.GetSpecialists()).ReturnsAsync(new List<SpecialistDetail>());
            var result = treatmentRepo.GetSpecialists();
            var a = mockRepo.Object.GetSpecialists();
            Assert.AreNotEqual(a, result);

        }

        [TestCase]
        public void GeneratePlanDetails_InValidData()
        {
            mockRepo.Setup(x => x.GeneratePlan(patient));
            TreatmentController pc = new TreatmentController(mockRepo.Object);
            var result1 = pc.GeneratePlanDetails(patient) as IEnumerable<Patient>;
            var result = pc.GeneratePlanDetails(patient);
            Assert.AreNotEqual(result, result1);

        }
        [TestCase]
        public void SaveAll_InValidData()
        {
            var obj = mockRepo.Setup(x => x.SaveAll(patient, planDetails)).ReturnsAsync(true);
            var result = treatmentRepo.SaveAll(patient, planDetails);
            Assert.AreNotEqual(obj, result);

        }
    }
}
