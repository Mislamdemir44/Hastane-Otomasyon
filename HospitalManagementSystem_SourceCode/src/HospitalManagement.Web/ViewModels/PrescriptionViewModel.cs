using System;
using System.ComponentModel.DataAnnotations;

namespace HospitalManagement.Web.ViewModels
{
    public class PrescriptionViewModel
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Hasta zorunludur")]
        [Display(Name = "Hasta")]
        public Guid PatientId { get; set; }

        [Required(ErrorMessage = "Doktor zorunludur")]
        [Display(Name = "Doktor")]
        public Guid DoctorId { get; set; }

        [Required(ErrorMessage = "Tıbbi kayıt zorunludur")]
        [Display(Name = "Tıbbi Kayıt")]
        public Guid MedicalRecordId { get; set; }

        [Required(ErrorMessage = "Reçete tarihi zorunludur")]
        [DataType(DataType.Date)]
        [Display(Name = "Reçete Tarihi")]
        public DateTime PrescriptionDate { get; set; }

        [Display(Name = "Durum")]
        public string Status { get; set; } = "Active";

        [StringLength(1000, ErrorMessage = "Notlar en fazla 1000 karakter olabilir")]
        [Display(Name = "Notlar")]
        public string Notes { get; set; }

        // Navigation properties for display
        [Display(Name = "Hasta Adı")]
        public string PatientName { get; set; }

        [Display(Name = "Doktor Adı")]
        public string DoctorName { get; set; }

        // Status display options
        public static readonly string[] StatusOptions = new[] { "Active", "Filled", "Expired" };
    }
}
