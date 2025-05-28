using System;
using System.Collections.Generic;

namespace HospitalManagement.Domain.Entities
{
    public class Patient : BaseEntity
    {
        public string IdentityNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string BloodType { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string EmergencyContactName { get; set; }
        public string EmergencyContactPhone { get; set; }
        public string InsuranceProvider { get; set; }
        public string InsuranceNumber { get; set; }
        public bool IsActive { get; set; }
        
        // Navigation properties
        public virtual ICollection<Appointment> Appointments { get; set; }
        public virtual ICollection<MedicalRecord> MedicalRecords { get; set; }
        public virtual ICollection<Prescription> Prescriptions { get; set; }
        public virtual ICollection<LabTestRequest> LabTestRequests { get; set; }
        public virtual ICollection<Invoice> Invoices { get; set; }
        
        public Patient()
        {
            Appointments = new HashSet<Appointment>();
            MedicalRecords = new HashSet<MedicalRecord>();
            Prescriptions = new HashSet<Prescription>();
            LabTestRequests = new HashSet<LabTestRequest>();
            Invoices = new HashSet<Invoice>();
            IsActive = true;
        }
    }
}
