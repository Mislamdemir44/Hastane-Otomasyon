using System;
using System.Collections.Generic;

namespace HospitalManagement.Domain.Entities
{
    public class Prescription : BaseEntity
    {
        public Guid PatientId { get; set; }
        public Guid DoctorId { get; set; }
        public Guid MedicalRecordId { get; set; }
        public DateTime PrescriptionDate { get; set; }
        public string Status { get; set; } // Active, Filled, Expired
        public string Notes { get; set; }
        
        // Navigation properties
        public virtual Patient Patient { get; set; }
        public virtual Doctor Doctor { get; set; }
        public virtual MedicalRecord MedicalRecord { get; set; }
        public virtual ICollection<PrescriptionItem> PrescriptionItems { get; set; }
        
        public Prescription()
        {
            PrescriptionItems = new HashSet<PrescriptionItem>();
            PrescriptionDate = DateTime.UtcNow;
            Status = "Active";
        }
    }
}
