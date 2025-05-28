using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HospitalManagement.Domain.Entities;

namespace HospitalManagement.Business.Interfaces
{
    public interface IBillingService
    {
        Task<IEnumerable<Invoice>> GetAllInvoicesAsync();
        Task<Invoice> GetInvoiceByIdAsync(Guid id);
        Task<Invoice> GetInvoiceByNumberAsync(string invoiceNumber);
        Task<IEnumerable<Invoice>> GetInvoicesByPatientAsync(Guid patientId);
        Task<IEnumerable<Invoice>> GetInvoicesByStatusAsync(string status);
        Task<IEnumerable<Invoice>> GetInvoicesByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<bool> CreateInvoiceAsync(Invoice invoice);
        Task<bool> UpdateInvoiceAsync(Invoice invoice);
        Task<bool> DeleteInvoiceAsync(Guid id);
        Task<bool> AddInvoiceItemAsync(InvoiceItem item);
        Task<bool> UpdateInvoiceItemAsync(InvoiceItem item);
        Task<bool> DeleteInvoiceItemAsync(Guid itemId);
        Task<bool> ProcessPaymentAsync(Payment payment);
        Task<IEnumerable<Payment>> GetPaymentsByInvoiceAsync(Guid invoiceId);
        Task<decimal> GetTotalRevenueAsync(DateTime startDate, DateTime endDate);
        Task<decimal> GetOutstandingBalanceAsync(Guid patientId);
    }
}
