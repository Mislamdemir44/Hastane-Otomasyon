using System;
using System.ComponentModel.DataAnnotations;

namespace HospitalManagement.Web.ViewModels
{
    public class InvoiceItemViewModel
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Fatura zorunludur")]
        [Display(Name = "Fatura")]
        public Guid InvoiceId { get; set; }

        [Required(ErrorMessage = "Açıklama zorunludur")]
        [StringLength(200, ErrorMessage = "Açıklama en fazla 200 karakter olabilir")]
        [Display(Name = "Açıklama")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Miktar zorunludur")]
        [Range(1, 1000, ErrorMessage = "Miktar 1 ile 1000 arasında olmalıdır")]
        [Display(Name = "Miktar")]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "Birim fiyat zorunludur")]
        [Range(0.01, 100000, ErrorMessage = "Birim fiyat 0.01 ile 100000 arasında olmalıdır")]
        [Display(Name = "Birim Fiyat")]
        public decimal UnitPrice { get; set; }

        [Range(0, 100, ErrorMessage = "İndirim 0 ile 100 arasında olmalıdır")]
        [Display(Name = "İndirim (%)")]
        public decimal Discount { get; set; }

        [Range(0, 100, ErrorMessage = "Vergi oranı 0 ile 100 arasında olmalıdır")]
        [Display(Name = "Vergi Oranı (%)")]
        public decimal TaxRate { get; set; }

        [Display(Name = "Toplam Fiyat")]
        public decimal TotalPrice { get; set; }

        [Display(Name = "Hizmet Tipi")]
        public string ServiceType { get; set; }

        [Display(Name = "Hizmet ID")]
        public Guid? ServiceId { get; set; }

        // Computed property for subtotal (before discount and tax)
        [Display(Name = "Ara Toplam")]
        public decimal Subtotal => Quantity * UnitPrice;

        // Computed property for discount amount
        [Display(Name = "İndirim Tutarı")]
        public decimal DiscountAmount => Subtotal * (Discount / 100);

        // Computed property for tax amount
        [Display(Name = "Vergi Tutarı")]
        public decimal TaxAmount => (Subtotal - DiscountAmount) * (TaxRate / 100);

        // Service type display options
        public static readonly string[] ServiceTypeOptions = new[] { "Consultation", "Procedure", "Laboratory", "Medication", "Room", "Other" };
    }
}
