using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HospitalManagement.Business.Interfaces;
using HospitalManagement.DataAccess.Interfaces;
using HospitalManagement.Domain.Entities;

namespace HospitalManagement.Business.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IGenericRepository<Appointment> _appointmentRepository;
        private readonly IGenericRepository<Doctor> _doctorRepository;
        private readonly IGenericRepository<Patient> _patientRepository;
        private readonly IGenericRepository<DoctorSchedule> _doctorScheduleRepository;
        private readonly IGenericRepository<DoctorLeave> _doctorLeaveRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AppointmentService(
            IGenericRepository<Appointment> appointmentRepository,
            IGenericRepository<Doctor> doctorRepository,
            IGenericRepository<Patient> patientRepository,
            IGenericRepository<DoctorSchedule> doctorScheduleRepository,
            IGenericRepository<DoctorLeave> doctorLeaveRepository,
            IUnitOfWork unitOfWork)
        {
            _appointmentRepository = appointmentRepository;
            _doctorRepository = doctorRepository;
            _patientRepository = patientRepository;
            _doctorScheduleRepository = doctorScheduleRepository;
            _doctorLeaveRepository = doctorLeaveRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Appointment>> GetAllAppointmentsAsync()
        {
            return await _appointmentRepository.GetAllAsync();
        }

        public async Task<Appointment> GetAppointmentByIdAsync(Guid id)
        {
            return await _appointmentRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Appointment>> GetAppointmentsByPatientAsync(Guid patientId)
        {
            return await _appointmentRepository.FindAsync(a => a.PatientId == patientId);
        }

        public async Task<IEnumerable<Appointment>> GetAppointmentsByDoctorAsync(Guid doctorId)
        {
            return await _appointmentRepository.FindAsync(a => a.DoctorId == doctorId);
        }

        public async Task<IEnumerable<Appointment>> GetAppointmentsByDateAsync(DateTime date)
        {
            return await _appointmentRepository.FindAsync(a => a.AppointmentDate.Date == date.Date);
        }

        public async Task<IEnumerable<Appointment>> GetAppointmentsByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _appointmentRepository.FindAsync(a => 
                a.AppointmentDate.Date >= startDate.Date && 
                a.AppointmentDate.Date <= endDate.Date);
        }

        public async Task<IEnumerable<Appointment>> GetAppointmentsByDepartmentAsync(Guid departmentId)
        {
            return await _appointmentRepository.FindAsync(a => a.DepartmentId == departmentId);
        }

        public async Task<IEnumerable<Appointment>> GetAppointmentsByStatusAsync(string status)
        {
            return await _appointmentRepository.FindAsync(a => a.Status == status);
        }

        public async Task<bool> CreateAppointmentAsync(Appointment appointment)
        {
            // Check if patient exists
            if (!await _patientRepository.ExistsAsync(p => p.Id == appointment.PatientId && p.IsActive))
            {
                return false;
            }

            // Check if doctor exists and is available for appointments
            if (!await _doctorRepository.ExistsAsync(d => d.Id == appointment.DoctorId && d.IsAvailableForAppointment))
            {
                return false;
            }

            // Check if time slot is available
            if (!await IsTimeSlotAvailableAsync(appointment.DoctorId, appointment.AppointmentDate, appointment.StartTime, appointment.EndTime))
            {
                return false;
            }

            appointment.Status = "Scheduled";
            appointment.CreatedAt = DateTime.UtcNow;

            await _appointmentRepository.AddAsync(appointment);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateAppointmentAsync(Appointment appointment)
        {
            var existingAppointment = await _appointmentRepository.GetByIdAsync(appointment.Id);
            if (existingAppointment == null)
            {
                return false;
            }

            // If changing date/time, check availability
            if (existingAppointment.AppointmentDate != appointment.AppointmentDate || 
                existingAppointment.StartTime != appointment.StartTime || 
                existingAppointment.EndTime != appointment.EndTime)
            {
                // Check if time slot is available (excluding this appointment)
                var conflictingAppointments = await _appointmentRepository.FindAsync(a => 
                    a.Id != appointment.Id &&
                    a.DoctorId == appointment.DoctorId && 
                    a.AppointmentDate.Date == appointment.AppointmentDate.Date &&
                    a.Status != "Cancelled" &&
                    ((a.StartTime <= appointment.StartTime && a.EndTime > appointment.StartTime) ||
                     (a.StartTime < appointment.EndTime && a.EndTime >= appointment.EndTime) ||
                     (a.StartTime >= appointment.StartTime && a.EndTime <= appointment.EndTime)));

                if (conflictingAppointments.Any())
                {
                    return false;
                }
            }

            existingAppointment.PatientId = appointment.PatientId;
            existingAppointment.DoctorId = appointment.DoctorId;
            existingAppointment.DepartmentId = appointment.DepartmentId;
            existingAppointment.AppointmentDate = appointment.AppointmentDate;
            existingAppointment.StartTime = appointment.StartTime;
            existingAppointment.EndTime = appointment.EndTime;
            existingAppointment.Status = appointment.Status;
            existingAppointment.Type = appointment.Type;
            existingAppointment.Notes = appointment.Notes;
            existingAppointment.UpdatedAt = DateTime.UtcNow;

            _appointmentRepository.Update(existingAppointment);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> CancelAppointmentAsync(Guid id)
        {
            var appointment = await _appointmentRepository.GetByIdAsync(id);
            if (appointment == null || appointment.Status == "Completed" || appointment.Status == "Cancelled")
            {
                return false;
            }

            appointment.Status = "Cancelled";
            appointment.UpdatedAt = DateTime.UtcNow;

            _appointmentRepository.Update(appointment);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> CompleteAppointmentAsync(Guid id)
        {
            var appointment = await _appointmentRepository.GetByIdAsync(id);
            if (appointment == null || appointment.Status != "Scheduled")
            {
                return false;
            }

            appointment.Status = "Completed";
            appointment.UpdatedAt = DateTime.UtcNow;

            _appointmentRepository.Update(appointment);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> IsTimeSlotAvailableAsync(Guid doctorId, DateTime date, TimeSpan startTime, TimeSpan endTime)
        {
            // Check if doctor is available on this day of week
            var dayOfWeek = (int)date.DayOfWeek;
            var doctorSchedule = await _doctorScheduleRepository.GetSingleAsync(ds => 
                ds.DoctorId == doctorId && 
                ds.DayOfWeek == dayOfWeek && 
                ds.IsAvailable);

            if (doctorSchedule == null)
            {
                return false;
            }

            // Check if the requested time is within doctor's working hours
            if (startTime < doctorSchedule.StartTime || endTime > doctorSchedule.EndTime)
            {
                return false;
            }

            // Check if doctor is on leave
            var doctorOnLeave = await _doctorLeaveRepository.ExistsAsync(dl => 
                dl.DoctorId == doctorId && 
                dl.Status == "Approved" &&
                dl.StartDate.Date <= date.Date && 
                dl.EndDate.Date >= date.Date);

            if (doctorOnLeave)
            {
                return false;
            }

            // Check for conflicting appointments
            var conflictingAppointments = await _appointmentRepository.FindAsync(a => 
                a.DoctorId == doctorId && 
                a.AppointmentDate.Date == date.Date &&
                a.Status != "Cancelled" &&
                ((a.StartTime <= startTime && a.EndTime > startTime) ||
                 (a.StartTime < endTime && a.EndTime >= endTime) ||
                 (a.StartTime >= startTime && a.EndTime <= endTime)));

            if (conflictingAppointments.Any())
            {
                return false;
            }

            // Check if maximum appointments for the day has been reached
            var appointmentsCount = await _appointmentRepository.CountAsync(a => 
                a.DoctorId == doctorId && 
                a.AppointmentDate.Date == date.Date && 
                a.Status != "Cancelled");

            if (appointmentsCount >= doctorSchedule.MaxAppointments)
            {
                return false;
            }

            return true;
        }

        public async Task<IEnumerable<TimeSpan>> GetAvailableTimeSlotsAsync(Guid doctorId, DateTime date)
        {
            var availableSlots = new List<TimeSpan>();
            
            // Check if doctor is available on this day of week
            var dayOfWeek = (int)date.DayOfWeek;
            var doctorSchedule = await _doctorScheduleRepository.GetSingleAsync(ds => 
                ds.DoctorId == doctorId && 
                ds.DayOfWeek == dayOfWeek && 
                ds.IsAvailable);

            if (doctorSchedule == null)
            {
                return availableSlots;
            }

            // Check if doctor is on leave
            var doctorOnLeave = await _doctorLeaveRepository.ExistsAsync(dl => 
                dl.DoctorId == doctorId && 
                dl.Status == "Approved" &&
                dl.StartDate.Date <= date.Date && 
                dl.EndDate.Date >= date.Date);

            if (doctorOnLeave)
            {
                return availableSlots;
            }

            // Check if maximum appointments for the day has been reached
            var appointmentsCount = await _appointmentRepository.CountAsync(a => 
                a.DoctorId == doctorId && 
                a.AppointmentDate.Date == date.Date && 
                a.Status != "Cancelled");

            if (appointmentsCount >= doctorSchedule.MaxAppointments)
            {
                return availableSlots;
            }

            // Get existing appointments for the day
            var existingAppointments = await _appointmentRepository.FindAsync(a => 
                a.DoctorId == doctorId && 
                a.AppointmentDate.Date == date.Date &&
                a.Status != "Cancelled");

            // Assuming 30-minute slots
            var slotDuration = TimeSpan.FromMinutes(30);
            var currentSlot = doctorSchedule.StartTime;

            while (currentSlot.Add(slotDuration) <= doctorSchedule.EndTime)
            {
                var slotEnd = currentSlot.Add(slotDuration);
                
                // Check if this slot conflicts with any existing appointment
                var isConflicting = existingAppointments.Any(a => 
                    (a.StartTime <= currentSlot && a.EndTime > currentSlot) ||
                    (a.StartTime < slotEnd && a.EndTime >= slotEnd) ||
                    (a.StartTime >= currentSlot && a.EndTime <= slotEnd));

                if (!isConflicting)
                {
                    availableSlots.Add(currentSlot);
                }

                currentSlot = slotEnd;
            }

            return availableSlots;
        }
    }
}
