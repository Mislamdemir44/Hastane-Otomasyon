using System;
using System.ComponentModel.DataAnnotations;

namespace HospitalManagement.Web.ViewModels
{
    public class LabTestResultViewModel
    {
        [Display(Name = "Test Öğesi ID")]
        public Guid ItemId { get; set; }

        [Display(Name = "İstek ID")]
        public Guid RequestId { get; set; }

        [Required(ErrorMessage = "Sonuç değeri zorunludur")]
        [StringLength(200, ErrorMessage = "Sonuç değeri en fazla 200 karakter olabilir")]
        [Display(Name = "Sonuç Değeri")]
        public string ResultValue { get; set; }

        [StringLength(100, ErrorMessage = "Referans aralığı en fazla 100 karakter olabilir")]
        [Display(Name = "Referans Aralığı")]
        public string ReferenceRange { get; set; }

        [StringLength(500, ErrorMessage = "Açıklamalar en fazla 500 karakter olabilir")]
        [Display(Name = "Açıklamalar")]
        public string Remarks { get; set; }

        [Display(Name = "Teknisyen")]
        public Guid? TechnicianId { get; set; }
    }
}
