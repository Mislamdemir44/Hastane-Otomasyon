using System;
using System.Collections.Generic;

namespace HospitalManagement.Domain.Entities
{
    public class DoctorSchedule : BaseEntity
    {
        public Guid DoctorId { get; set; }
        public int DayOfWeek { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public bool IsAvailable { get; set; }
        public int MaxAppointments { get; set; }
        
        // Navigation properties
        public virtual Doctor Doctor { get; set; }
        
        public DoctorSchedule()
        {
            IsAvailable = true;
            MaxAppointments = 10; // Default value
        }
    }
}
