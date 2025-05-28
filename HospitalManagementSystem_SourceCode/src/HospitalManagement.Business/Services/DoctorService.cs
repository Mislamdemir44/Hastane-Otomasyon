using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HospitalManagement.Business.Interfaces;
using HospitalManagement.DataAccess.Interfaces;
using HospitalManagement.Domain.Entities;

namespace HospitalManagement.Business.Services
{
    public class DoctorService : IDoctorService
    {
        private readonly IGenericRepository<Doctor> _doctorRepository;
        private readonly IGenericRepository<DoctorSchedule> _doctorScheduleRepository;
        private readonly IGenericRepository<DoctorLeave> _doctorLeaveRepository;
        private readonly IGenericRepository<Appointment> _appointmentRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DoctorService(
            IGenericRepository<Doctor> doctorRepository,
            IGenericRepository<DoctorSchedule> doctorScheduleRepository,
            IGenericRepository<DoctorLeave> doctorLeaveRepository,
            IGenericRepository<Appointment> appointmentRepository,
            IUnitOfWork unitOfWork)
        {
            _doctorRepository = doctorRepository;
            _doctorScheduleRepository = doctorScheduleRepository;
            _doctorLeaveRepository = doctorLeaveRepository;
            _appointmentRepository = appointmentRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Doctor>> GetAllDoctorsAsync()
        {
            return await _doctorRepository.GetAllAsync();
        }

        public async Task<Doctor> GetDoctorByIdAsync(Guid id)
        {
            return await _doctorRepository.GetByIdAsync(id);
        }

        public async Task<Doctor> GetDoctorByUserIdAsync(Guid userId)
        {
            return await _doctorRepository.GetSingleAsync(d => d.UserId == userId);
        }

        public async Task<IEnumerable<Doctor>> GetDoctorsByDepartmentAsync(Guid departmentId)
        {
            return await _doctorRepository.FindAsync(d => d.DepartmentId == departmentId);
        }

        public async Task<IEnumerable<Doctor>> GetDoctorsBySpecialtyAsync(Guid specialtyId)
        {
            return await _doctorRepository.FindAsync(d => d.SpecialtyId == specialtyId);
        }

        public async Task<IEnumerable<Doctor>> SearchDoctorsAsync(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return new List<Doctor>();
            }

            searchTerm = searchTerm.ToLower();
            
            // This is a simplified search. In a real application, you would use joins or navigation properties
            // to search through related User entity for name, etc.
            return await _doctorRepository.FindAsync(d =>
                d.LicenseNumber.ToLower().Contains(searchTerm) ||
                d.Biography.ToLower().Contains(searchTerm));
        }

        public async Task<bool> CreateDoctorAsync(Doctor doctor)
        {
            // Check if license number already exists
            if (await _doctorRepository.ExistsAsync(d => d.LicenseNumber == doctor.LicenseNumber))
            {
                return false;
            }

            doctor.CreatedAt = DateTime.UtcNow;
            doctor.IsAvailableForAppointment = true;

            await _doctorRepository.AddAsync(doctor);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateDoctorAsync(Doctor doctor)
        {
            var existingDoctor = await _doctorRepository.GetByIdAsync(doctor.Id);
            if (existingDoctor == null)
            {
                return false;
            }

            // Check if license number is being changed and if it conflicts with existing doctors
            if (doctor.LicenseNumber != existingDoctor.LicenseNumber && 
                await _doctorRepository.ExistsAsync(d => d.LicenseNumber == doctor.LicenseNumber))
            {
                return false;
            }

            existingDoctor.DepartmentId = doctor.DepartmentId;
            existingDoctor.SpecialtyId = doctor.SpecialtyId;
            existingDoctor.LicenseNumber = doctor.LicenseNumber;
            existingDoctor.Education = doctor.Education;
            existingDoctor.Biography = doctor.Biography;
            existingDoctor.ConsultationFee = doctor.ConsultationFee;
            existingDoctor.IsAvailableForAppointment = doctor.IsAvailableForAppointment;
            existingDoctor.UpdatedAt = DateTime.UtcNow;

            _doctorRepository.Update(existingDoctor);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteDoctorAsync(Guid id)
        {
            var doctor = await _doctorRepository.GetByIdAsync(id);
            if (doctor == null)
            {
                return false;
            }

            // Soft delete by making doctor unavailable for appointments
            doctor.IsAvailableForAppointment = false;
            doctor.UpdatedAt = DateTime.UtcNow;

            _doctorRepository.Update(doctor);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<DoctorSchedule>> GetDoctorSchedulesAsync(Guid doctorId)
        {
            return await _doctorScheduleRepository.FindAsync(ds => ds.DoctorId == doctorId);
        }

        public async Task<IEnumerable<DoctorLeave>> GetDoctorLeavesAsync(Guid doctorId)
        {
            return await _doctorLeaveRepository.FindAsync(dl => dl.DoctorId == doctorId);
        }

        public async Task<IEnumerable<Appointment>> GetDoctorAppointmentsAsync(Guid doctorId, DateTime? date = null)
        {
            if (date.HasValue)
            {
                return await _appointmentRepository.FindAsync(a => 
                    a.DoctorId == doctorId && 
                    a.AppointmentDate.Date == date.Value.Date);
            }
            
            return await _appointmentRepository.FindAsync(a => a.DoctorId == doctorId);
        }

        public async Task<bool> AddDoctorScheduleAsync(DoctorSchedule schedule)
        {
            // Check if doctor exists
            if (!await _doctorRepository.ExistsAsync(d => d.Id == schedule.DoctorId))
            {
                return false;
            }

            // Check if schedule for this day already exists
            var existingSchedule = await _doctorScheduleRepository.GetSingleAsync(ds => 
                ds.DoctorId == schedule.DoctorId && 
                ds.DayOfWeek == schedule.DayOfWeek);

            if (existingSchedule != null)
            {
                // Update existing schedule
                existingSchedule.StartTime = schedule.StartTime;
                existingSchedule.EndTime = schedule.EndTime;
                existingSchedule.IsAvailable = schedule.IsAvailable;
                existingSchedule.MaxAppointments = schedule.MaxAppointments;
                existingSchedule.UpdatedAt = DateTime.UtcNow;

                _doctorScheduleRepository.Update(existingSchedule);
            }
            else
            {
                // Create new schedule
                schedule.CreatedAt = DateTime.UtcNow;
                await _doctorScheduleRepository.AddAsync(schedule);
            }

            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateDoctorScheduleAsync(DoctorSchedule schedule)
        {
            var existingSchedule = await _doctorScheduleRepository.GetByIdAsync(schedule.Id);
            if (existingSchedule == null)
            {
                return false;
            }

            existingSchedule.StartTime = schedule.StartTime;
            existingSchedule.EndTime = schedule.EndTime;
            existingSchedule.IsAvailable = schedule.IsAvailable;
            existingSchedule.MaxAppointments = schedule.MaxAppointments;
            existingSchedule.UpdatedAt = DateTime.UtcNow;

            _doctorScheduleRepository.Update(existingSchedule);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteDoctorScheduleAsync(Guid scheduleId)
        {
            var schedule = await _doctorScheduleRepository.GetByIdAsync(scheduleId);
            if (schedule == null)
            {
                return false;
            }

            _doctorScheduleRepository.Delete(schedule);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RequestLeaveAsync(DoctorLeave leave)
        {
            // Check if doctor exists
            if (!await _doctorRepository.ExistsAsync(d => d.Id == leave.DoctorId))
            {
                return false;
            }

            leave.Status = "Pending";
            leave.CreatedAt = DateTime.UtcNow;

            await _doctorLeaveRepository.AddAsync(leave);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ApproveLeaveAsync(Guid leaveId, Guid approvedById)
        {
            var leave = await _doctorLeaveRepository.GetByIdAsync(leaveId);
            if (leave == null || leave.Status != "Pending")
            {
                return false;
            }

            leave.Status = "Approved";
            leave.ApprovedById = approvedById;
            leave.UpdatedAt = DateTime.UtcNow;

            _doctorLeaveRepository.Update(leave);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RejectLeaveAsync(Guid leaveId, Guid rejectedById)
        {
            var leave = await _doctorLeaveRepository.GetByIdAsync(leaveId);
            if (leave == null || leave.Status != "Pending")
            {
                return false;
            }

            leave.Status = "Rejected";
            leave.ApprovedById = rejectedById; // Using the same field for rejection
            leave.UpdatedAt = DateTime.UtcNow;

            _doctorLeaveRepository.Update(leave);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}
