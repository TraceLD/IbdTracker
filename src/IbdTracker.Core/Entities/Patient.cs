using System;
using System.Collections.Generic;

namespace IbdTracker.Core.Entities
{
    public class Patient
    {
        public string PatientId { get; set; } = null!;
        public string Name { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime DateDiagnosed { get; set; }
        
        public string? DoctorId { get; set; }
        public Doctor? Doctor { get; set; }

        public List<Prescription> Prescriptions { get; } = new();
        public List<PainEvent> PainEvents { get; } = new();
        public List<BowelMovementEvent> BowelMovementEvents { get; } = new();
        public List<Appointment> Appointments { get; } = new();
        public List<Meal> Meals { get; } = new();
        public List<MealEvent> MealEvents { get; } = new();

        public Patient(string name, DateTime dateOfBirth, DateTime dateDiagnosed, string? doctorId = null)
        {
            Name = name;
            DateOfBirth = dateOfBirth;
            DateDiagnosed = dateDiagnosed;
            DoctorId = doctorId;
        }
    }
}