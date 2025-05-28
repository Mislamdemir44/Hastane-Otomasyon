using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HospitalManagement.Business.Interfaces;
using HospitalManagement.DataAccess.Interfaces;
using HospitalManagement.Domain.Entities;

namespace HospitalManagement.Business.Services
{
    public class PatientService : IPatientService
    {
        private readonly IGenericRepository<Patient> _patientRepository;
        private readonly IGenericRepository<MedicalRecord> _medicalRecordRepository;
        private readonly IGenericRepository<Appointment> _appointmentRepository;
        private readonly IGenericRepository<Prescription> _prescriptionRepository;
        private readonly IGenericRepository<Invoice> _invoiceRepository;
        private readonly IUnitOfWork _unitOfWork;

        public PatientService(
            IGenericRepository<Patient> patientRepository,
            IGenericRepository<MedicalRecord> medicalRecordRepository,
            IGenericRepository<Appointment> appointmentRepository,
            IGenericRepository<Prescription> prescriptionRepository,
            IGenericRepository<Invoice> invoiceRepository,
            IUnitOfWork unitOfWork)
        {
            _patientRepository = patientRepository;
            _medicalRecordRepository = medicalRecordRepository;
            _appointmentRepository = appointmentRepository;
            _prescriptionRepository = prescriptionRepository;
            _invoiceRepository = invoiceRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Patient>> GetAllPatientsAsync()
        {
            return await _patientRepository.GetAllAsync();
        }

        public async Task<Patient> GetPatientByIdAsync(Guid id)
        {
            return await _patientRepository.GetByIdAsync(id);
        }

        public async Task<Patient> GetPatientByIdentityNumberAsync(string identityNumber)
        {
            return await _patientRepository.GetSingleAsync(p => p.IdentityNumber == identityNumber);
        }

        public async Task<IEnumerable<Patient>> SearchPatientsAsync(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return new List<Patient>();
            }

            searchTerm = searchTerm.ToLower();
            return await _patientRepository.FindAsync(p =>
                p.FirstName.ToLower().Contains(searchTerm) ||
                p.LastName.ToLower().Contains(searchTerm) ||
                p.IdentityNumber.Contains(searchTerm) ||
                p.PhoneNumber.Contains(searchTerm) ||
                p.Email.ToLower().Contains(searchTerm));
        }

        public async Task<bool> CreatePatientAsync(Patient patient)
        {
            // Check if identity number already exists
            if (await _patientRepository.ExistsAsync(p => p.IdentityNumber == patient.IdentityNumber))
            {
                return false;
            }

            patient.CreatedAt = DateTime.UtcNow;
            patient.IsActive = true;

            await _patientRepository.AddAsync(patient);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdatePatientAsync(Patient patient)
        {
            var existingPatient = await _patientRepository.GetByIdAsync(patient.Id);
            if (existingPatient == null)
            {
                return false;
            }

            // Check if identity number is being changed and if it conflicts with existing patients
            if (patient.IdentityNumber != existingPatient.IdentityNumber && 
                await _patientRepository.ExistsAsync(p => p.IdentityNumber == patient.IdentityNumber))
            {
                return false;
            }

            existingPatient.IdentityNumber = patient.IdentityNumber;
            existingPatient.FirstName = patient.FirstName;
            existingPatient.LastName = patient.LastName;
            existingPatient.DateOfBirth = patient.DateOfBirth;
            existingPatient.Gender = patient.Gender;
            existingPatient.BloodType = patient.BloodType;
            existingPatient.Address = patient.Address;
            existingPatient.City = patient.City;
            existingPatient.Country = patient.Country;
            existingPatient.PhoneNumber = patient.PhoneNumber;
            existingPatient.Email = patient.Email;
            existingPatient.EmergencyContactName = patient.EmergencyContactName;
            existingPatient.EmergencyContactPhone = patient.EmergencyContactPhone;
            existingPatient.InsuranceProvider = patient.InsuranceProvider;
            existingPatient.InsuranceNumber = patient.InsuranceNumber;
            existingPatient.UpdatedAt = DateTime.UtcNow;

            _patientRepository.Update(existingPatient);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeletePatientAsync(Guid id)
        {
            var patient = await _patientRepository.GetByIdAsync(id);
            if (patient == null)
            {
                return false;
            }

            // Soft delete
            patient.IsActive = false;
            patient.UpdatedAt = DateTime.UtcNow;

            _patientRepository.Update(patient);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<MedicalRecord>> GetPatientMedicalRecordsAsync(Guid patientId)
        {
            return await _medicalRecordRepository.FindAsync(mr => mr.PatientId == patientId);
        }

        public async Task<IEnumerable<Appointment>> GetPatientAppointmentsAsync(Guid patientId)
        {
            return await _appointmentRepository.FindAsync(a => a.PatientId == patientId);
        }

        public async Task<IEnumerable<Prescription>> GetPatientPrescriptionsAsync(Guid patientId)
        {
            return await _prescriptionRepository.FindAsync(p => p.PatientId == patientId);
        }

        public async Task<IEnumerable<Invoice>> GetPatientInvoicesAsync(Guid patientId)
        {
            return await _invoiceRepository.FindAsync(i => i.PatientId == patientId);
        }
    }
}
