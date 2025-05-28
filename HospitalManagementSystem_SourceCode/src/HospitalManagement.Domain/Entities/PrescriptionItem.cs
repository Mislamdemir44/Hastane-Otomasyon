using System;

namespace HospitalManagement.Domain.Entities
{
    public class PrescriptionItem : BaseEntity
    {
        public Guid PrescriptionId { get; set; }
        public Guid MedicationId { get; set; }
        public string Dosage { get; set; }
        public string Frequency { get; set; }
        public string Duration { get; set; }
        public int Quantity { get; set; }
        public string Instructions { get; set; }
        
        // Navigation properties
        public virtual Prescription Prescription { get; set; }
        public virtual Medication Medication { get; set; }
    }
}
