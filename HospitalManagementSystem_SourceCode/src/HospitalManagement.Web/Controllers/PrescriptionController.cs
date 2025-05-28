using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HospitalManagement.Business.Interfaces;
using HospitalManagement.Domain.Entities;
using HospitalManagement.Web.ViewModels;

namespace HospitalManagement.Web.Controllers
{
    public class PrescriptionController : Controller
    {
        private readonly IPrescriptionService _prescriptionService;
        private readonly IPatientService _patientService;
        private readonly IDoctorService _doctorService;
        private readonly IMedicalRecordService _medicalRecordService;

        public PrescriptionController(
            IPrescriptionService prescriptionService,
            IPatientService patientService,
            IDoctorService doctorService,
            IMedicalRecordService medicalRecordService)
        {
            _prescriptionService = prescriptionService;
            _patientService = patientService;
            _doctorService = doctorService;
            _medicalRecordService = medicalRecordService;
        }

        // GET: Prescription
        public async Task<IActionResult> Index()
        {
            var prescriptions = await _prescriptionService.GetAllPrescriptionsAsync();
            return View(prescriptions);
        }

        // GET: Prescription/Details/5
        public async Task<IActionResult> Details(Guid id)
        {
            var prescription = await _prescriptionService.GetPrescriptionByIdAsync(id);
            if (prescription == null)
            {
                return NotFound();
            }

            return View(prescription);
        }

        // GET: Prescription/Create
        public async Task<IActionResult> Create(Guid medicalRecordId)
        {
            var medicalRecord = await _medicalRecordService.GetMedicalRecordByIdAsync(medicalRecordId);
            if (medicalRecord == null)
            {
                return NotFound();
            }

            var patient = await _patientService.GetPatientByIdAsync(medicalRecord.PatientId);
            var doctor = await _doctorService.GetDoctorByIdAsync(medicalRecord.DoctorId);

            var prescriptionViewModel = new PrescriptionViewModel
            {
                PatientId = medicalRecord.PatientId,
                DoctorId = medicalRecord.DoctorId,
                MedicalRecordId = medicalRecordId,
                PrescriptionDate = DateTime.UtcNow
            };

            ViewBag.Patient = patient;
            ViewBag.Doctor = doctor;
            ViewBag.MedicalRecord = medicalRecord;

            return View(prescriptionViewModel);
        }

        // POST: Prescription/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PrescriptionViewModel prescriptionViewModel)
        {
            if (ModelState.IsValid)
            {
                var prescription = new Prescription
                {
                    PatientId = prescriptionViewModel.PatientId,
                    DoctorId = prescriptionViewModel.DoctorId,
                    MedicalRecordId = prescriptionViewModel.MedicalRecordId,
                    PrescriptionDate = prescriptionViewModel.PrescriptionDate,
                    Status = "Active",
                    Notes = prescriptionViewModel.Notes
                };

                var result = await _prescriptionService.CreatePrescriptionAsync(prescription);
                if (result)
                {
                    return RedirectToAction(nameof(AddItems), new { id = prescription.Id });
                }
                
                ModelState.AddModelError("", "Failed to create prescription.");
            }
            
            var patient = await _patientService.GetPatientByIdAsync(prescriptionViewModel.PatientId);
            var doctor = await _doctorService.GetDoctorByIdAsync(prescriptionViewModel.DoctorId);
            var medicalRecord = await _medicalRecordService.GetMedicalRecordByIdAsync(prescriptionViewModel.MedicalRecordId);

            ViewBag.Patient = patient;
            ViewBag.Doctor = doctor;
            ViewBag.MedicalRecord = medicalRecord;
            
            return View(prescriptionViewModel);
        }

        // GET: Prescription/Edit/5
        public async Task<IActionResult> Edit(Guid id)
        {
            var prescription = await _prescriptionService.GetPrescriptionByIdAsync(id);
            if (prescription == null)
            {
                return NotFound();
            }

            var patient = await _patientService.GetPatientByIdAsync(prescription.PatientId);
            var doctor = await _doctorService.GetDoctorByIdAsync(prescription.DoctorId);
            var medicalRecord = await _medicalRecordService.GetMedicalRecordByIdAsync(prescription.MedicalRecordId);

            var prescriptionViewModel = new PrescriptionViewModel
            {
                Id = prescription.Id,
                PatientId = prescription.PatientId,
                DoctorId = prescription.DoctorId,
                MedicalRecordId = prescription.MedicalRecordId,
                PrescriptionDate = prescription.PrescriptionDate,
                Status = prescription.Status,
                Notes = prescription.Notes
            };

            ViewBag.Patient = patient;
            ViewBag.Doctor = doctor;
            ViewBag.MedicalRecord = medicalRecord;

            return View(prescriptionViewModel);
        }

        // POST: Prescription/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, PrescriptionViewModel prescriptionViewModel)
        {
            if (id != prescriptionViewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var prescription = new Prescription
                {
                    Id = prescriptionViewModel.Id,
                    PatientId = prescriptionViewModel.PatientId,
                    DoctorId = prescriptionViewModel.DoctorId,
                    MedicalRecordId = prescriptionViewModel.MedicalRecordId,
                    PrescriptionDate = prescriptionViewModel.PrescriptionDate,
                    Status = prescriptionViewModel.Status,
                    Notes = prescriptionViewModel.Notes
                };

                var result = await _prescriptionService.UpdatePrescriptionAsync(prescription);
                if (result)
                {
                    return RedirectToAction(nameof(Details), new { id = prescription.Id });
                }
                
                ModelState.AddModelError("", "Failed to update prescription.");
            }
            
            var patient = await _patientService.GetPatientByIdAsync(prescriptionViewModel.PatientId);
            var doctor = await _doctorService.GetDoctorByIdAsync(prescriptionViewModel.DoctorId);
            var medicalRecord = await _medicalRecordService.GetMedicalRecordByIdAsync(prescriptionViewModel.MedicalRecordId);

            ViewBag.Patient = patient;
            ViewBag.Doctor = doctor;
            ViewBag.MedicalRecord = medicalRecord;
            
            return View(prescriptionViewModel);
        }

        // GET: Prescription/Delete/5
        public async Task<IActionResult> Delete(Guid id)
        {
            var prescription = await _prescriptionService.GetPrescriptionByIdAsync(id);
            if (prescription == null)
            {
                return NotFound();
            }

            return View(prescription);
        }

        // POST: Prescription/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            await _prescriptionService.DeletePrescriptionAsync(id);
            return RedirectToAction(nameof(Index));
        }

        // GET: Prescription/AddItems/5
        public async Task<IActionResult> AddItems(Guid id)
        {
            var prescription = await _prescriptionService.GetPrescriptionByIdAsync(id);
            if (prescription == null)
            {
                return NotFound();
            }

            ViewBag.Prescription = prescription;
            return View();
        }

        // POST: Prescription/AddItem
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddItem(PrescriptionItemViewModel itemViewModel)
        {
            if (ModelState.IsValid)
            {
                var item = new PrescriptionItem
                {
                    PrescriptionId = itemViewModel.PrescriptionId,
                    MedicationId = itemViewModel.MedicationId,
                    Dosage = itemViewModel.Dosage,
                    Frequency = itemViewModel.Frequency,
                    Duration = itemViewModel.Duration,
                    Quantity = itemViewModel.Quantity,
                    Instructions = itemViewModel.Instructions
                };

                var result = await _prescriptionService.AddPrescriptionItemAsync(item);
                if (result)
                {
                    return RedirectToAction(nameof(Details), new { id = itemViewModel.PrescriptionId });
                }
                
                ModelState.AddModelError("", "Failed to add prescription item.");
            }
            
            var prescription = await _prescriptionService.GetPrescriptionByIdAsync(itemViewModel.PrescriptionId);
            ViewBag.Prescription = prescription;
            
            return View("AddItems", itemViewModel);
        }

        // POST: Prescription/MarkAsFilled/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MarkAsFilled(Guid id)
        {
            await _prescriptionService.MarkPrescriptionAsFilledAsync(id);
            return RedirectToAction(nameof(Details), new { id });
        }

        // POST: Prescription/MarkAsExpired/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MarkAsExpired(Guid id)
        {
            await _prescriptionService.MarkPrescriptionAsExpiredAsync(id);
            return RedirectToAction(nameof(Details), new { id });
        }

        // GET: Prescription/ByPatient/5
        public async Task<IActionResult> ByPatient(Guid id)
        {
            var patient = await _patientService.GetPatientByIdAsync(id);
            if (patient == null)
            {
                return NotFound();
            }

            var prescriptions = await _prescriptionService.GetPrescriptionsByPatientAsync(id);
            ViewBag.Patient = patient;
            return View(prescriptions);
        }

        // GET: Prescription/ByDoctor/5
        public async Task<IActionResult> ByDoctor(Guid id)
        {
            var doctor = await _doctorService.GetDoctorByIdAsync(id);
            if (doctor == null)
            {
                return NotFound();
            }

            var prescriptions = await _prescriptionService.GetPrescriptionsByDoctorAsync(id);
            ViewBag.Doctor = doctor;
            return View(prescriptions);
        }
    }
}
