using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using HospitalManagement.Business.Interfaces;
using HospitalManagement.Domain.Entities;
using HospitalManagement.Web.ViewModels;

namespace HospitalManagement.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IAppointmentService _appointmentService;
        private readonly IDoctorService _doctorService;
        private readonly IPatientService _patientService;

        public HomeController(
            ILogger<HomeController> logger,
            IAppointmentService appointmentService,
            IDoctorService doctorService,
            IPatientService patientService)
        {
            _logger = logger;
            _appointmentService = appointmentService;
            _doctorService = doctorService;
            _patientService = patientService;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                // Get today's appointments
                var today = DateTime.Today;
                var appointments = await _appointmentService.GetAppointmentsByDateAsync(today);
                
                // Get counts for dashboard
                ViewBag.TodayAppointmentsCount = appointments.Count();
                ViewBag.DoctorsCount = (await _doctorService.GetAllDoctorsAsync()).Count();
                ViewBag.PatientsCount = (await _patientService.GetAllPatientsAsync()).Count();
                
                return View(appointments);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while loading Home/Index");
                return View(new List<Appointment>());
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View();
        }
    }
}
