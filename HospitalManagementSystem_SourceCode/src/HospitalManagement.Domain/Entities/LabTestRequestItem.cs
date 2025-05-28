using System;

namespace HospitalManagement.Domain.Entities
{
    public class LabTestRequestItem : BaseEntity
    {
        public Guid LabTestRequestId { get; set; }
        public Guid LabTestId { get; set; }
        public string Status { get; set; } // Pending, In Progress, Completed, Cancelled
        public DateTime? ResultDate { get; set; }
        public string ResultValue { get; set; }
        public string ReferenceRange { get; set; }
        public string Remarks { get; set; }
        public Guid? TechnicianId { get; set; }
        public Guid? VerifiedById { get; set; }
        
        // Navigation properties
        public virtual LabTestRequest LabTestRequest { get; set; }
        public virtual LabTest LabTest { get; set; }
        public virtual User Technician { get; set; }
        public virtual User VerifiedBy { get; set; }
        
        public LabTestRequestItem()
        {
            Status = "Pending";
        }
    }
}
