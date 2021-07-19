using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TreatmentMicroservice.Models;
namespace TreatmentMicroservice.Data
{
    public class TreatmentContext:DbContext
    {
        public TreatmentContext(DbContextOptions<TreatmentContext> options) : base(options) { }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<TreatmentPlan> Plans { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);

        }

    }
}
