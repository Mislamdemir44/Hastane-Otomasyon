using System;

namespace HospitalManagement.Domain.Entities
{
    public class Notification : BaseEntity
    {
        public Guid UserId { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public string Type { get; set; } // Appointment, Lab Result, Payment, System
        public bool IsRead { get; set; }
        public string RelatedEntityType { get; set; }
        public Guid? RelatedEntityId { get; set; }
        
        // Navigation properties
        public virtual User User { get; set; }
        
        public Notification()
        {
            IsRead = false;
        }
    }
}
