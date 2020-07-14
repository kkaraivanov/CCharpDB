namespace P01_HospitalDatabase.Data
{
    using Configuration;
    using Microsoft.EntityFrameworkCore;
    using Models;

    public class HospitalContext : DbContext
    {
        public HospitalContext(){}

        public HospitalContext(DbContextOptions options) : base(options) { }

        public DbSet<Patient> Patients { get; set; }

        public DbSet<Doctor> Doctors { get; set; }

        public DbSet<Diagnose> Diagnoses { get; set; }

        public DbSet<Medicament> Medicaments { get; set; }

        public DbSet<PatientMedicament> PatientMedicaments { get; set; }

        public DbSet<Visitation> Visitations { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder ob)
        {
            if (!ob.IsConfigured)
                ob.UseSqlServer(DbContextConfiguration.ConnectionString);
        }

        protected override void OnModelCreating(ModelBuilder mb)
        {
            mb.ApplyConfiguration(new EntityDiagnoseConfiguration());
            mb.ApplyConfiguration(new EntityMedicamentConfiguration());
            mb.ApplyConfiguration(new EntityPatientConfiguration());
            mb.ApplyConfiguration(new EntityPatientMedicamentConfiguration());
            mb.ApplyConfiguration(new EntityVisitationConfiguration());
        }
    }
}