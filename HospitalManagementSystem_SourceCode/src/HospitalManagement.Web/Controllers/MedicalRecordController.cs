using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HospitalManagement.Business.Interfaces;
using HospitalManagement.Domain.Entities;
using HospitalManagement.Web.ViewModels;

namespace HospitalManagement.Web.Controllers
{
    public class MedicalRecordController : Controller
    {
        private readonly IMedicalRecordService _medicalRecordService;
        private readonly IPatientService _patientService;
        private readonly IDoctorService _doctorService;
        private readonly IAppointmentService _appointmentService;

        public MedicalRecordController(
            IMedicalRecordService medicalRecordService,
            IPatientService patientService,
            IDoctorService doctorService,
            IAppointmentService appointmentService)
        {
            _medicalRecordService = medicalRecordService;
            _patientService = patientService;
            _doctorService = doctorService;
            _appointmentService = appointmentService;
        }

        // GET: MedicalRecord
        public async Task<IActionResult> Index()
        {
            var medicalRecords = await _medicalRecordService.GetAllMedicalRecordsAsync();
            return View(medicalRecords);
        }

        // GET: MedicalRecord/Details/5
        public async Task<IActionResult> Details(Guid id)
        {
            var medicalRecord = await _medicalRecordService.GetMedicalRecordByIdAsync(id);
            if (medicalRecord == null)
            {
                return NotFound();
            }

            return View(medicalRecord);
        }

        // GET: MedicalRecord/Create
        public async Task<IActionResult> Create(Guid appointmentId)
        {
            var appointment = await _appointmentService.GetAppointmentByIdAsync(appointmentId);
            if (appointment == null)
            {
                return NotFound();
            }

            var patient = await _patientService.GetPatientByIdAsync(appointment.PatientId);
            var doctor = await _doctorService.GetDoctorByIdAsync(appointment.DoctorId);

            var medicalRecordViewModel = new MedicalRecordViewModel
            {
                PatientId = appointment.PatientId,
                DoctorId = appointment.DoctorId,
                AppointmentId = appointmentId,
                VisitDate = appointment.AppointmentDate
            };

            ViewBag.Patient = patient;
            ViewBag.Doctor = doctor;
            ViewBag.Appointment = appointment;

            return View(medicalRecordViewModel);
        }

        // POST: MedicalRecord/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MedicalRecordViewModel medicalRecordViewModel)
        {
            if (ModelState.IsValid)
            {
                var medicalRecord = new MedicalRecord
                {
                    PatientId = medicalRecordViewModel.PatientId,
                    DoctorId = medicalRecordViewModel.DoctorId,
                    AppointmentId = medicalRecordViewModel.AppointmentId,
                    VisitDate = medicalRecordViewModel.VisitDate,
                    ChiefComplaint = medicalRecordViewModel.ChiefComplaint,
                    Diagnosis = medicalRecordViewModel.Diagnosis,
                    Treatment = medicalRecordViewModel.Treatment,
                    Prescription = medicalRecordViewModel.Prescription,
                    Notes = medicalRecordViewModel.Notes,
                    FollowUpDate = medicalRecordViewModel.FollowUpDate
                };

                var result = await _medicalRecordService.CreateMedicalRecordAsync(medicalRecord);
                if (result)
                {
                    // Mark appointment as completed
                    await _appointmentService.CompleteAppointmentAsync(medicalRecordViewModel.AppointmentId);
                    return RedirectToAction(nameof(Details), new { id = medicalRecord.Id });
                }
                
                ModelState.AddModelError("", "Failed to create medical record.");
            }
            
            var patient = await _patientService.GetPatientByIdAsync(medicalRecordViewModel.PatientId);
            var doctor = await _doctorService.GetDoctorByIdAsync(medicalRecordViewModel.DoctorId);
            var appointment = await _appointmentService.GetAppointmentByIdAsync(medicalRecordViewModel.AppointmentId);

            ViewBag.Patient = patient;
            ViewBag.Doctor = doctor;
            ViewBag.Appointment = appointment;
            
            return View(medicalRecordViewModel);
        }

        // GET: MedicalRecord/Edit/5
        public async Task<IActionResult> Edit(Guid id)
        {
            var medicalRecord = await _medicalRecordService.GetMedicalRecordByIdAsync(id);
            if (medicalRecord == null)
            {
                return NotFound();
            }

            var patient = await _patientService.GetPatientByIdAsync(medicalRecord.PatientId);
            var doctor = await _doctorService.GetDoctorByIdAsync(medicalRecord.DoctorId);
            var appointment = await _appointmentService.GetAppointmentByIdAsync(medicalRecord.AppointmentId);

            var medicalRecordViewModel = new MedicalRecordViewModel
            {
                Id = medicalRecord.Id,
                PatientId = medicalRecord.PatientId,
                DoctorId = medicalRecord.DoctorId,
                AppointmentId = medicalRecord.AppointmentId,
                VisitDate = medicalRecord.VisitDate,
                ChiefComplaint = medicalRecord.ChiefComplaint,
                Diagnosis = medicalRecord.Diagnosis,
                Treatment = medicalRecord.Treatment,
                Prescription = medicalRecord.Prescription,
                Notes = medicalRecord.Notes,
                FollowUpDate = medicalRecord.FollowUpDate
            };

            ViewBag.Patient = patient;
            ViewBag.Doctor = doctor;
            ViewBag.Appointment = appointment;

            return View(medicalRecordViewModel);
        }

        // POST: MedicalRecord/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, MedicalRecordViewModel medicalRecordViewModel)
        {
            if (id != medicalRecordViewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var medicalRecord = new MedicalRecord
                {
                    Id = medicalRecordViewModel.Id,
                    PatientId = medicalRecordViewModel.PatientId,
                    DoctorId = medicalRecordViewModel.DoctorId,
                    AppointmentId = medicalRecordViewModel.AppointmentId,
                    VisitDate = medicalRecordViewModel.VisitDate,
                    ChiefComplaint = medicalRecordViewModel.ChiefComplaint,
                    Diagnosis = medicalRecordViewModel.Diagnosis,
                    Treatment = medicalRecordViewModel.Treatment,
                    Prescription = medicalRecordViewModel.Prescription,
                    Notes = medicalRecordViewModel.Notes,
                    FollowUpDate = medicalRecordViewModel.FollowUpDate
                };

                var result = await _medicalRecordService.UpdateMedicalRecordAsync(medicalRecord);
                if (result)
                {
                    return RedirectToAction(nameof(Details), new { id = medicalRecord.Id });
                }
                
                ModelState.AddModelError("", "Failed to update medical record.");
            }
            
            var patient = await _patientService.GetPatientByIdAsync(medicalRecordViewModel.PatientId);
            var doctor = await _doctorService.GetDoctorByIdAsync(medicalRecordViewModel.DoctorId);
            var appointment = await _appointmentService.GetAppointmentByIdAsync(medicalRecordViewModel.AppointmentId);

            ViewBag.Patient = patient;
            ViewBag.Doctor = doctor;
            ViewBag.Appointment = appointment;
            
            return View(medicalRecordViewModel);
        }

        // GET: MedicalRecord/Delete/5
        public async Task<IActionResult> Delete(Guid id)
        {
            var medicalRecord = await _medicalRecordService.GetMedicalRecordByIdAsync(id);
            if (medicalRecord == null)
            {
                return NotFound();
            }

            return View(medicalRecord);
        }

        // POST: MedicalRecord/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            await _medicalRecordService.DeleteMedicalRecordAsync(id);
            return RedirectToAction(nameof(Index));
        }

        // GET: MedicalRecord/ByPatient/5
        public async Task<IActionResult> ByPatient(Guid id)
        {
            var patient = await _patientService.GetPatientByIdAsync(id);
            if (patient == null)
            {
                return NotFound();
            }

            var medicalRecords = await _medicalRecordService.GetMedicalRecordsByPatientAsync(id);
            ViewBag.Patient = patient;
            return View(medicalRecords);
        }

        // GET: MedicalRecord/ByDoctor/5
        public async Task<IActionResult> ByDoctor(Guid id)
        {
            var doctor = await _doctorService.GetDoctorByIdAsync(id);
            if (doctor == null)
            {
                return NotFound();
            }

            var medicalRecords = await _medicalRecordService.GetMedicalRecordsByDoctorAsync(id);
            ViewBag.Doctor = doctor;
            return View(medicalRecords);
        }
    }
}
