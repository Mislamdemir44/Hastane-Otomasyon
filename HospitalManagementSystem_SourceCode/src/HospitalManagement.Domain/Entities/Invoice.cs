using System;
using System.Collections.Generic;

namespace HospitalManagement.Domain.Entities
{
    public class Invoice : BaseEntity
    {
        public Guid PatientId { get; set; }
        public string InvoiceNumber { get; set; }
        public DateTime InvoiceDate { get; set; }
        public DateTime DueDate { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal PaidAmount { get; set; }
        public string Status { get; set; } // Pending, Paid, Partially Paid, Overdue, Cancelled
        public string PaymentMethod { get; set; }
        public string Notes { get; set; }
        public Guid CreatedById { get; set; }
        
        // Navigation properties
        public virtual Patient Patient { get; set; }
        public virtual User CreatedBy { get; set; }
        public virtual ICollection<InvoiceItem> InvoiceItems { get; set; }
        public virtual ICollection<Payment> Payments { get; set; }
        
        public Invoice()
        {
            InvoiceItems = new HashSet<InvoiceItem>();
            Payments = new HashSet<Payment>();
            InvoiceDate = DateTime.UtcNow;
            DueDate = DateTime.UtcNow.AddDays(30);
            Status = "Pending";
            PaidAmount = 0;
        }
    }
}
