using System;
using System.ComponentModel.DataAnnotations;

namespace HospitalManagement.Web.ViewModels
{
    public class InvoiceViewModel
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Hasta zorunludur")]
        [Display(Name = "Hasta")]
        public Guid PatientId { get; set; }

        [Display(Name = "Fatura Numarası")]
        public string InvoiceNumber { get; set; }

        [Required(ErrorMessage = "Fatura tarihi zorunludur")]
        [DataType(DataType.Date)]
        [Display(Name = "Fatura Tarihi")]
        public DateTime InvoiceDate { get; set; }

        [Required(ErrorMessage = "Son ödeme tarihi zorunludur")]
        [DataType(DataType.Date)]
        [Display(Name = "Son Ödeme Tarihi")]
        public DateTime DueDate { get; set; }

        [Display(Name = "Toplam Tutar")]
        public decimal TotalAmount { get; set; }

        [Display(Name = "Ödenen Tutar")]
        public decimal PaidAmount { get; set; }

        [Display(Name = "Durum")]
        public string Status { get; set; } = "Pending";

        [Display(Name = "Ödeme Yöntemi")]
        public string PaymentMethod { get; set; }

        [StringLength(1000, ErrorMessage = "Notlar en fazla 1000 karakter olabilir")]
        [Display(Name = "Notlar")]
        public string Notes { get; set; }

        [Required(ErrorMessage = "Oluşturan kullanıcı zorunludur")]
        [Display(Name = "Oluşturan")]
        public Guid CreatedById { get; set; }

        // Navigation properties for display
        [Display(Name = "Hasta Adı")]
        public string PatientName { get; set; }

        [Display(Name = "Oluşturan")]
        public string CreatedByName { get; set; }

        // Computed property for outstanding amount
        [Display(Name = "Kalan Tutar")]
        public decimal OutstandingAmount => TotalAmount - PaidAmount;

        // Status display options
        public static readonly string[] StatusOptions = new[] { "Pending", "Paid", "Partially Paid", "Overdue", "Cancelled" };

        // Payment method display options
        public static readonly string[] PaymentMethodOptions = new[] { "Cash", "Credit Card", "Debit Card", "Bank Transfer", "Insurance", "Other" };
    }
}
