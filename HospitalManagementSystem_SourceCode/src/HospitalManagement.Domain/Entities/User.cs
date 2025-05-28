using System;
using System.Collections.Generic;

namespace HospitalManagement.Domain.Entities
{
    public class User : BaseEntity
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsActive { get; set; }
        public DateTime? LastLoginAt { get; set; }
        
        // Navigation properties
        public virtual ICollection<UserRole> UserRoles { get; set; }
        public virtual Doctor Doctor { get; set; }
        
        public User()
        {
            UserRoles = new HashSet<UserRole>();
            IsActive = true;
        }
    }
}
