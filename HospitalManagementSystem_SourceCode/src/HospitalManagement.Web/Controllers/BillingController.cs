using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HospitalManagement.Business.Interfaces;
using HospitalManagement.Domain.Entities;
using HospitalManagement.Web.ViewModels;

namespace HospitalManagement.Web.Controllers
{
    public class BillingController : Controller
    {
        private readonly IBillingService _billingService;
        private readonly IPatientService _patientService;

        public BillingController(
            IBillingService billingService,
            IPatientService patientService)
        {
            _billingService = billingService;
            _patientService = patientService;
        }

        // GET: Billing
        public async Task<IActionResult> Index()
        {
            var invoices = await _billingService.GetAllInvoicesAsync();
            return View(invoices);
        }

        // GET: Billing/InvoiceDetails/5
        public async Task<IActionResult> InvoiceDetails(Guid id)
        {
            var invoice = await _billingService.GetInvoiceByIdAsync(id);
            if (invoice == null)
            {
                return NotFound();
            }

            return View(invoice);
        }

        // GET: Billing/CreateInvoice
        public async Task<IActionResult> CreateInvoice(Guid patientId)
        {
            var patient = await _patientService.GetPatientByIdAsync(patientId);
            if (patient == null)
            {
                return NotFound();
            }

            var invoiceViewModel = new InvoiceViewModel
            {
                PatientId = patientId,
                InvoiceDate = DateTime.UtcNow,
                DueDate = DateTime.UtcNow.AddDays(30),
                Status = "Pending"
            };

            ViewBag.Patient = patient;
            return View(invoiceViewModel);
        }

        // POST: Billing/CreateInvoice
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateInvoice(InvoiceViewModel invoiceViewModel)
        {
            if (ModelState.IsValid)
            {
                var invoice = new Invoice
                {
                    PatientId = invoiceViewModel.PatientId,
                    InvoiceNumber = GenerateInvoiceNumber(),
                    InvoiceDate = invoiceViewModel.InvoiceDate,
                    DueDate = invoiceViewModel.DueDate,
                    TotalAmount = 0, // Will be updated when items are added
                    PaidAmount = 0,
                    Status = invoiceViewModel.Status,
                    PaymentMethod = invoiceViewModel.PaymentMethod,
                    Notes = invoiceViewModel.Notes,
                    CreatedById = invoiceViewModel.CreatedById
                };

                var result = await _billingService.CreateInvoiceAsync(invoice);
                if (result)
                {
                    return RedirectToAction(nameof(AddInvoiceItems), new { id = invoice.Id });
                }
                
                ModelState.AddModelError("", "Failed to create invoice.");
            }
            
            var patient = await _patientService.GetPatientByIdAsync(invoiceViewModel.PatientId);
            ViewBag.Patient = patient;
            
            return View(invoiceViewModel);
        }

        // GET: Billing/EditInvoice/5
        public async Task<IActionResult> EditInvoice(Guid id)
        {
            var invoice = await _billingService.GetInvoiceByIdAsync(id);
            if (invoice == null)
            {
                return NotFound();
            }

            var patient = await _patientService.GetPatientByIdAsync(invoice.PatientId);

            var invoiceViewModel = new InvoiceViewModel
            {
                Id = invoice.Id,
                PatientId = invoice.PatientId,
                InvoiceNumber = invoice.InvoiceNumber,
                InvoiceDate = invoice.InvoiceDate,
                DueDate = invoice.DueDate,
                TotalAmount = invoice.TotalAmount,
                PaidAmount = invoice.PaidAmount,
                Status = invoice.Status,
                PaymentMethod = invoice.PaymentMethod,
                Notes = invoice.Notes,
                CreatedById = invoice.CreatedById
            };

            ViewBag.Patient = patient;
            return View(invoiceViewModel);
        }

        // POST: Billing/EditInvoice/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditInvoice(Guid id, InvoiceViewModel invoiceViewModel)
        {
            if (id != invoiceViewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var invoice = new Invoice
                {
                    Id = invoiceViewModel.Id,
                    PatientId = invoiceViewModel.PatientId,
                    InvoiceNumber = invoiceViewModel.InvoiceNumber,
                    InvoiceDate = invoiceViewModel.InvoiceDate,
                    DueDate = invoiceViewModel.DueDate,
                    TotalAmount = invoiceViewModel.TotalAmount,
                    PaidAmount = invoiceViewModel.PaidAmount,
                    Status = invoiceViewModel.Status,
                    PaymentMethod = invoiceViewModel.PaymentMethod,
                    Notes = invoiceViewModel.Notes,
                    CreatedById = invoiceViewModel.CreatedById
                };

                var result = await _billingService.UpdateInvoiceAsync(invoice);
                if (result)
                {
                    return RedirectToAction(nameof(InvoiceDetails), new { id = invoice.Id });
                }
                
                ModelState.AddModelError("", "Failed to update invoice.");
            }
            
            var patient = await _patientService.GetPatientByIdAsync(invoiceViewModel.PatientId);
            ViewBag.Patient = patient;
            
            return View(invoiceViewModel);
        }

        // GET: Billing/DeleteInvoice/5
        public async Task<IActionResult> DeleteInvoice(Guid id)
        {
            var invoice = await _billingService.GetInvoiceByIdAsync(id);
            if (invoice == null)
            {
                return NotFound();
            }

            return View(invoice);
        }

        // POST: Billing/DeleteInvoice/5
        [HttpPost, ActionName("DeleteInvoice")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteInvoiceConfirmed(Guid id)
        {
            await _billingService.DeleteInvoiceAsync(id);
            return RedirectToAction(nameof(Index));
        }

        // GET: Billing/AddInvoiceItems/5
        public async Task<IActionResult> AddInvoiceItems(Guid id)
        {
            var invoice = await _billingService.GetInvoiceByIdAsync(id);
            if (invoice == null)
            {
                return NotFound();
            }

            ViewBag.Invoice = invoice;
            return View();
        }

        // POST: Billing/AddInvoiceItem
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddInvoiceItem(InvoiceItemViewModel itemViewModel)
        {
            if (ModelState.IsValid)
            {
                var item = new InvoiceItem
                {
                    InvoiceId = itemViewModel.InvoiceId,
                    Description = itemViewModel.Description,
                    Quantity = itemViewModel.Quantity,
                    UnitPrice = itemViewModel.UnitPrice,
                    Discount = itemViewModel.Discount,
                    TaxRate = itemViewModel.TaxRate,
                    TotalPrice = CalculateTotalPrice(itemViewModel.Quantity, itemViewModel.UnitPrice, itemViewModel.Discount, itemViewModel.TaxRate),
                    ServiceType = itemViewModel.ServiceType,
                    ServiceId = itemViewModel.ServiceId
                };

                var result = await _billingService.AddInvoiceItemAsync(item);
                if (result)
                {
                    // Update invoice total
                    var invoice = await _billingService.GetInvoiceByIdAsync(itemViewModel.InvoiceId);
                    invoice.TotalAmount += item.TotalPrice;
                    await _billingService.UpdateInvoiceAsync(invoice);
                    
                    return RedirectToAction(nameof(InvoiceDetails), new { id = itemViewModel.InvoiceId });
                }
                
                ModelState.AddModelError("", "Failed to add invoice item.");
            }
            
            var invoiceForView = await _billingService.GetInvoiceByIdAsync(itemViewModel.InvoiceId);
            ViewBag.Invoice = invoiceForView;
            
            return View("AddInvoiceItems", itemViewModel);
        }

        // GET: Billing/ProcessPayment/5
        public async Task<IActionResult> ProcessPayment(Guid id)
        {
            var invoice = await _billingService.GetInvoiceByIdAsync(id);
            if (invoice == null)
            {
                return NotFound();
            }

            var paymentViewModel = new PaymentViewModel
            {
                InvoiceId = id,
                PaymentDate = DateTime.UtcNow,
                Amount = invoice.TotalAmount - invoice.PaidAmount,
                Status = "Completed"
            };

            ViewBag.Invoice = invoice;
            return View(paymentViewModel);
        }

        // POST: Billing/ProcessPayment
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProcessPayment(PaymentViewModel paymentViewModel)
        {
            if (ModelState.IsValid)
            {
                var payment = new Payment
                {
                    InvoiceId = paymentViewModel.InvoiceId,
                    PaymentDate = paymentViewModel.PaymentDate,
                    Amount = paymentViewModel.Amount,
                    PaymentMethod = paymentViewModel.PaymentMethod,
                    TransactionId = paymentViewModel.TransactionId,
                    Status = paymentViewModel.Status,
                    Notes = paymentViewModel.Notes,
                    ReceivedById = paymentViewModel.ReceivedById
                };

                var result = await _billingService.ProcessPaymentAsync(payment);
                if (result)
                {
                    return RedirectToAction(nameof(InvoiceDetails), new { id = paymentViewModel.InvoiceId });
                }
                
                ModelState.AddModelError("", "Failed to process payment.");
            }
            
            var invoice = await _billingService.GetInvoiceByIdAsync(paymentViewModel.InvoiceId);
            ViewBag.Invoice = invoice;
            
            return View(paymentViewModel);
        }

        // GET: Billing/PatientInvoices/5
        public async Task<IActionResult> PatientInvoices(Guid id)
        {
            var patient = await _patientService.GetPatientByIdAsync(id);
            if (patient == null)
            {
                return NotFound();
            }

            var invoices = await _billingService.GetInvoicesByPatientAsync(id);
            ViewBag.Patient = patient;
            ViewBag.OutstandingBalance = await _billingService.GetOutstandingBalanceAsync(id);
            
            return View(invoices);
        }

        // GET: Billing/InvoicesByStatus
        public async Task<IActionResult> InvoicesByStatus(string status)
        {
            var invoices = await _billingService.GetInvoicesByStatusAsync(status);
            ViewBag.Status = status;
            return View(invoices);
        }

        // GET: Billing/RevenueReport
        public async Task<IActionResult> RevenueReport(DateTime? startDate, DateTime? endDate)
        {
            startDate = startDate ?? DateTime.UtcNow.AddMonths(-1);
            endDate = endDate ?? DateTime.UtcNow;

            var totalRevenue = await _billingService.GetTotalRevenueAsync(startDate.Value, endDate.Value);
            ViewBag.StartDate = startDate.Value;
            ViewBag.EndDate = endDate.Value;
            ViewBag.TotalRevenue = totalRevenue;
            
            return View();
        }

        private string GenerateInvoiceNumber()
        {
            // Simple invoice number generation
            return "INV-" + DateTime.UtcNow.ToString("yyyyMMdd") + "-" + Guid.NewGuid().ToString().Substring(0, 8).ToUpper();
        }

        private decimal CalculateTotalPrice(int quantity, decimal unitPrice, decimal discount, decimal taxRate)
        {
            var subtotal = quantity * unitPrice;
            var discountAmount = subtotal * (discount / 100);
            var taxAmount = (subtotal - discountAmount) * (taxRate / 100);
            return subtotal - discountAmount + taxAmount;
        }
    }
}
