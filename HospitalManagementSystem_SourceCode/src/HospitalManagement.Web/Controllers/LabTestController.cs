using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HospitalManagement.Business.Interfaces;
using HospitalManagement.Domain.Entities;
using HospitalManagement.Web.ViewModels;

namespace HospitalManagement.Web.Controllers
{
    public class LabTestController : Controller
    {
        private readonly ILabTestService _labTestService;
        private readonly IPatientService _patientService;
        private readonly IDoctorService _doctorService;
        private readonly IMedicalRecordService _medicalRecordService;

        public LabTestController(
            ILabTestService labTestService,
            IPatientService patientService,
            IDoctorService doctorService,
            IMedicalRecordService medicalRecordService)
        {
            _labTestService = labTestService;
            _patientService = patientService;
            _doctorService = doctorService;
            _medicalRecordService = medicalRecordService;
        }

        // GET: LabTest
        public async Task<IActionResult> Index()
        {
            var labTests = await _labTestService.GetAllLabTestsAsync();
            return View(labTests);
        }

        // GET: LabTest/Details/5
        public async Task<IActionResult> Details(Guid id)
        {
            var labTest = await _labTestService.GetLabTestByIdAsync(id);
            if (labTest == null)
            {
                return NotFound();
            }

            return View(labTest);
        }

        // GET: LabTest/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: LabTest/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(LabTestViewModel labTestViewModel)
        {
            if (ModelState.IsValid)
            {
                var labTest = new LabTest
                {
                    Name = labTestViewModel.Name,
                    Description = labTestViewModel.Description,
                    Department = labTestViewModel.Department,
                    Price = labTestViewModel.Price,
                    IsActive = true
                };

                var result = await _labTestService.CreateLabTestAsync(labTest);
                if (result)
                {
                    return RedirectToAction(nameof(Index));
                }
                
                ModelState.AddModelError("", "Failed to create lab test.");
            }
            
            return View(labTestViewModel);
        }

        // GET: LabTest/Edit/5
        public async Task<IActionResult> Edit(Guid id)
        {
            var labTest = await _labTestService.GetLabTestByIdAsync(id);
            if (labTest == null)
            {
                return NotFound();
            }

            var labTestViewModel = new LabTestViewModel
            {
                Id = labTest.Id,
                Name = labTest.Name,
                Description = labTest.Description,
                Department = labTest.Department,
                Price = labTest.Price,
                IsActive = labTest.IsActive
            };

            return View(labTestViewModel);
        }

        // POST: LabTest/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, LabTestViewModel labTestViewModel)
        {
            if (id != labTestViewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var labTest = new LabTest
                {
                    Id = labTestViewModel.Id,
                    Name = labTestViewModel.Name,
                    Description = labTestViewModel.Description,
                    Department = labTestViewModel.Department,
                    Price = labTestViewModel.Price,
                    IsActive = labTestViewModel.IsActive
                };

                var result = await _labTestService.UpdateLabTestAsync(labTest);
                if (result)
                {
                    return RedirectToAction(nameof(Index));
                }
                
                ModelState.AddModelError("", "Failed to update lab test.");
            }
            
            return View(labTestViewModel);
        }

        // GET: LabTest/Delete/5
        public async Task<IActionResult> Delete(Guid id)
        {
            var labTest = await _labTestService.GetLabTestByIdAsync(id);
            if (labTest == null)
            {
                return NotFound();
            }

            return View(labTest);
        }

        // POST: LabTest/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            await _labTestService.DeleteLabTestAsync(id);
            return RedirectToAction(nameof(Index));
        }

        // GET: LabTest/Requests
        public async Task<IActionResult> Requests()
        {
            var labTestRequests = await _labTestService.GetAllLabTestRequestsAsync();
            return View(labTestRequests);
        }

        // GET: LabTest/RequestDetails/5
        public async Task<IActionResult> RequestDetails(Guid id)
        {
            var labTestRequest = await _labTestService.GetLabTestRequestByIdAsync(id);
            if (labTestRequest == null)
            {
                return NotFound();
            }

            return View(labTestRequest);
        }

        // GET: LabTest/CreateRequest
        public async Task<IActionResult> CreateRequest(Guid medicalRecordId)
        {
            var medicalRecord = await _medicalRecordService.GetMedicalRecordByIdAsync(medicalRecordId);
            if (medicalRecord == null)
            {
                return NotFound();
            }

            var patient = await _patientService.GetPatientByIdAsync(medicalRecord.PatientId);
            var doctor = await _doctorService.GetDoctorByIdAsync(medicalRecord.DoctorId);

            var requestViewModel = new LabTestRequestViewModel
            {
                PatientId = medicalRecord.PatientId,
                DoctorId = medicalRecord.DoctorId,
                MedicalRecordId = medicalRecordId,
                RequestDate = DateTime.UtcNow,
                Priority = "Routine"
            };

            ViewBag.Patient = patient;
            ViewBag.Doctor = doctor;
            ViewBag.MedicalRecord = medicalRecord;

            return View(requestViewModel);
        }

        // POST: LabTest/CreateRequest
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateRequest(LabTestRequestViewModel requestViewModel)
        {
            if (ModelState.IsValid)
            {
                var request = new LabTestRequest
                {
                    PatientId = requestViewModel.PatientId,
                    DoctorId = requestViewModel.DoctorId,
                    MedicalRecordId = requestViewModel.MedicalRecordId,
                    RequestDate = requestViewModel.RequestDate,
                    Status = "Requested",
                    Priority = requestViewModel.Priority,
                    Notes = requestViewModel.Notes
                };

                var result = await _labTestService.CreateLabTestRequestAsync(request);
                if (result)
                {
                    return RedirectToAction(nameof(AddRequestItems), new { id = request.Id });
                }
                
                ModelState.AddModelError("", "Failed to create lab test request.");
            }
            
            var patient = await _patientService.GetPatientByIdAsync(requestViewModel.PatientId);
            var doctor = await _doctorService.GetDoctorByIdAsync(requestViewModel.DoctorId);
            var medicalRecord = await _medicalRecordService.GetMedicalRecordByIdAsync(requestViewModel.MedicalRecordId);

            ViewBag.Patient = patient;
            ViewBag.Doctor = doctor;
            ViewBag.MedicalRecord = medicalRecord;
            
            return View(requestViewModel);
        }

        // GET: LabTest/AddRequestItems/5
        public async Task<IActionResult> AddRequestItems(Guid id)
        {
            var request = await _labTestService.GetLabTestRequestByIdAsync(id);
            if (request == null)
            {
                return NotFound();
            }

            var labTests = await _labTestService.GetAllLabTestsAsync();
            ViewBag.Request = request;
            ViewBag.LabTests = labTests;
            
            return View();
        }

        // POST: LabTest/AddRequestItem
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddRequestItem(LabTestRequestItemViewModel itemViewModel)
        {
            if (ModelState.IsValid)
            {
                var item = new LabTestRequestItem
                {
                    LabTestRequestId = itemViewModel.LabTestRequestId,
                    LabTestId = itemViewModel.LabTestId,
                    Status = "Pending"
                };

                var result = await _labTestService.AddLabTestRequestItemAsync(item);
                if (result)
                {
                    return RedirectToAction(nameof(RequestDetails), new { id = itemViewModel.LabTestRequestId });
                }
                
                ModelState.AddModelError("", "Failed to add lab test request item.");
            }
            
            var request = await _labTestService.GetLabTestRequestByIdAsync(itemViewModel.LabTestRequestId);
            var labTests = await _labTestService.GetAllLabTestsAsync();
            ViewBag.Request = request;
            ViewBag.LabTests = labTests;
            
            return View("AddRequestItems", itemViewModel);
        }

        // GET: LabTest/EnterResults/5
        public async Task<IActionResult> EnterResults(Guid id)
        {
            var item = await _labTestService.GetLabTestRequestItemByIdAsync(id);
            if (item == null)
            {
                return NotFound();
            }

            var resultViewModel = new LabTestResultViewModel
            {
                ItemId = id,
                ResultValue = item.ResultValue,
                ReferenceRange = item.ReferenceRange,
                Remarks = item.Remarks
            };

            return View(resultViewModel);
        }

        // POST: LabTest/EnterResults
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EnterResults(LabTestResultViewModel resultViewModel)
        {
            if (ModelState.IsValid)
            {
                var result = await _labTestService.UpdateLabTestResultAsync(
                    resultViewModel.ItemId,
                    resultViewModel.ResultValue,
                    resultViewModel.Remarks,
                    resultViewModel.TechnicianId);
                
                if (result)
                {
                    return RedirectToAction(nameof(RequestDetails), new { id = resultViewModel.RequestId });
                }
                
                ModelState.AddModelError("", "Failed to update lab test result.");
            }
            
            return View(resultViewModel);
        }

        // POST: LabTest/VerifyResult/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> VerifyResult(Guid id, Guid verifiedById)
        {
            await _labTestService.VerifyLabTestResultAsync(id, verifiedById);
            return RedirectToAction(nameof(RequestDetails), new { id });
        }

        // GET: LabTest/RequestsByPatient/5
        public async Task<IActionResult> RequestsByPatient(Guid id)
        {
            var patient = await _patientService.GetPatientByIdAsync(id);
            if (patient == null)
            {
                return NotFound();
            }

            var requests = await _labTestService.GetLabTestRequestsByPatientAsync(id);
            ViewBag.Patient = patient;
            return View(requests);
        }

        // GET: LabTest/RequestsByDoctor/5
        public async Task<IActionResult> RequestsByDoctor(Guid id)
        {
            var doctor = await _doctorService.GetDoctorByIdAsync(id);
            if (doctor == null)
            {
                return NotFound();
            }

            var requests = await _labTestService.GetLabTestRequestsByDoctorAsync(id);
            ViewBag.Doctor = doctor;
            return View(requests);
        }

        // GET: LabTest/RequestsByStatus
        public async Task<IActionResult> RequestsByStatus(string status)
        {
            var requests = await _labTestService.GetLabTestRequestsByStatusAsync(status);
            ViewBag.Status = status;
            return View(requests);
        }
    }
}
