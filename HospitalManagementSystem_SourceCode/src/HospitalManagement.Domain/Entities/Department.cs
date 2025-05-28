using System;
using System.Collections.Generic;

namespace HospitalManagement.Domain.Entities
{
    public class Department : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        
        // Navigation properties
        public virtual ICollection<Doctor> Doctors { get; set; }
        public virtual ICollection<Appointment> Appointments { get; set; }
        
        public Department()
        {
            Doctors = new HashSet<Doctor>();
            Appointments = new HashSet<Appointment>();
            IsActive = true;
        }
    }
}
