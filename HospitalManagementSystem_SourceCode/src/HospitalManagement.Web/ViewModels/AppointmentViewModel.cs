using System;
using System.ComponentModel.DataAnnotations;

namespace HospitalManagement.Web.ViewModels
{
    public class AppointmentViewModel
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Hasta zorunludur")]
        [Display(Name = "Hasta")]
        public Guid PatientId { get; set; }

        [Required(ErrorMessage = "Doktor zorunludur")]
        [Display(Name = "Doktor")]
        public Guid DoctorId { get; set; }

        [Required(ErrorMessage = "Departman zorunludur")]
        [Display(Name = "Departman")]
        public Guid DepartmentId { get; set; }

        [Required(ErrorMessage = "Randevu tarihi zorunludur")]
        [DataType(DataType.Date)]
        [Display(Name = "Randevu Tarihi")]
        public DateTime AppointmentDate { get; set; }

        [Required(ErrorMessage = "Başlangıç saati zorunludur")]
        [Display(Name = "Başlangıç Saati")]
        public TimeSpan StartTime { get; set; }

        [Required(ErrorMessage = "Bitiş saati zorunludur")]
        [Display(Name = "Bitiş Saati")]
        public TimeSpan EndTime { get; set; }

        [Display(Name = "Durum")]
        public string Status { get; set; } = "Scheduled";

        [Required(ErrorMessage = "Randevu tipi zorunludur")]
        [Display(Name = "Randevu Tipi")]
        public string Type { get; set; } = "Regular";

        [StringLength(1000, ErrorMessage = "Notlar en fazla 1000 karakter olabilir")]
        [Display(Name = "Notlar")]
        public string Notes { get; set; }

        [Required(ErrorMessage = "Oluşturan kullanıcı zorunludur")]
        [Display(Name = "Oluşturan")]
        public Guid CreatedById { get; set; }

        // Navigation properties for display
        [Display(Name = "Hasta Adı")]
        public string PatientName { get; set; }

        [Display(Name = "Doktor Adı")]
        public string DoctorName { get; set; }

        [Display(Name = "Departman Adı")]
        public string DepartmentName { get; set; }

        [Display(Name = "Oluşturan")]
        public string CreatedByName { get; set; }

        // Computed property for appointment time display
        [Display(Name = "Randevu Zamanı")]
        public string AppointmentTime => $"{StartTime.ToString(@"hh\:mm")} - {EndTime.ToString(@"hh\:mm")}";

        // Status display options
        public static readonly string[] StatusOptions = new[] { "Scheduled", "Completed", "Cancelled", "No-Show" };

        // Type display options
        public static readonly string[] TypeOptions = new[] { "Regular", "Follow-up", "Emergency" };
    }
}
