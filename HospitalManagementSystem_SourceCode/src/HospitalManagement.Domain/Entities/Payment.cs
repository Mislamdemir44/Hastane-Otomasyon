using System;

namespace HospitalManagement.Domain.Entities
{
    public class Payment : BaseEntity
    {
        public Guid InvoiceId { get; set; }
        public DateTime PaymentDate { get; set; }
        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; }
        public string TransactionId { get; set; }
        public string Status { get; set; } // Completed, Failed, Refunded
        public string Notes { get; set; }
        public Guid ReceivedById { get; set; }
        
        // Navigation properties
        public virtual Invoice Invoice { get; set; }
        public virtual User ReceivedBy { get; set; }
        
        public Payment()
        {
            PaymentDate = DateTime.UtcNow;
            Status = "Completed";
        }
    }
}
