using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HospitalManagement.Domain.Entities;

namespace HospitalManagement.Business.Interfaces
{
    public interface IDoctorService
    {
        Task<IEnumerable<Doctor>> GetAllDoctorsAsync();
        Task<Doctor> GetDoctorByIdAsync(Guid id);
        Task<Doctor> GetDoctorByUserIdAsync(Guid userId);
        Task<IEnumerable<Doctor>> GetDoctorsByDepartmentAsync(Guid departmentId);
        Task<IEnumerable<Doctor>> GetDoctorsBySpecialtyAsync(Guid specialtyId);
        Task<IEnumerable<Doctor>> SearchDoctorsAsync(string searchTerm);
        Task<bool> CreateDoctorAsync(Doctor doctor);
        Task<bool> UpdateDoctorAsync(Doctor doctor);
        Task<bool> DeleteDoctorAsync(Guid id);
        Task<IEnumerable<DoctorSchedule>> GetDoctorSchedulesAsync(Guid doctorId);
        Task<IEnumerable<DoctorLeave>> GetDoctorLeavesAsync(Guid doctorId);
        Task<IEnumerable<Appointment>> GetDoctorAppointmentsAsync(Guid doctorId, DateTime? date = null);
        Task<bool> AddDoctorScheduleAsync(DoctorSchedule schedule);
        Task<bool> UpdateDoctorScheduleAsync(DoctorSchedule schedule);
        Task<bool> DeleteDoctorScheduleAsync(Guid scheduleId);
        Task<bool> RequestLeaveAsync(DoctorLeave leave);
        Task<bool> ApproveLeaveAsync(Guid leaveId, Guid approvedById);
        Task<bool> RejectLeaveAsync(Guid leaveId, Guid rejectedById);
    }
}
