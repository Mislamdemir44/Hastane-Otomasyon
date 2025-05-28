using System;
using System.ComponentModel.DataAnnotations;

namespace HospitalManagement.Web.ViewModels
{
    public class LabTestViewModel
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Test adı zorunludur")]
        [StringLength(100, ErrorMessage = "Test adı en fazla 100 karakter olabilir")]
        [Display(Name = "Test Adı")]
        public string Name { get; set; }

        [StringLength(500, ErrorMessage = "Açıklama en fazla 500 karakter olabilir")]
        [Display(Name = "Açıklama")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Departman zorunludur")]
        [StringLength(100, ErrorMessage = "Departman en fazla 100 karakter olabilir")]
        [Display(Name = "Departman")]
        public string Department { get; set; }

        [Required(ErrorMessage = "Fiyat zorunludur")]
        [Range(0.01, 10000, ErrorMessage = "Fiyat 0.01 ile 10000 arasında olmalıdır")]
        [Display(Name = "Fiyat")]
        public decimal Price { get; set; }

        [Display(Name = "Aktif")]
        public bool IsActive { get; set; } = true;
    }
}
