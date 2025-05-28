using System;

namespace HospitalManagement.Domain.Entities
{
    public class InvoiceItem : BaseEntity
    {
        public Guid InvoiceId { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Discount { get; set; }
        public decimal TaxRate { get; set; }
        public decimal TotalPrice { get; set; }
        public string ServiceType { get; set; } // Consultation, Lab Test, Medication, Procedure
        public Guid? ServiceId { get; set; }
        
        // Navigation properties
        public virtual Invoice Invoice { get; set; }
    }
}
