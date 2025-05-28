using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HospitalManagement.Domain.Entities;

namespace HospitalManagement.Business.Interfaces
{
    public interface IPrescriptionService
    {
        Task<IEnumerable<Prescription>> GetAllPrescriptionsAsync();
        Task<Prescription> GetPrescriptionByIdAsync(Guid id);
        Task<IEnumerable<Prescription>> GetPrescriptionsByPatientAsync(Guid patientId);
        Task<IEnumerable<Prescription>> GetPrescriptionsByDoctorAsync(Guid doctorId);
        Task<IEnumerable<Prescription>> GetPrescriptionsByMedicalRecordAsync(Guid medicalRecordId);
        Task<IEnumerable<Prescription>> GetPrescriptionsByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<IEnumerable<Prescription>> GetPrescriptionsByStatusAsync(string status);
        Task<bool> CreatePrescriptionAsync(Prescription prescription);
        Task<bool> UpdatePrescriptionAsync(Prescription prescription);
        Task<bool> DeletePrescriptionAsync(Guid id);
        Task<bool> AddPrescriptionItemAsync(PrescriptionItem item);
        Task<bool> UpdatePrescriptionItemAsync(PrescriptionItem item);
        Task<bool> DeletePrescriptionItemAsync(Guid itemId);
        Task<bool> MarkPrescriptionAsFilledAsync(Guid prescriptionId);
        Task<bool> MarkPrescriptionAsExpiredAsync(Guid prescriptionId);
    }
}
