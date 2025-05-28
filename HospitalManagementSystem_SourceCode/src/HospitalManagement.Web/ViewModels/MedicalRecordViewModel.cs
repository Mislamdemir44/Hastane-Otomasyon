using System;
using System.ComponentModel.DataAnnotations;

namespace HospitalManagement.Web.ViewModels
{
    public class MedicalRecordViewModel
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Hasta zorunludur")]
        [Display(Name = "Hasta")]
        public Guid PatientId { get; set; }

        [Required(ErrorMessage = "Doktor zorunludur")]
        [Display(Name = "Doktor")]
        public Guid DoctorId { get; set; }

        [Required(ErrorMessage = "Randevu zorunludur")]
        [Display(Name = "Randevu")]
        public Guid AppointmentId { get; set; }

        [Required(ErrorMessage = "Ziyaret tarihi zorunludur")]
        [DataType(DataType.Date)]
        [Display(Name = "Ziyaret Tarihi")]
        public DateTime VisitDate { get; set; }

        [Required(ErrorMessage = "Şikayet zorunludur")]
        [StringLength(1000, ErrorMessage = "Şikayet en fazla 1000 karakter olabilir")]
        [Display(Name = "Şikayet")]
        public string ChiefComplaint { get; set; }

        [Required(ErrorMessage = "Teşhis zorunludur")]
        [StringLength(1000, ErrorMessage = "Teşhis en fazla 1000 karakter olabilir")]
        [Display(Name = "Teşhis")]
        public string Diagnosis { get; set; }

        [StringLength(2000, ErrorMessage = "Tedavi en fazla 2000 karakter olabilir")]
        [Display(Name = "Tedavi")]
        public string Treatment { get; set; }

        [StringLength(2000, ErrorMessage = "Reçete en fazla 2000 karakter olabilir")]
        [Display(Name = "Reçete")]
        public string Prescription { get; set; }

        [StringLength(2000, ErrorMessage = "Notlar en fazla 2000 karakter olabilir")]
        [Display(Name = "Notlar")]
        public string Notes { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Kontrol Tarihi")]
        public DateTime? FollowUpDate { get; set; }

        // Navigation properties for display
        [Display(Name = "Hasta Adı")]
        public string PatientName { get; set; }

        [Display(Name = "Doktor Adı")]
        public string DoctorName { get; set; }

        [Display(Name = "Randevu Tarihi")]
        public DateTime AppointmentDate { get; set; }
    }
}
