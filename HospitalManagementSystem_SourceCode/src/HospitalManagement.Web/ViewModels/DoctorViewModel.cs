using System;
using System.ComponentModel.DataAnnotations;

namespace HospitalManagement.Web.ViewModels
{
    public class DoctorViewModel
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Kullanıcı ID zorunludur")]
        [Display(Name = "Kullanıcı ID")]
        public Guid UserId { get; set; }

        [Required(ErrorMessage = "Departman zorunludur")]
        [Display(Name = "Departman")]
        public Guid DepartmentId { get; set; }

        [Required(ErrorMessage = "Uzmanlık alanı zorunludur")]
        [Display(Name = "Uzmanlık Alanı")]
        public Guid SpecialtyId { get; set; }

        [Required(ErrorMessage = "Lisans numarası zorunludur")]
        [StringLength(50, ErrorMessage = "Lisans numarası en fazla 50 karakter olabilir")]
        [Display(Name = "Lisans Numarası")]
        public string LicenseNumber { get; set; }

        [StringLength(500, ErrorMessage = "Eğitim bilgisi en fazla 500 karakter olabilir")]
        [Display(Name = "Eğitim")]
        public string Education { get; set; }

        [StringLength(2000, ErrorMessage = "Biyografi en fazla 2000 karakter olabilir")]
        [Display(Name = "Biyografi")]
        public string Biography { get; set; }

        [Required(ErrorMessage = "Konsültasyon ücreti zorunludur")]
        [Range(0, 10000, ErrorMessage = "Konsültasyon ücreti 0 ile 10000 arasında olmalıdır")]
        [Display(Name = "Konsültasyon Ücreti")]
        public decimal ConsultationFee { get; set; }

        [Display(Name = "Randevu Alınabilir")]
        public bool IsAvailableForAppointment { get; set; } = true;

        // Navigation properties for display
        [Display(Name = "Kullanıcı Adı")]
        public string UserName { get; set; }

        [Display(Name = "Ad")]
        public string FirstName { get; set; }

        [Display(Name = "Soyad")]
        public string LastName { get; set; }

        [Display(Name = "Departman Adı")]
        public string DepartmentName { get; set; }

        [Display(Name = "Uzmanlık Alanı")]
        public string SpecialtyName { get; set; }

        // Computed property for full name
        [Display(Name = "Doktor Adı")]
        public string FullName => $"Dr. {FirstName} {LastName}";
    }
}
