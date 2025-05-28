using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HospitalManagement.Domain.Entities;

namespace HospitalManagement.Business.Interfaces
{
    public interface ILabTestService
    {
        Task<IEnumerable<LabTest>> GetAllLabTestsAsync();
        Task<LabTest> GetLabTestByIdAsync(Guid id);
        Task<IEnumerable<LabTest>> GetLabTestsByDepartmentAsync(string department);
        Task<IEnumerable<LabTest>> SearchLabTestsAsync(string searchTerm);
        Task<bool> CreateLabTestAsync(LabTest labTest);
        Task<bool> UpdateLabTestAsync(LabTest labTest);
        Task<bool> DeleteLabTestAsync(Guid id);
        
        Task<IEnumerable<LabTestRequest>> GetAllLabTestRequestsAsync();
        Task<LabTestRequest> GetLabTestRequestByIdAsync(Guid id);
        Task<IEnumerable<LabTestRequest>> GetLabTestRequestsByPatientAsync(Guid patientId);
        Task<IEnumerable<LabTestRequest>> GetLabTestRequestsByDoctorAsync(Guid doctorId);
        Task<IEnumerable<LabTestRequest>> GetLabTestRequestsByStatusAsync(string status);
        Task<IEnumerable<LabTestRequest>> GetLabTestRequestsByPriorityAsync(string priority);
        Task<bool> CreateLabTestRequestAsync(LabTestRequest request);
        Task<bool> UpdateLabTestRequestAsync(LabTestRequest request);
        Task<bool> CancelLabTestRequestAsync(Guid id);
        
        Task<bool> AddLabTestRequestItemAsync(LabTestRequestItem item);
        Task<bool> UpdateLabTestRequestItemAsync(LabTestRequestItem item);
        Task<bool> DeleteLabTestRequestItemAsync(Guid itemId);
        Task<bool> UpdateLabTestResultAsync(Guid itemId, string resultValue, string remarks, Guid technicianId);
        Task<bool> VerifyLabTestResultAsync(Guid itemId, Guid verifiedById);
    }
}
