using System;
using System.ComponentModel.DataAnnotations;

namespace HospitalManagement.Web.ViewModels
{
    public class LabTestRequestItemViewModel
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Test isteği zorunludur")]
        [Display(Name = "Test İsteği")]
        public Guid LabTestRequestId { get; set; }

        [Required(ErrorMessage = "Test zorunludur")]
        [Display(Name = "Test")]
        public Guid LabTestId { get; set; }

        [Display(Name = "Durum")]
        public string Status { get; set; } = "Pending";

        [Display(Name = "Sonuç Değeri")]
        public string ResultValue { get; set; }

        [Display(Name = "Referans Aralığı")]
        public string ReferenceRange { get; set; }

        [Display(Name = "Açıklamalar")]
        public string Remarks { get; set; }

        [Display(Name = "Teknisyen")]
        public Guid? TechnicianId { get; set; }

        [Display(Name = "Doğrulayan")]
        public Guid? VerifiedById { get; set; }

        [Display(Name = "Sonuç Tarihi")]
        public DateTime? ResultDate { get; set; }

        // Navigation properties for display
        [Display(Name = "Test Adı")]
        public string LabTestName { get; set; }

        [Display(Name = "Teknisyen Adı")]
        public string TechnicianName { get; set; }

        [Display(Name = "Doğrulayan Adı")]
        public string VerifiedByName { get; set; }

        // For form submission
        [Display(Name = "İstek ID")]
        public Guid RequestId { get; set; }

        // Status display options
        public static readonly string[] StatusOptions = new[] { "Pending", "In Progress", "Completed", "Verified", "Cancelled" };
    }
}
