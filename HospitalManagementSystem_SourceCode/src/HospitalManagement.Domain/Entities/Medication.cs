using System;
using System.Collections.Generic;

namespace HospitalManagement.Domain.Entities
{
    public class Medication : BaseEntity
    {
        public string Name { get; set; }
        public string GenericName { get; set; }
        public string Description { get; set; }
        public string DosageForm { get; set; }
        public string Strength { get; set; }
        public string Manufacturer { get; set; }
        public int StockQuantity { get; set; }
        public decimal UnitPrice { get; set; }
        public bool IsActive { get; set; }
        
        // Navigation properties
        public virtual ICollection<PrescriptionItem> PrescriptionItems { get; set; }
        
        public Medication()
        {
            PrescriptionItems = new HashSet<PrescriptionItem>();
            IsActive = true;
        }
    }
}
