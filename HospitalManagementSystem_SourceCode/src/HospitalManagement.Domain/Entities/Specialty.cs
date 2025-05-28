using System;
using System.Collections.Generic;

namespace HospitalManagement.Domain.Entities
{
    public class Specialty : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        
        // Navigation properties
        public virtual ICollection<Doctor> Doctors { get; set; }
        
        public Specialty()
        {
            Doctors = new HashSet<Doctor>();
        }
    }
}
