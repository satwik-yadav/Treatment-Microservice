using log4net;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TreatmentMicroservice.Models;
using TreatmentMicroservice.Repository;

namespace TreatmentMicroservice.Controllers
{

    [Route("api/[controller]")]
    [ApiController]

    public class TreatmentController : ControllerBase
    {
        static readonly log4net.ILog _log4net = log4net.LogManager.GetLogger(typeof(TreatmentController));
        private ITreatmentRepo _repo;
        public TreatmentController(ITreatmentRepo repo)
        {
            this._repo = repo;
        }

        [HttpGet]
        [Route("FormulateTreatmentTimetable")]

        public async Task<ActionResult<TreatmentPlan>> GeneratePlanDetails([FromQuery]Patient patientDetails)
        {
            _log4net.Info("Generating plan...");
            try
            {
                TreatmentPlan plan = await _repo.GeneratePlan(patientDetails);
                if (plan == null)
                {
                    _log4net.Info("Plan generation failed!");
                    return null;
                }
                _log4net.Info("Plan generated and returned.");
                return Ok(plan);
            }
            catch (Exception ex)
            {
                _log4net.Error($"Some error occurred while generating plan for {patientDetails.Name}!\n {ex.Message}");
                return null;
            }
        }
    }
}
