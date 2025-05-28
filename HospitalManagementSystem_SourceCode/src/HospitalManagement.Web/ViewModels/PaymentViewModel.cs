using System;
using System.ComponentModel.DataAnnotations;

namespace HospitalManagement.Web.ViewModels
{
    public class PaymentViewModel
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Fatura zorunludur")]
        [Display(Name = "Fatura")]
        public Guid InvoiceId { get; set; }

        [Required(ErrorMessage = "Ödeme tarihi zorunludur")]
        [DataType(DataType.Date)]
        [Display(Name = "Ödeme Tarihi")]
        public DateTime PaymentDate { get; set; }

        [Required(ErrorMessage = "Tutar zorunludur")]
        [Range(0.01, 1000000, ErrorMessage = "Tutar 0.01 ile 1000000 arasında olmalıdır")]
        [Display(Name = "Tutar")]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "Ödeme yöntemi zorunludur")]
        [Display(Name = "Ödeme Yöntemi")]
        public string PaymentMethod { get; set; }

        [StringLength(100, ErrorMessage = "İşlem numarası en fazla 100 karakter olabilir")]
        [Display(Name = "İşlem Numarası")]
        public string TransactionId { get; set; }

        [Display(Name = "Durum")]
        public string Status { get; set; } = "Completed";

        [StringLength(500, ErrorMessage = "Notlar en fazla 500 karakter olabilir")]
        [Display(Name = "Notlar")]
        public string Notes { get; set; }

        [Required(ErrorMessage = "Tahsilat yapan kullanıcı zorunludur")]
        [Display(Name = "Tahsilat Yapan")]
        public Guid ReceivedById { get; set; }

        // Navigation properties for display
        [Display(Name = "Fatura Numarası")]
        public string InvoiceNumber { get; set; }

        [Display(Name = "Hasta Adı")]
        public string PatientName { get; set; }

        [Display(Name = "Tahsilat Yapan")]
        public string ReceivedByName { get; set; }

        // Payment method display options
        public static readonly string[] PaymentMethodOptions = new[] { "Cash", "Credit Card", "Debit Card", "Bank Transfer", "Insurance", "Other" };

        // Status display options
        public static readonly string[] StatusOptions = new[] { "Completed", "Pending", "Failed", "Refunded" };
    }
}
