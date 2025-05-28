using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HospitalManagement.Domain.Entities;

namespace HospitalManagement.Business.Interfaces
{
    public interface IMedicalRecordService
    {
        Task<IEnumerable<MedicalRecord>> GetAllMedicalRecordsAsync();
        Task<MedicalRecord> GetMedicalRecordByIdAsync(Guid id);
        Task<MedicalRecord> GetMedicalRecordByAppointmentIdAsync(Guid appointmentId);
        Task<IEnumerable<MedicalRecord>> GetMedicalRecordsByPatientAsync(Guid patientId);
        Task<IEnumerable<MedicalRecord>> GetMedicalRecordsByDoctorAsync(Guid doctorId);
        Task<IEnumerable<MedicalRecord>> GetMedicalRecordsByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<bool> CreateMedicalRecordAsync(MedicalRecord medicalRecord);
        Task<bool> UpdateMedicalRecordAsync(MedicalRecord medicalRecord);
        Task<bool> DeleteMedicalRecordAsync(Guid id);
    }
}
