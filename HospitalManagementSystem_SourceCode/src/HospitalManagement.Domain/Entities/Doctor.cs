using System;
using System.Collections.Generic;

namespace HospitalManagement.Domain.Entities
{
    public class Doctor : BaseEntity
    {
        public Guid UserId { get; set; }
        public Guid DepartmentId { get; set; }
        public Guid SpecialtyId { get; set; }
        public string LicenseNumber { get; set; }
        public string Education { get; set; }
        public string Biography { get; set; }
        public decimal ConsultationFee { get; set; }
        public bool IsAvailableForAppointment { get; set; }
        
        // Navigation properties
        public virtual User User { get; set; }
        public virtual Department Department { get; set; }
        public virtual Specialty Specialty { get; set; }
        public virtual ICollection<DoctorSchedule> DoctorSchedules { get; set; }
        public virtual ICollection<DoctorLeave> DoctorLeaves { get; set; }
        public virtual ICollection<Appointment> Appointments { get; set; }
        public virtual ICollection<MedicalRecord> MedicalRecords { get; set; }
        public virtual ICollection<Prescription> Prescriptions { get; set; }
        public virtual ICollection<LabTestRequest> LabTestRequests { get; set; }
        
        public Doctor()
        {
            DoctorSchedules = new HashSet<DoctorSchedule>();
            DoctorLeaves = new HashSet<DoctorLeave>();
            Appointments = new HashSet<Appointment>();
            MedicalRecords = new HashSet<MedicalRecord>();
            Prescriptions = new HashSet<Prescription>();
            LabTestRequests = new HashSet<LabTestRequest>();
            IsAvailableForAppointment = true;
        }
    }
}
