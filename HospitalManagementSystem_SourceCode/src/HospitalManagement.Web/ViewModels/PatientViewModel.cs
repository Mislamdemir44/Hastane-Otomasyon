using System;
using System.ComponentModel.DataAnnotations;

namespace HospitalManagement.Web.ViewModels
{
    public class PatientViewModel
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "TC Kimlik Numarası zorunludur")]
        [StringLength(20, ErrorMessage = "TC Kimlik Numarası en fazla 20 karakter olabilir")]
        [Display(Name = "TC Kimlik Numarası")]
        public string IdentityNumber { get; set; }

        [Required(ErrorMessage = "Ad zorunludur")]
        [StringLength(50, ErrorMessage = "Ad en fazla 50 karakter olabilir")]
        [Display(Name = "Ad")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Soyad zorunludur")]
        [StringLength(50, ErrorMessage = "Soyad en fazla 50 karakter olabilir")]
        [Display(Name = "Soyad")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Doğum tarihi zorunludur")]
        [DataType(DataType.Date)]
        [Display(Name = "Doğum Tarihi")]
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "Cinsiyet zorunludur")]
        [Display(Name = "Cinsiyet")]
        public string Gender { get; set; }

        [Display(Name = "Kan Grubu")]
        public string BloodType { get; set; }

        [StringLength(500, ErrorMessage = "Adres en fazla 500 karakter olabilir")]
        [Display(Name = "Adres")]
        public string Address { get; set; }

        [StringLength(50, ErrorMessage = "Şehir en fazla 50 karakter olabilir")]
        [Display(Name = "Şehir")]
        public string City { get; set; }

        [StringLength(50, ErrorMessage = "Ülke en fazla 50 karakter olabilir")]
        [Display(Name = "Ülke")]
        public string Country { get; set; }

        [Required(ErrorMessage = "Telefon numarası zorunludur")]
        [StringLength(20, ErrorMessage = "Telefon numarası en fazla 20 karakter olabilir")]
        [Phone(ErrorMessage = "Geçerli bir telefon numarası giriniz")]
        [Display(Name = "Telefon Numarası")]
        public string PhoneNumber { get; set; }

        [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi giriniz")]
        [StringLength(100, ErrorMessage = "E-posta en fazla 100 karakter olabilir")]
        [Display(Name = "E-posta")]
        public string Email { get; set; }

        [StringLength(100, ErrorMessage = "Acil durum kişisi en fazla 100 karakter olabilir")]
        [Display(Name = "Acil Durum Kişisi")]
        public string EmergencyContactName { get; set; }

        [StringLength(20, ErrorMessage = "Acil durum telefonu en fazla 20 karakter olabilir")]
        [Phone(ErrorMessage = "Geçerli bir telefon numarası giriniz")]
        [Display(Name = "Acil Durum Telefonu")]
        public string EmergencyContactPhone { get; set; }

        [StringLength(100, ErrorMessage = "Sigorta şirketi en fazla 100 karakter olabilir")]
        [Display(Name = "Sigorta Şirketi")]
        public string InsuranceProvider { get; set; }

        [StringLength(50, ErrorMessage = "Sigorta numarası en fazla 50 karakter olabilir")]
        [Display(Name = "Sigorta Numarası")]
        public string InsuranceNumber { get; set; }

        [Display(Name = "Aktif")]
        public bool IsActive { get; set; } = true;

        // Computed property for full name
        [Display(Name = "Hasta Adı")]
        public string FullName => $"{FirstName} {LastName}";

        // Computed property for age
        [Display(Name = "Yaş")]
        public int Age => CalculateAge(DateOfBirth);

        private int CalculateAge(DateTime birthDate)
        {
            var today = DateTime.Today;
            var age = today.Year - birthDate.Year;
            if (birthDate.Date > today.AddYears(-age)) age--;
            return age;
        }
    }
}
