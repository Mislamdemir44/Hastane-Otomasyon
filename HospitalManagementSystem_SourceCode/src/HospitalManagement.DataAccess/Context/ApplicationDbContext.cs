using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using HospitalManagement.Domain.Entities;

namespace HospitalManagement.DataAccess.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Specialty> Specialties { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<DoctorSchedule> DoctorSchedules { get; set; }
        public DbSet<DoctorLeave> DoctorLeaves { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<MedicalRecord> MedicalRecords { get; set; }
        public DbSet<Prescription> Prescriptions { get; set; }
        public DbSet<PrescriptionItem> PrescriptionItems { get; set; }
        public DbSet<Medication> Medications { get; set; }
        public DbSet<LabTest> LabTests { get; set; }
        public DbSet<LabTestRequest> LabTestRequests { get; set; }
        public DbSet<LabTestRequestItem> LabTestRequestItems { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<InvoiceItem> InvoiceItems { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure UserRole as a join table with composite key
            modelBuilder.Entity<UserRole>()
                .HasKey(ur => new { ur.UserId, ur.RoleId });

            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.User)
                .WithMany(u => u.UserRoles)
                .HasForeignKey(ur => ur.UserId);

            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.Role)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(ur => ur.RoleId);

            // Configure User entity
            modelBuilder.Entity<User>()
                .HasIndex(u => u.UserName)
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            // Configure Patient entity
            modelBuilder.Entity<Patient>()
                .HasIndex(p => p.IdentityNumber)
                .IsUnique();

            modelBuilder.Entity<Patient>()
                .HasIndex(p => new { p.LastName, p.FirstName });

            // Configure Doctor entity
            modelBuilder.Entity<Doctor>()
                .HasIndex(d => d.LicenseNumber)
                .IsUnique();

            modelBuilder.Entity<Doctor>()
                .HasOne(d => d.User)
                .WithOne(u => u.Doctor)
                .HasForeignKey<Doctor>(d => d.UserId);

            modelBuilder.Entity<Doctor>()
                .HasOne(d => d.Department)
                .WithMany(d => d.Doctors)
                .HasForeignKey(d => d.DepartmentId);

            modelBuilder.Entity<Doctor>()
                .HasOne(d => d.Specialty)
                .WithMany(s => s.Doctors)
                .HasForeignKey(d => d.SpecialtyId);

            // Configure Department entity
            modelBuilder.Entity<Department>()
                .HasIndex(d => d.Name)
                .IsUnique();

            // Configure Specialty entity
            modelBuilder.Entity<Specialty>()
                .HasIndex(s => s.Name)
                .IsUnique();

            // Configure DoctorSchedule entity
            modelBuilder.Entity<DoctorSchedule>()
                .HasOne(ds => ds.Doctor)
                .WithMany(d => d.DoctorSchedules)
                .HasForeignKey(ds => ds.DoctorId);

            // Configure DoctorLeave entity
            modelBuilder.Entity<DoctorLeave>()
                .HasOne(dl => dl.Doctor)
                .WithMany(d => d.DoctorLeaves)
                .HasForeignKey(dl => dl.DoctorId);

            modelBuilder.Entity<DoctorLeave>()
                .HasOne(dl => dl.ApprovedBy)
                .WithMany()
                .HasForeignKey(dl => dl.ApprovedById)
                .IsRequired(false);

            // Configure Appointment entity
            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Patient)
                .WithMany(p => p.Appointments)
                .HasForeignKey(a => a.PatientId);

            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Doctor)
                .WithMany(d => d.Appointments)
                .HasForeignKey(a => a.DoctorId);

            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Department)
                .WithMany(d => d.Appointments)
                .HasForeignKey(a => a.DepartmentId);

            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.CreatedBy)
                .WithMany()
                .HasForeignKey(a => a.CreatedById);

            modelBuilder.Entity<Appointment>()
                .HasIndex(a => new { a.PatientId, a.DoctorId, a.AppointmentDate });

            // Configure MedicalRecord entity
            modelBuilder.Entity<MedicalRecord>()
                .HasOne(mr => mr.Patient)
                .WithMany(p => p.MedicalRecords)
                .HasForeignKey(mr => mr.PatientId);

            modelBuilder.Entity<MedicalRecord>()
                .HasOne(mr => mr.Doctor)
                .WithMany(d => d.MedicalRecords)
                .HasForeignKey(mr => mr.DoctorId);

            modelBuilder.Entity<MedicalRecord>()
                .HasOne(mr => mr.Appointment)
                .WithOne(a => a.MedicalRecord)
                .HasForeignKey<MedicalRecord>(mr => mr.AppointmentId);

            // Configure Prescription entity
            modelBuilder.Entity<Prescription>()
                .HasOne(p => p.Patient)
                .WithMany(p => p.Prescriptions)
                .HasForeignKey(p => p.PatientId);

            modelBuilder.Entity<Prescription>()
                .HasOne(p => p.Doctor)
                .WithMany(d => d.Prescriptions)
                .HasForeignKey(p => p.DoctorId);

            modelBuilder.Entity<Prescription>()
                .HasOne(p => p.MedicalRecord)
                .WithMany(mr => mr.Prescriptions)
                .HasForeignKey(p => p.MedicalRecordId);

            // Configure PrescriptionItem entity
            modelBuilder.Entity<PrescriptionItem>()
                .HasOne(pi => pi.Prescription)
                .WithMany(p => p.PrescriptionItems)
                .HasForeignKey(pi => pi.PrescriptionId);

            modelBuilder.Entity<PrescriptionItem>()
                .HasOne(pi => pi.Medication)
                .WithMany(m => m.PrescriptionItems)
                .HasForeignKey(pi => pi.MedicationId);

            // Configure LabTestRequest entity
            modelBuilder.Entity<LabTestRequest>()
                .HasOne(ltr => ltr.Patient)
                .WithMany(p => p.LabTestRequests)
                .HasForeignKey(ltr => ltr.PatientId);

            modelBuilder.Entity<LabTestRequest>()
                .HasOne(ltr => ltr.Doctor)
                .WithMany(d => d.LabTestRequests)
                .HasForeignKey(ltr => ltr.DoctorId);

            modelBuilder.Entity<LabTestRequest>()
                .HasOne(ltr => ltr.MedicalRecord)
                .WithMany(mr => mr.LabTestRequests)
                .HasForeignKey(ltr => ltr.MedicalRecordId);

            // Configure LabTestRequestItem entity
            modelBuilder.Entity<LabTestRequestItem>()
                .HasOne(ltri => ltri.LabTestRequest)
                .WithMany(ltr => ltr.LabTestRequestItems)
                .HasForeignKey(ltri => ltri.LabTestRequestId);

            modelBuilder.Entity<LabTestRequestItem>()
                .HasOne(ltri => ltri.LabTest)
                .WithMany(lt => lt.LabTestRequestItems)
                .HasForeignKey(ltri => ltri.LabTestId);

            modelBuilder.Entity<LabTestRequestItem>()
                .HasOne(ltri => ltri.Technician)
                .WithMany()
                .HasForeignKey(ltri => ltri.TechnicianId)
                .IsRequired(false);

            modelBuilder.Entity<LabTestRequestItem>()
                .HasOne(ltri => ltri.VerifiedBy)
                .WithMany()
                .HasForeignKey(ltri => ltri.VerifiedById)
                .IsRequired(false);

            // Configure Invoice entity
            modelBuilder.Entity<Invoice>()
                .HasOne(i => i.Patient)
                .WithMany(p => p.Invoices)
                .HasForeignKey(i => i.PatientId);

            modelBuilder.Entity<Invoice>()
                .HasOne(i => i.CreatedBy)
                .WithMany()
                .HasForeignKey(i => i.CreatedById);

            modelBuilder.Entity<Invoice>()
                .HasIndex(i => i.InvoiceNumber)
                .IsUnique();

            // Configure InvoiceItem entity
            modelBuilder.Entity<InvoiceItem>()
                .HasOne(ii => ii.Invoice)
                .WithMany(i => i.InvoiceItems)
                .HasForeignKey(ii => ii.InvoiceId);

            // Configure Payment entity
            modelBuilder.Entity<Payment>()
                .HasOne(p => p.Invoice)
                .WithMany(i => i.Payments)
                .HasForeignKey(p => p.InvoiceId);

            modelBuilder.Entity<Payment>()
                .HasOne(p => p.ReceivedBy)
                .WithMany()
                .HasForeignKey(p => p.ReceivedById);

            // Configure Notification entity
            modelBuilder.Entity<Notification>()
                .HasOne(n => n.User)
                .WithMany()
                .HasForeignKey(n => n.UserId);

            // Configure AuditLog entity
            modelBuilder.Entity<AuditLog>()
                .HasOne(al => al.User)
                .WithMany()
                .HasForeignKey(al => al.UserId)
                .IsRequired(false);

            // Add check constraints
            modelBuilder.Entity<Appointment>()
                .HasCheckConstraint("CK_Appointment_EndTime_GT_StartTime", "\"EndTime\" > \"StartTime\"");

            modelBuilder.Entity<DoctorSchedule>()
                .HasCheckConstraint("CK_DoctorSchedule_EndTime_GT_StartTime", "\"EndTime\" > \"StartTime\"");

            modelBuilder.Entity<DoctorLeave>()
                .HasCheckConstraint("CK_DoctorLeave_EndDate_GTE_StartDate", "\"EndDate\" >= \"StartDate\"");

            modelBuilder.Entity<Invoice>()
                .HasCheckConstraint("CK_Invoice_TotalAmount_GTE_Zero", "\"TotalAmount\" >= 0");

            modelBuilder.Entity<Invoice>()
                .HasCheckConstraint("CK_Invoice_PaidAmount_GTE_Zero", "\"PaidAmount\" >= 0");

            modelBuilder.Entity<Payment>()
                .HasCheckConstraint("CK_Payment_Amount_GT_Zero", "\"Amount\" > 0");
        }
    }
}
