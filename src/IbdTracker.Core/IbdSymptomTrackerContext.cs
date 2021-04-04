using IbdTracker.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace IbdTracker.Core
{
    public class IbdSymptomTrackerContext : DbContext
    {
        public DbSet<GlobalNotification> GlobalNotifications { get; set; } = null!;
        public DbSet<Doctor> Doctors { get; set; } = null!;
        public DbSet<Patient> Patients { get; set; } = null!;
        public DbSet<Medication> Medications { get; set; } = null!;
        public DbSet<Prescription> Prescriptions { get; set; } = null!;
        public DbSet<SideEffectEvent> SideEffectEvents { get; set; } = null!;
        public DbSet<PainEvent> PainEvents { get; set; } = null!;
        public DbSet<BowelMovementEvent> BowelMovementEvents { get; set; } = null!;
        public DbSet<Appointment> Appointments { get; set; } = null!;
        public DbSet<FoodItem> FoodItems { get; set; } = null!;
        public DbSet<Meal> Meals { get; set; } = null!;
        public DbSet<MealEvent> MealEvents { get; set; } = null!;

        public IbdSymptomTrackerContext(DbContextOptions<IbdSymptomTrackerContext> options)
            : base(options)
        {
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }
    }
}
