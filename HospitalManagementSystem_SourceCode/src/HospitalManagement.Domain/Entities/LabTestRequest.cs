using System;
using System.Collections.Generic;

namespace HospitalManagement.Domain.Entities
{
    public class LabTestRequest : BaseEntity
    {
        public Guid PatientId { get; set; }
        public Guid DoctorId { get; set; }
        public Guid MedicalRecordId { get; set; }
        public DateTime RequestDate { get; set; }
        public string Status { get; set; } // Requested, In Progress, Completed, Cancelled
        public string Priority { get; set; } // Routine, Urgent, STAT
        public string Notes { get; set; }
        
        // Navigation properties
        public virtual Patient Patient { get; set; }
        public virtual Doctor Doctor { get; set; }
        public virtual MedicalRecord MedicalRecord { get; set; }
        public virtual ICollection<LabTestRequestItem> LabTestRequestItems { get; set; }
        
        public LabTestRequest()
        {
            LabTestRequestItems = new HashSet<LabTestRequestItem>();
            RequestDate = DateTime.UtcNow;
            Status = "Requested";
            Priority = "Routine";
        }
    }
}
