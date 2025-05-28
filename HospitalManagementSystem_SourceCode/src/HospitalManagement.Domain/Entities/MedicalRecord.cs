using System;
using System.Collections.Generic;

namespace HospitalManagement.Domain.Entities
{
    public class MedicalRecord : BaseEntity
    {
        public Guid PatientId { get; set; }
        public Guid DoctorId { get; set; }
        public Guid AppointmentId { get; set; }
        public DateTime VisitDate { get; set; }
        public string ChiefComplaint { get; set; }
        public string Diagnosis { get; set; }
        public string Treatment { get; set; }
        public string Prescription { get; set; }
        public string Notes { get; set; }
        public DateTime? FollowUpDate { get; set; }
        
        // Navigation properties
        public virtual Patient Patient { get; set; }
        public virtual Doctor Doctor { get; set; }
        public virtual Appointment Appointment { get; set; }
        public virtual ICollection<Prescription> Prescriptions { get; set; }
        public virtual ICollection<LabTestRequest> LabTestRequests { get; set; }
        
        public MedicalRecord()
        {
            Prescriptions = new HashSet<Prescription>();
            LabTestRequests = new HashSet<LabTestRequest>();
            VisitDate = DateTime.UtcNow;
        }
    }
}
