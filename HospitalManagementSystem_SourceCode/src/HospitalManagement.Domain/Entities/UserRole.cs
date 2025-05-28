using System;

namespace HospitalManagement.Domain.Entities
{
    public class UserRole
    {
        public Guid UserId { get; set; }
        public Guid RoleId { get; set; }
        
        // Navigation properties
        public virtual User User { get; set; }
        public virtual Role Role { get; set; }
    }
}
