using System;
using System.Collections.Generic;

namespace HospitalManagement.Domain.Entities
{
    public class Appointment : BaseEntity
    {
        public Guid PatientId { get; set; }
        public Guid DoctorId { get; set; }
        public Guid DepartmentId { get; set; }
        public DateTime AppointmentDate { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public string Status { get; set; } // Scheduled, Completed, Cancelled, No-Show
        public string Type { get; set; } // Regular, Follow-up, Emergency
        public string Notes { get; set; }
        public Guid CreatedById { get; set; }
        
        // Navigation properties
        public virtual Patient Patient { get; set; }
        public virtual Doctor Doctor { get; set; }
        public virtual Department Department { get; set; }
        public virtual User CreatedBy { get; set; }
        public virtual MedicalRecord MedicalRecord { get; set; }
    }
}
