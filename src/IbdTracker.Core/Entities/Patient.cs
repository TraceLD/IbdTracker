using System;
using System.Collections.Generic;

namespace IbdTracker.Core.Entities
{
    public class Patient
    {
        public int PatientId { get; set; }
        public string AuthId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime DateDiagnosed { get; set; }
        public int? DoctorId { get; set; }

        public List<Prescription> Prescriptions { get; } = new List<Prescription>();
        public List<PainEvent> PainEvents { get; } = new List<PainEvent>();
        public List<BowelMovementEvent> BowelMovementEvents { get; } = new List<BowelMovementEvent>();

        public Patient(string authId, string firstName, string lastName, DateTime dateOfBirth, DateTime dateDiagnosed,
            int? doctorId = null)
        {
            AuthId = authId;
            FirstName = firstName;
            LastName = lastName;
            DateOfBirth = dateOfBirth;
            DateDiagnosed = dateDiagnosed;
            DoctorId = doctorId;
        }
    }
}