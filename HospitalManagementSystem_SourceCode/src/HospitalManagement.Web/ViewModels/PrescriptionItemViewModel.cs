using System;
using System.ComponentModel.DataAnnotations;

namespace HospitalManagement.Web.ViewModels
{
    public class PrescriptionItemViewModel
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Reçete zorunludur")]
        [Display(Name = "Reçete")]
        public Guid PrescriptionId { get; set; }

        [Required(ErrorMessage = "İlaç zorunludur")]
        [Display(Name = "İlaç")]
        public Guid MedicationId { get; set; }

        [Required(ErrorMessage = "Dozaj zorunludur")]
        [StringLength(100, ErrorMessage = "Dozaj en fazla 100 karakter olabilir")]
        [Display(Name = "Dozaj")]
        public string Dosage { get; set; }

        [Required(ErrorMessage = "Kullanım sıklığı zorunludur")]
        [StringLength(100, ErrorMessage = "Kullanım sıklığı en fazla 100 karakter olabilir")]
        [Display(Name = "Kullanım Sıklığı")]
        public string Frequency { get; set; }

        [Required(ErrorMessage = "Kullanım süresi zorunludur")]
        [StringLength(100, ErrorMessage = "Kullanım süresi en fazla 100 karakter olabilir")]
        [Display(Name = "Kullanım Süresi")]
        public string Duration { get; set; }

        [Required(ErrorMessage = "Miktar zorunludur")]
        [Range(1, 1000, ErrorMessage = "Miktar 1 ile 1000 arasında olmalıdır")]
        [Display(Name = "Miktar")]
        public int Quantity { get; set; }

        [StringLength(500, ErrorMessage = "Talimatlar en fazla 500 karakter olabilir")]
        [Display(Name = "Talimatlar")]
        public string Instructions { get; set; }

        // Navigation properties for display
        [Display(Name = "İlaç Adı")]
        public string MedicationName { get; set; }
    }
}
