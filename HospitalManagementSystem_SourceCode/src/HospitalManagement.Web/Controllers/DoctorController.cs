using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HospitalManagement.Business.Interfaces;
using HospitalManagement.Domain.Entities;
using HospitalManagement.Web.ViewModels;

namespace HospitalManagement.Web.Controllers
{
    public class DoctorController : Controller
    {
        private readonly IDoctorService _doctorService;
        private readonly IUserService _userService;

        public DoctorController(IDoctorService doctorService, IUserService userService)
        {
            _doctorService = doctorService;
            _userService = userService;
        }

        // GET: Doctor
        public async Task<IActionResult> Index()
        {
            var doctors = await _doctorService.GetAllDoctorsAsync();
            return View(doctors);
        }

        // GET: Doctor/Details/5
        public async Task<IActionResult> Details(Guid id)
        {
            var doctor = await _doctorService.GetDoctorByIdAsync(id);
            if (doctor == null)
            {
                return NotFound();
            }

            return View(doctor);
        }

        // GET: Doctor/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Doctor/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DoctorViewModel doctorViewModel)
        {
            if (ModelState.IsValid)
            {
                var doctor = new Doctor
                {
                    UserId = doctorViewModel.UserId,
                    DepartmentId = doctorViewModel.DepartmentId,
                    SpecialtyId = doctorViewModel.SpecialtyId,
                    LicenseNumber = doctorViewModel.LicenseNumber,
                    Education = doctorViewModel.Education,
                    Biography = doctorViewModel.Biography,
                    ConsultationFee = doctorViewModel.ConsultationFee,
                    IsAvailableForAppointment = true
                };

                var result = await _doctorService.CreateDoctorAsync(doctor);
                if (result)
                {
                    return RedirectToAction(nameof(Index));
                }
                
                ModelState.AddModelError("", "License Number already exists.");
            }
            
            return View(doctorViewModel);
        }

        // GET: Doctor/Edit/5
        public async Task<IActionResult> Edit(Guid id)
        {
            var doctor = await _doctorService.GetDoctorByIdAsync(id);
            if (doctor == null)
            {
                return NotFound();
            }

            var doctorViewModel = new DoctorViewModel
            {
                Id = doctor.Id,
                UserId = doctor.UserId,
                DepartmentId = doctor.DepartmentId,
                SpecialtyId = doctor.SpecialtyId,
                LicenseNumber = doctor.LicenseNumber,
                Education = doctor.Education,
                Biography = doctor.Biography,
                ConsultationFee = doctor.ConsultationFee,
                IsAvailableForAppointment = doctor.IsAvailableForAppointment
            };

            return View(doctorViewModel);
        }

        // POST: Doctor/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, DoctorViewModel doctorViewModel)
        {
            if (id != doctorViewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var doctor = new Doctor
                {
                    Id = doctorViewModel.Id,
                    UserId = doctorViewModel.UserId,
                    DepartmentId = doctorViewModel.DepartmentId,
                    SpecialtyId = doctorViewModel.SpecialtyId,
                    LicenseNumber = doctorViewModel.LicenseNumber,
                    Education = doctorViewModel.Education,
                    Biography = doctorViewModel.Biography,
                    ConsultationFee = doctorViewModel.ConsultationFee,
                    IsAvailableForAppointment = doctorViewModel.IsAvailableForAppointment
                };

                var result = await _doctorService.UpdateDoctorAsync(doctor);
                if (result)
                {
                    return RedirectToAction(nameof(Index));
                }
                
                ModelState.AddModelError("", "License Number already exists.");
            }
            
            return View(doctorViewModel);
        }

        // GET: Doctor/Delete/5
        public async Task<IActionResult> Delete(Guid id)
        {
            var doctor = await _doctorService.GetDoctorByIdAsync(id);
            if (doctor == null)
            {
                return NotFound();
            }

            return View(doctor);
        }

        // POST: Doctor/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            await _doctorService.DeleteDoctorAsync(id);
            return RedirectToAction(nameof(Index));
        }

        // GET: Doctor/Schedule/5
        public async Task<IActionResult> Schedule(Guid id)
        {
            var doctor = await _doctorService.GetDoctorByIdAsync(id);
            if (doctor == null)
            {
                return NotFound();
            }

            var schedules = await _doctorService.GetDoctorSchedulesAsync(id);
            ViewBag.Doctor = doctor;
            return View(schedules);
        }

        // GET: Doctor/AddSchedule/5
        public async Task<IActionResult> AddSchedule(Guid id)
        {
            var doctor = await _doctorService.GetDoctorByIdAsync(id);
            if (doctor == null)
            {
                return NotFound();
            }

            var scheduleViewModel = new DoctorScheduleViewModel
            {
                DoctorId = id
            };

            ViewBag.Doctor = doctor;
            return View(scheduleViewModel);
        }

        // POST: Doctor/AddSchedule
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddSchedule(DoctorScheduleViewModel scheduleViewModel)
        {
            if (ModelState.IsValid)
            {
                var schedule = new DoctorSchedule
                {
                    DoctorId = scheduleViewModel.DoctorId,
                    DayOfWeek = scheduleViewModel.DayOfWeek,
                    StartTime = scheduleViewModel.StartTime,
                    EndTime = scheduleViewModel.EndTime,
                    IsAvailable = scheduleViewModel.IsAvailable,
                    MaxAppointments = scheduleViewModel.MaxAppointments
                };

                var result = await _doctorService.AddDoctorScheduleAsync(schedule);
                if (result)
                {
                    return RedirectToAction(nameof(Schedule), new { id = scheduleViewModel.DoctorId });
                }
                
                ModelState.AddModelError("", "Failed to add schedule.");
            }
            
            var doctor = await _doctorService.GetDoctorByIdAsync(scheduleViewModel.DoctorId);
            ViewBag.Doctor = doctor;
            return View(scheduleViewModel);
        }

        // GET: Doctor/Leaves/5
        public async Task<IActionResult> Leaves(Guid id)
        {
            var doctor = await _doctorService.GetDoctorByIdAsync(id);
            if (doctor == null)
            {
                return NotFound();
            }

            var leaves = await _doctorService.GetDoctorLeavesAsync(id);
            ViewBag.Doctor = doctor;
            return View(leaves);
        }

        // GET: Doctor/RequestLeave/5
        public async Task<IActionResult> RequestLeave(Guid id)
        {
            var doctor = await _doctorService.GetDoctorByIdAsync(id);
            if (doctor == null)
            {
                return NotFound();
            }

            var leaveViewModel = new DoctorLeaveViewModel
            {
                DoctorId = id
            };

            ViewBag.Doctor = doctor;
            return View(leaveViewModel);
        }

        // POST: Doctor/RequestLeave
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RequestLeave(DoctorLeaveViewModel leaveViewModel)
        {
            if (ModelState.IsValid)
            {
                var leave = new DoctorLeave
                {
                    DoctorId = leaveViewModel.DoctorId,
                    StartDate = leaveViewModel.StartDate,
                    EndDate = leaveViewModel.EndDate,
                    Reason = leaveViewModel.Reason
                };

                var result = await _doctorService.RequestLeaveAsync(leave);
                if (result)
                {
                    return RedirectToAction(nameof(Leaves), new { id = leaveViewModel.DoctorId });
                }
                
                ModelState.AddModelError("", "Failed to request leave.");
            }
            
            var doctor = await _doctorService.GetDoctorByIdAsync(leaveViewModel.DoctorId);
            ViewBag.Doctor = doctor;
            return View(leaveViewModel);
        }

        // GET: Doctor/Appointments/5
        public async Task<IActionResult> Appointments(Guid id, DateTime? date = null)
        {
            var doctor = await _doctorService.GetDoctorByIdAsync(id);
            if (doctor == null)
            {
                return NotFound();
            }

            var appointments = await _doctorService.GetDoctorAppointmentsAsync(id, date);
            ViewBag.Doctor = doctor;
            ViewBag.Date = date ?? DateTime.Today;
            return View(appointments);
        }
    }
}
