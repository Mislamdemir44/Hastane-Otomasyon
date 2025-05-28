using System;

namespace HospitalManagement.Domain.Entities
{
    public class DoctorLeave : BaseEntity
    {
        public Guid DoctorId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Reason { get; set; }
        public string Status { get; set; } // Pending, Approved, Rejected
        public Guid? ApprovedById { get; set; }
        
        // Navigation properties
        public virtual Doctor Doctor { get; set; }
        public virtual User ApprovedBy { get; set; }
    }
}
