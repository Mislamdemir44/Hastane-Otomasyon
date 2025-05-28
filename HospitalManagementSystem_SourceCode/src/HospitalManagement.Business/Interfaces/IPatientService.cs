using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HospitalManagement.Domain.Entities;

namespace HospitalManagement.Business.Interfaces
{
    public interface IPatientService
    {
        Task<IEnumerable<Patient>> GetAllPatientsAsync();
        Task<Patient> GetPatientByIdAsync(Guid id);
        Task<Patient> GetPatientByIdentityNumberAsync(string identityNumber);
        Task<IEnumerable<Patient>> SearchPatientsAsync(string searchTerm);
        Task<bool> CreatePatientAsync(Patient patient);
        Task<bool> UpdatePatientAsync(Patient patient);
        Task<bool> DeletePatientAsync(Guid id);
        Task<IEnumerable<MedicalRecord>> GetPatientMedicalRecordsAsync(Guid patientId);
        Task<IEnumerable<Appointment>> GetPatientAppointmentsAsync(Guid patientId);
        Task<IEnumerable<Prescription>> GetPatientPrescriptionsAsync(Guid patientId);
        Task<IEnumerable<Invoice>> GetPatientInvoicesAsync(Guid patientId);
    }
}
