﻿// <auto-generated />
using System;
using System.Collections.Generic;
using IbdTracker.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace IbdTracker.Core.Migrations
{
    [DbContext(typeof(IbdSymptomTrackerContext))]
    [Migration("20210423185724_MoveSettingsToSeparateTable")]
    partial class MoveSettingsToSeparateTable
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseIdentityByDefaultColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.1");

            modelBuilder.Entity("FoodItemMeal", b =>
                {
                    b.Property<Guid>("FoodItemsFoodItemId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("MealsMealId")
                        .HasColumnType("uuid");

                    b.HasKey("FoodItemsFoodItemId", "MealsMealId");

                    b.HasIndex("MealsMealId");

                    b.ToTable("FoodItemMeal");
                });

            modelBuilder.Entity("IbdTracker.Core.Entities.Appointment", b =>
                {
                    b.Property<Guid>("AppointmentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("DoctorId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("DoctorNotes")
                        .HasColumnType("text");

                    b.Property<int>("DurationMinutes")
                        .HasColumnType("integer");

                    b.Property<string>("PatientId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("PatientNotes")
                        .HasColumnType("text");

                    b.Property<DateTime>("StartDateTime")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("AppointmentId");

                    b.HasIndex("PatientId");

                    b.HasIndex("DoctorId", "StartDateTime")
                        .IsUnique();

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

                    b.Property<string>("Location")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<List<OfficeHours>>("OfficeHours")
                        .IsRequired()
                        .HasColumnType("jsonb");

                    b.HasKey("DoctorId");

                    b.ToTable("Doctors");
                });

            modelBuilder.Entity("IbdTracker.Core.Entities.FoodItem", b =>
                {
                    b.Property<Guid>("FoodItemId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("PictureUrl")
                        .HasColumnType("text");

                    b.HasKey("FoodItemId");

                    b.ToTable("FoodItems");
                });

            modelBuilder.Entity("IbdTracker.Core.Entities.GlobalNotification", b =>
                {
                    b.Property<Guid>("GlobalNotificationId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Message")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("TailwindColour")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Url")
                        .HasColumnType("text");

                    b.HasKey("GlobalNotificationId");

                    b.ToTable("GlobalNotifications");
                });

            modelBuilder.Entity("IbdTracker.Core.Entities.InformationRequest", b =>
                {
                    b.Property<Guid>("InformationRequestId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("DoctorId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean");

                    b.Property<string>("PatientId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("RequestedBms")
                        .HasColumnType("boolean");

                    b.Property<DateTime>("RequestedDataFrom")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTime>("RequestedDataTo")
                        .HasColumnType("timestamp without time zone");

                    b.Property<bool>("RequestedPain")
                        .HasColumnType("boolean");

                    b.HasKey("InformationRequestId");

                    b.HasIndex("DoctorId");

                    b.HasIndex("PatientId");

                    b.ToTable("InformationRequests");
                });

            modelBuilder.Entity("IbdTracker.Core.Entities.Meal", b =>
                {
                    b.Property<Guid>("MealId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("PatientId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("MealId");

                    b.HasIndex("PatientId");

                    b.ToTable("Meals");
                });

            modelBuilder.Entity("IbdTracker.Core.Entities.MealEvent", b =>
                {
                    b.Property<Guid>("MealEventId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("DateTime")
                        .HasColumnType("timestamp without time zone");

                    b.Property<Guid>("MealId")
                        .HasColumnType("uuid");

                    b.Property<string>("PatientId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("MealEventId");

                    b.HasIndex("MealId");

                    b.HasIndex("PatientId");

                    b.ToTable("MealEvents");
                });

            modelBuilder.Entity("IbdTracker.Core.Entities.Medication", b =>
                {
                    b.Property<Guid>("MedicationId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("BnfChapter")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("BnfChapterCode")
                        .HasColumnType("integer");

                    b.Property<string>("BnfChemicalSubstance")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("BnfChemicalSubstanceCode")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("BnfParagraph")
                        .HasColumnType("text");

                    b.Property<int>("BnfParagraphCode")
                        .HasColumnType("integer");

                    b.Property<string>("BnfPresentation")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("BnfPresentationCode")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("BnfProduct")
                        .HasColumnType("text");

                    b.Property<string>("BnfProductCode")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("BnfSection")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("BnfSectionCode")
                        .HasColumnType("integer");

                    b.Property<string>("BnfSubparagraph")
                        .HasColumnType("text");

                    b.Property<int>("BnfSubparagraphCode")
                        .HasColumnType("integer");

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

            modelBuilder.Entity("IbdTracker.Core.Entities.PatientApplicationSettings", b =>
                {
                    b.Property<string>("PatientId")
                        .HasColumnType("text");

                    b.Property<bool>("ShareDataForResearch")
                        .HasColumnType("boolean");

                    b.HasKey("PatientId");

                    b.ToTable("PatientApplicationSettings");
                });

            modelBuilder.Entity("IbdTracker.Core.Entities.Prescription", b =>
                {
                    b.Property<Guid>("PrescriptionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("DoctorInstructions")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("EndDateTime")
                        .HasColumnType("timestamp without time zone");

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

            modelBuilder.Entity("IbdTracker.Core.Entities.SideEffectEvent", b =>
                {
                    b.Property<Guid>("SideEffectEventId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("PrescriptionId")
                        .HasColumnType("uuid");

                    b.HasKey("SideEffectEventId");

                    b.HasIndex("PrescriptionId");

                    b.ToTable("SideEffectEvents");
                });

            modelBuilder.Entity("FoodItemMeal", b =>
                {
                    b.HasOne("IbdTracker.Core.Entities.FoodItem", null)
                        .WithMany()
                        .HasForeignKey("FoodItemsFoodItemId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("IbdTracker.Core.Entities.Meal", null)
                        .WithMany()
                        .HasForeignKey("MealsMealId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
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

            modelBuilder.Entity("IbdTracker.Core.Entities.InformationRequest", b =>
                {
                    b.HasOne("IbdTracker.Core.Entities.Doctor", null)
                        .WithMany("InformationRequests")
                        .HasForeignKey("DoctorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("IbdTracker.Core.Entities.Patient", null)
                        .WithMany("InformationRequests")
                        .HasForeignKey("PatientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("IbdTracker.Core.Entities.Meal", b =>
                {
                    b.HasOne("IbdTracker.Core.Entities.Patient", null)
                        .WithMany("Meals")
                        .HasForeignKey("PatientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("IbdTracker.Core.Entities.MealEvent", b =>
                {
                    b.HasOne("IbdTracker.Core.Entities.Meal", null)
                        .WithMany("MealEvents")
                        .HasForeignKey("MealId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("IbdTracker.Core.Entities.Patient", null)
                        .WithMany("MealEvents")
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

            modelBuilder.Entity("IbdTracker.Core.Entities.PatientApplicationSettings", b =>
                {
                    b.HasOne("IbdTracker.Core.Entities.Patient", null)
                        .WithOne("PatientApplicationSettings")
                        .HasForeignKey("IbdTracker.Core.Entities.PatientApplicationSettings", "PatientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
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

            modelBuilder.Entity("IbdTracker.Core.Entities.SideEffectEvent", b =>
                {
                    b.HasOne("IbdTracker.Core.Entities.Prescription", null)
                        .WithMany("SideEffectEvents")
                        .HasForeignKey("PrescriptionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("IbdTracker.Core.Entities.Doctor", b =>
                {
                    b.Navigation("Appointments");

                    b.Navigation("InformationRequests");

                    b.Navigation("Patients");
                });

            modelBuilder.Entity("IbdTracker.Core.Entities.Meal", b =>
                {
                    b.Navigation("MealEvents");
                });

            modelBuilder.Entity("IbdTracker.Core.Entities.Medication", b =>
                {
                    b.Navigation("Prescriptions");
                });

            modelBuilder.Entity("IbdTracker.Core.Entities.Patient", b =>
                {
                    b.Navigation("Appointments");

                    b.Navigation("BowelMovementEvents");

                    b.Navigation("InformationRequests");

                    b.Navigation("MealEvents");

                    b.Navigation("Meals");

                    b.Navigation("PainEvents");

                    b.Navigation("PatientApplicationSettings");

                    b.Navigation("Prescriptions");
                });

            modelBuilder.Entity("IbdTracker.Core.Entities.Prescription", b =>
                {
                    b.Navigation("SideEffectEvents");
                });
#pragma warning restore 612, 618
        }
    }
}
