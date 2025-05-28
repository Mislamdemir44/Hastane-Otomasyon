using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HospitalManagement.Domain.Entities;

namespace HospitalManagement.Business.Interfaces
{
    public interface IAppointmentService
    {
        Task<IEnumerable<Appointment>> GetAllAppointmentsAsync();
        Task<Appointment> GetAppointmentByIdAsync(Guid id);
        Task<IEnumerable<Appointment>> GetAppointmentsByPatientAsync(Guid patientId);
        Task<IEnumerable<Appointment>> GetAppointmentsByDoctorAsync(Guid doctorId);
        Task<IEnumerable<Appointment>> GetAppointmentsByDateAsync(DateTime date);
        Task<IEnumerable<Appointment>> GetAppointmentsByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<IEnumerable<Appointment>> GetAppointmentsByDepartmentAsync(Guid departmentId);
        Task<IEnumerable<Appointment>> GetAppointmentsByStatusAsync(string status);
        Task<bool> CreateAppointmentAsync(Appointment appointment);
        Task<bool> UpdateAppointmentAsync(Appointment appointment);
        Task<bool> CancelAppointmentAsync(Guid id);
        Task<bool> CompleteAppointmentAsync(Guid id);
        Task<bool> IsTimeSlotAvailableAsync(Guid doctorId, DateTime date, TimeSpan startTime, TimeSpan endTime);
        Task<IEnumerable<TimeSpan>> GetAvailableTimeSlotsAsync(Guid doctorId, DateTime date);
    }
}
