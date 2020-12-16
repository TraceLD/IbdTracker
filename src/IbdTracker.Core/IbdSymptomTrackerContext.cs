using IbdTracker.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace IbdTracker.Core
{
    public class IbdSymptomTrackerContext : DbContext
    {
        public DbSet<Doctor> Doctors { get; set; } = null!;
        public DbSet<Patient> Patients { get; set; } = null!;
        public DbSet<Medication> Medications { get; set; } = null!;
        public DbSet<Prescription> Prescriptions { get; set; } = null!;
        public DbSet<PainEvent> PainEvents { get; set; } = null!;
        public DbSet<BowelMovementEvent> BowelMovementEvents { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(
                @"");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // to search patients via ID from Auth0 JWT;
            modelBuilder.Entity<Patient>()
                .HasIndex(p => p.AuthId);
            modelBuilder.Entity<Doctor>()
                .HasIndex(d => d.AuthId);
        }
    }
}
