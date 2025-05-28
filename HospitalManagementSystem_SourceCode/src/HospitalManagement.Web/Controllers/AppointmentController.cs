using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HospitalManagement.Business.Interfaces;
using HospitalManagement.Domain.Entities;
using HospitalManagement.Web.ViewModels;

namespace HospitalManagement.Web.Controllers
{
    public class AppointmentController : Controller
    {
        private readonly IAppointmentService _appointmentService;
        private readonly IPatientService _patientService;
        private readonly IDoctorService _doctorService;

        public AppointmentController(
            IAppointmentService appointmentService,
            IPatientService patientService,
            IDoctorService doctorService)
        {
            _appointmentService = appointmentService;
            _patientService = patientService;
            _doctorService = doctorService;
        }

        // GET: Appointment
        public async Task<IActionResult> Index()
        {
            var appointments = await _appointmentService.GetAllAppointmentsAsync();
            return View(appointments);
        }

        // GET: Appointment/Details/5
        public async Task<IActionResult> Details(Guid id)
        {
            var appointment = await _appointmentService.GetAppointmentByIdAsync(id);
            if (appointment == null)
            {
                return NotFound();
            }

            return View(appointment);
        }

        // GET: Appointment/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Appointment/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AppointmentViewModel appointmentViewModel)
        {
            if (ModelState.IsValid)
            {
                var appointment = new Appointment
                {
                    PatientId = appointmentViewModel.PatientId,
                    DoctorId = appointmentViewModel.DoctorId,
                    DepartmentId = appointmentViewModel.DepartmentId,
                    AppointmentDate = appointmentViewModel.AppointmentDate,
                    StartTime = appointmentViewModel.StartTime,
                    EndTime = appointmentViewModel.EndTime,
                    Type = appointmentViewModel.Type,
                    Notes = appointmentViewModel.Notes,
                    CreatedById = appointmentViewModel.CreatedById
                };

                var result = await _appointmentService.CreateAppointmentAsync(appointment);
                if (result)
                {
                    return RedirectToAction(nameof(Index));
                }
                
                ModelState.AddModelError("", "Selected time slot is not available.");
            }
            
            return View(appointmentViewModel);
        }

        // GET: Appointment/Edit/5
        public async Task<IActionResult> Edit(Guid id)
        {
            var appointment = await _appointmentService.GetAppointmentByIdAsync(id);
            if (appointment == null)
            {
                return NotFound();
            }

            var appointmentViewModel = new AppointmentViewModel
            {
                Id = appointment.Id,
                PatientId = appointment.PatientId,
                DoctorId = appointment.DoctorId,
                DepartmentId = appointment.DepartmentId,
                AppointmentDate = appointment.AppointmentDate,
                StartTime = appointment.StartTime,
                EndTime = appointment.EndTime,
                Status = appointment.Status,
                Type = appointment.Type,
                Notes = appointment.Notes,
                CreatedById = appointment.CreatedById
            };

            return View(appointmentViewModel);
        }

        // POST: Appointment/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, AppointmentViewModel appointmentViewModel)
        {
            if (id != appointmentViewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var appointment = new Appointment
                {
                    Id = appointmentViewModel.Id,
                    PatientId = appointmentViewModel.PatientId,
                    DoctorId = appointmentViewModel.DoctorId,
                    DepartmentId = appointmentViewModel.DepartmentId,
                    AppointmentDate = appointmentViewModel.AppointmentDate,
                    StartTime = appointmentViewModel.StartTime,
                    EndTime = appointmentViewModel.EndTime,
                    Status = appointmentViewModel.Status,
                    Type = appointmentViewModel.Type,
                    Notes = appointmentViewModel.Notes,
                    CreatedById = appointmentViewModel.CreatedById
                };

                var result = await _appointmentService.UpdateAppointmentAsync(appointment);
                if (result)
                {
                    return RedirectToAction(nameof(Index));
                }
                
                ModelState.AddModelError("", "Selected time slot is not available.");
            }
            
            return View(appointmentViewModel);
        }

        // GET: Appointment/Cancel/5
        public async Task<IActionResult> Cancel(Guid id)
        {
            var appointment = await _appointmentService.GetAppointmentByIdAsync(id);
            if (appointment == null)
            {
                return NotFound();
            }

            return View(appointment);
        }

        // POST: Appointment/Cancel/5
        [HttpPost, ActionName("Cancel")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CancelConfirmed(Guid id)
        {
            await _appointmentService.CancelAppointmentAsync(id);
            return RedirectToAction(nameof(Index));
        }

        // POST: Appointment/Complete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Complete(Guid id)
        {
            await _appointmentService.CompleteAppointmentAsync(id);
            return RedirectToAction(nameof(Index));
        }

        // GET: Appointment/ByDate
        public async Task<IActionResult> ByDate(DateTime? date)
        {
            date = date ?? DateTime.Today;
            var appointments = await _appointmentService.GetAppointmentsByDateAsync(date.Value);
            ViewBag.Date = date.Value;
            return View(appointments);
        }

        // GET: Appointment/ByDoctor/5
        public async Task<IActionResult> ByDoctor(Guid id, DateTime? date)
        {
            var doctor = await _doctorService.GetDoctorByIdAsync(id);
            if (doctor == null)
            {
                return NotFound();
            }

            date = date ?? DateTime.Today;
            var appointments = await _appointmentService.GetAppointmentsByDoctorAsync(id);
            ViewBag.Doctor = doctor;
            ViewBag.Date = date.Value;
            return View(appointments);
        }

        // GET: Appointment/ByPatient/5
        public async Task<IActionResult> ByPatient(Guid id)
        {
            var patient = await _patientService.GetPatientByIdAsync(id);
            if (patient == null)
            {
                return NotFound();
            }

            var appointments = await _appointmentService.GetAppointmentsByPatientAsync(id);
            ViewBag.Patient = patient;
            return View(appointments);
        }

        // GET: Appointment/GetAvailableTimeSlots
        [HttpGet]
        public async Task<JsonResult> GetAvailableTimeSlots(Guid doctorId, DateTime date)
        {
            var timeSlots = await _appointmentService.GetAvailableTimeSlotsAsync(doctorId, date);
            return Json(timeSlots);
        }

        // GET: Appointment/CheckAvailability
        [HttpGet]
        public async Task<JsonResult> CheckAvailability(Guid doctorId, DateTime date, TimeSpan startTime, TimeSpan endTime)
        {
            var isAvailable = await _appointmentService.IsTimeSlotAvailableAsync(doctorId, date, startTime, endTime);
            return Json(new { isAvailable });
        }
    }
}
