using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HospitalManagement.Business.Interfaces;
using HospitalManagement.Domain.Entities;
using HospitalManagement.Web.ViewModels;

namespace HospitalManagement.Web.Controllers
{
    public class PatientController : Controller
    {
        private readonly IPatientService _patientService;

        public PatientController(IPatientService patientService)
        {
            _patientService = patientService;
        }

        // GET: Patient
        public async Task<IActionResult> Index()
        {
            var patients = await _patientService.GetAllPatientsAsync();
            return View(patients);
        }

        // GET: Patient/Details/5
        public async Task<IActionResult> Details(Guid id)
        {
            var patient = await _patientService.GetPatientByIdAsync(id);
            if (patient == null)
            {
                return NotFound();
            }

            return View(patient);
        }

        // GET: Patient/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Patient/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PatientViewModel patientViewModel)
        {
            if (ModelState.IsValid)
            {
                var patient = new Patient
                {
                    IdentityNumber = patientViewModel.IdentityNumber,
                    FirstName = patientViewModel.FirstName,
                    LastName = patientViewModel.LastName,
                    DateOfBirth = patientViewModel.DateOfBirth,
                    Gender = patientViewModel.Gender,
                    BloodType = patientViewModel.BloodType,
                    Address = patientViewModel.Address,
                    City = patientViewModel.City,
                    Country = patientViewModel.Country,
                    PhoneNumber = patientViewModel.PhoneNumber,
                    Email = patientViewModel.Email,
                    EmergencyContactName = patientViewModel.EmergencyContactName,
                    EmergencyContactPhone = patientViewModel.EmergencyContactPhone,
                    InsuranceProvider = patientViewModel.InsuranceProvider,
                    InsuranceNumber = patientViewModel.InsuranceNumber
                };

                var result = await _patientService.CreatePatientAsync(patient);
                if (result)
                {
                    return RedirectToAction(nameof(Index));
                }
                
                ModelState.AddModelError("", "Identity Number already exists.");
            }
            
            return View(patientViewModel);
        }

        // GET: Patient/Edit/5
        public async Task<IActionResult> Edit(Guid id)
        {
            var patient = await _patientService.GetPatientByIdAsync(id);
            if (patient == null)
            {
                return NotFound();
            }

            var patientViewModel = new PatientViewModel
            {
                Id = patient.Id,
                IdentityNumber = patient.IdentityNumber,
                FirstName = patient.FirstName,
                LastName = patient.LastName,
                DateOfBirth = patient.DateOfBirth,
                Gender = patient.Gender,
                BloodType = patient.BloodType,
                Address = patient.Address,
                City = patient.City,
                Country = patient.Country,
                PhoneNumber = patient.PhoneNumber,
                Email = patient.Email,
                EmergencyContactName = patient.EmergencyContactName,
                EmergencyContactPhone = patient.EmergencyContactPhone,
                InsuranceProvider = patient.InsuranceProvider,
                InsuranceNumber = patient.InsuranceNumber
            };

            return View(patientViewModel);
        }

        // POST: Patient/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, PatientViewModel patientViewModel)
        {
            if (id != patientViewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var patient = new Patient
                {
                    Id = patientViewModel.Id,
                    IdentityNumber = patientViewModel.IdentityNumber,
                    FirstName = patientViewModel.FirstName,
                    LastName = patientViewModel.LastName,
                    DateOfBirth = patientViewModel.DateOfBirth,
                    Gender = patientViewModel.Gender,
                    BloodType = patientViewModel.BloodType,
                    Address = patientViewModel.Address,
                    City = patientViewModel.City,
                    Country = patientViewModel.Country,
                    PhoneNumber = patientViewModel.PhoneNumber,
                    Email = patientViewModel.Email,
                    EmergencyContactName = patientViewModel.EmergencyContactName,
                    EmergencyContactPhone = patientViewModel.EmergencyContactPhone,
                    InsuranceProvider = patientViewModel.InsuranceProvider,
                    InsuranceNumber = patientViewModel.InsuranceNumber
                };

                var result = await _patientService.UpdatePatientAsync(patient);
                if (result)
                {
                    return RedirectToAction(nameof(Index));
                }
                
                ModelState.AddModelError("", "Identity Number already exists.");
            }
            
            return View(patientViewModel);
        }

        // GET: Patient/Delete/5
        public async Task<IActionResult> Delete(Guid id)
        {
            var patient = await _patientService.GetPatientByIdAsync(id);
            if (patient == null)
            {
                return NotFound();
            }

            return View(patient);
        }

        // POST: Patient/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            await _patientService.DeletePatientAsync(id);
            return RedirectToAction(nameof(Index));
        }

        // GET: Patient/Search
        public IActionResult Search()
        {
            return View();
        }

        // POST: Patient/Search
        [HttpPost]
        public async Task<IActionResult> Search(string searchTerm)
        {
            var patients = await _patientService.SearchPatientsAsync(searchTerm);
            return View("Index", patients);
        }

        // GET: Patient/MedicalRecords/5
        public async Task<IActionResult> MedicalRecords(Guid id)
        {
            var patient = await _patientService.GetPatientByIdAsync(id);
            if (patient == null)
            {
                return NotFound();
            }

            var medicalRecords = await _patientService.GetPatientMedicalRecordsAsync(id);
            ViewBag.Patient = patient;
            return View(medicalRecords);
        }

        // GET: Patient/Appointments/5
        public async Task<IActionResult> Appointments(Guid id)
        {
            var patient = await _patientService.GetPatientByIdAsync(id);
            if (patient == null)
            {
                return NotFound();
            }

            var appointments = await _patientService.GetPatientAppointmentsAsync(id);
            ViewBag.Patient = patient;
            return View(appointments);
        }

        // GET: Patient/Prescriptions/5
        public async Task<IActionResult> Prescriptions(Guid id)
        {
            var patient = await _patientService.GetPatientByIdAsync(id);
            if (patient == null)
            {
                return NotFound();
            }

            var prescriptions = await _patientService.GetPatientPrescriptionsAsync(id);
            ViewBag.Patient = patient;
            return View(prescriptions);
        }

        // GET: Patient/Invoices/5
        public async Task<IActionResult> Invoices(Guid id)
        {
            var patient = await _patientService.GetPatientByIdAsync(id);
            if (patient == null)
            {
                return NotFound();
            }

            var invoices = await _patientService.GetPatientInvoicesAsync(id);
            ViewBag.Patient = patient;
            return View(invoices);
        }
    }
}
