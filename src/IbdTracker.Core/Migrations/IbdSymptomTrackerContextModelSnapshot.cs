﻿// <auto-generated />
using System;
using IbdTracker.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace IbdTracker.Core.Migrations
{
    [DbContext(typeof(IbdSymptomTrackerContext))]
    partial class IbdSymptomTrackerContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseIdentityByDefaultColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.1");

            modelBuilder.Entity("IbdTracker.Core.Entities.Appointment", b =>
                {
                    b.Property<Guid>("AppointmentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("DoctorId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("EndDateTime")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Location")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Notes")
                        .HasColumnType("text");

                    b.Property<string>("PatientId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("StartDateTime")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("AppointmentId");

                    b.HasIndex("DoctorId");

                    b.HasIndex("PatientId");

                    b.ToTable("Appointments");
                });

            modelBuilder.Entity("IbdTracker.Core.Entities.BowelMovementEvent", b =>
                {
                    b.Property<Guid>("BowelMovementEventId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<bool>("ContainedBlood")
                        .HasColumnType("boolean");

                    b.Property<bool>("ContainedMucus")
                        .HasColumnType("boolean");

                    b.Property<DateTime>("DateTime")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("PatientId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("BowelMovementEventId");

                    b.HasIndex("PatientId");

                    b.ToTable("BowelMovementEvents");
                });

            modelBuilder.Entity("IbdTracker.Core.Entities.Doctor", b =>
                {
                    b.Property<string>("DoctorId")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("DoctorId");

                    b.ToTable("Doctors");
                });

            modelBuilder.Entity("IbdTracker.Core.Entities.Medication", b =>
                {
                    b.Property<Guid>("MedicationId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("ActiveIngredient")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("BrandName")
                        .HasColumnType("text");

                    b.HasKey("MedicationId");

                    b.ToTable("Medications");
                });

            modelBuilder.Entity("IbdTracker.Core.Entities.PainEvent", b =>
                {
                    b.Property<Guid>("PainEventId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("DateTime")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int>("MinutesDuration")
                        .HasColumnType("integer");

                    b.Property<int>("PainScore")
                        .HasColumnType("integer");

                    b.Property<string>("PatientId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("PainEventId");

                    b.HasIndex("PatientId");

                    b.ToTable("PainEvents");
                });

            modelBuilder.Entity("IbdTracker.Core.Entities.Patient", b =>
                {
                    b.Property<string>("PatientId")
                        .HasColumnType("text");

                    b.Property<DateTime>("DateDiagnosed")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTime>("DateOfBirth")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("DoctorId")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("PatientId");

                    b.HasIndex("DoctorId");

                    b.ToTable("Patients");
                });

            modelBuilder.Entity("IbdTracker.Core.Entities.Prescription", b =>
                {
                    b.Property<Guid>("PrescriptionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Dosage")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("MedicationId")
                        .HasColumnType("uuid");

                    b.Property<string>("PatientId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("PrescriptionId");

                    b.HasIndex("MedicationId");

                    b.HasIndex("PatientId");

                    b.ToTable("Prescriptions");
                });

            modelBuilder.Entity("IbdTracker.Core.Entities.Appointment", b =>
                {
                    b.HasOne("IbdTracker.Core.Entities.Doctor", "Doctor")
                        .WithMany("Appointments")
                        .HasForeignKey("DoctorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("IbdTracker.Core.Entities.Patient", null)
                        .WithMany("Appointments")
                        .HasForeignKey("PatientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Doctor");
                });

            modelBuilder.Entity("IbdTracker.Core.Entities.BowelMovementEvent", b =>
                {
                    b.HasOne("IbdTracker.Core.Entities.Patient", null)
                        .WithMany("BowelMovementEvents")
                        .HasForeignKey("PatientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("IbdTracker.Core.Entities.PainEvent", b =>
                {
                    b.HasOne("IbdTracker.Core.Entities.Patient", null)
                        .WithMany("PainEvents")
                        .HasForeignKey("PatientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("IbdTracker.Core.Entities.Patient", b =>
                {
                    b.HasOne("IbdTracker.Core.Entities.Doctor", "Doctor")
                        .WithMany("Patients")
                        .HasForeignKey("DoctorId");

                    b.Navigation("Doctor");
                });

            modelBuilder.Entity("IbdTracker.Core.Entities.Prescription", b =>
                {
                    b.HasOne("IbdTracker.Core.Entities.Medication", "Medication")
                        .WithMany("Prescriptions")
                        .HasForeignKey("MedicationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("IbdTracker.Core.Entities.Patient", null)
                        .WithMany("Prescriptions")
                        .HasForeignKey("PatientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Medication");
                });

            modelBuilder.Entity("IbdTracker.Core.Entities.Doctor", b =>
                {
                    b.Navigation("Appointments");

                    b.Navigation("Patients");
                });

            modelBuilder.Entity("IbdTracker.Core.Entities.Medication", b =>
                {
                    b.Navigation("Prescriptions");
                });

            modelBuilder.Entity("IbdTracker.Core.Entities.Patient", b =>
                {
                    b.Navigation("Appointments");

                    b.Navigation("BowelMovementEvents");

                    b.Navigation("PainEvents");

                    b.Navigation("Prescriptions");
                });
#pragma warning restore 612, 618
        }
    }
}
