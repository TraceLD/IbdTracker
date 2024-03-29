﻿using IbdTracker.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace IbdTracker.Core
{
    public class IbdSymptomTrackerContext : DbContext
    {
        public DbSet<GlobalNotification> GlobalNotifications { get; set; } = null!;
        public DbSet<Doctor> Doctors { get; set; } = null!;
        public DbSet<Patient> Patients { get; set; } = null!;
        public DbSet<PatientApplicationSettings> PatientApplicationSettings { get; set; } = null!;
        public DbSet<Medication> Medications { get; set; } = null!;
        public DbSet<Prescription> Prescriptions { get; set; } = null!;
        public DbSet<PainEvent> PainEvents { get; set; } = null!;
        public DbSet<BowelMovementEvent> BowelMovementEvents { get; set; } = null!;
        public DbSet<Appointment> Appointments { get; set; } = null!;
        public DbSet<FoodItem> FoodItems { get; set; } = null!;
        public DbSet<Meal> Meals { get; set; } = null!;
        public DbSet<MealEvent> MealEvents { get; set; } = null!;
        public DbSet<InformationRequest> InformationRequests { get; set; } = null!;

        public IbdSymptomTrackerContext(DbContextOptions<IbdSymptomTrackerContext> options)
            : base(options)
        {
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Doctor>()
                .Property(d => d.OfficeHours)
                .HasColumnType("jsonb");

            // prevent the same doctor from having multiple appointments start at the same time;
            // we don't worry about duration/end time/uneven start times as they are all 30 minutes long
            // and start at even 00 or 30 minutes. This is validated by the API when an appointment is created.
            modelBuilder.Entity<Appointment>()
                .HasIndex(a => new {a.DoctorId, a.StartDateTime})
                .IsUnique();

            modelBuilder.Entity<PatientApplicationSettings>()
                .HasKey(s => s.PatientId);
            
            // indexes for enhanced performance of common queries;
            modelBuilder.Entity<Medication>()
                .HasIndex(m => m.BnfChemicalSubstance);
            modelBuilder.Entity<Medication>()
                .HasIndex(m => m.BnfProduct);
            modelBuilder.Entity<Prescription>()
                .HasIndex(p => p.StartDateTime);
            modelBuilder.Entity<FoodItem>()
                .HasIndex(fi => fi.Name);
            
        }
    }
}
