using System;
using System.Collections.Generic;

namespace HospitalManagement.Domain.Entities
{
    public class LabTest : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Department { get; set; }
        public decimal Price { get; set; }
        public bool IsActive { get; set; }
        
        // Navigation properties
        public virtual ICollection<LabTestRequestItem> LabTestRequestItems { get; set; }
        
        public LabTest()
        {
            LabTestRequestItems = new HashSet<LabTestRequestItem>();
            IsActive = true;
        }
    }
}
