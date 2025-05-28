# Hastane Otomasyon Sistemi Veritabanı Şeması

## Veritabanı Tasarımı

Hastane Otomasyon Sistemi için PostgreSQL veritabanı kullanılacaktır. Veritabanı şeması, Entity Framework Core Code-First yaklaşımı ile oluşturulacaktır. Aşağıda, sistemin ana tablolarını ve aralarındaki ilişkileri detaylandıran veritabanı şeması bulunmaktadır.

## Ana Tablolar ve İlişkiler

### 1. Users (Kullanıcılar)
- **Id**: UUID (Primary Key)
- **UserName**: VARCHAR(50) (Unique)
- **Email**: VARCHAR(100) (Unique)
- **PasswordHash**: VARCHAR(256)
- **FirstName**: VARCHAR(50)
- **LastName**: VARCHAR(50)
- **PhoneNumber**: VARCHAR(20)
- **IsActive**: BOOLEAN
- **CreatedAt**: TIMESTAMP
- **UpdatedAt**: TIMESTAMP
- **LastLoginAt**: TIMESTAMP

### 2. Roles (Roller)
- **Id**: UUID (Primary Key)
- **Name**: VARCHAR(50) (Unique)
- **Description**: VARCHAR(200)

### 3. UserRoles (Kullanıcı Rolleri)
- **UserId**: UUID (Foreign Key -> Users.Id)
- **RoleId**: UUID (Foreign Key -> Roles.Id)
- **Primary Key**: (UserId, RoleId)

### 4. Patients (Hastalar)
- **Id**: UUID (Primary Key)
- **IdentityNumber**: VARCHAR(20) (Unique, TC Kimlik No)
- **FirstName**: VARCHAR(50)
- **LastName**: VARCHAR(50)
- **DateOfBirth**: DATE
- **Gender**: VARCHAR(10)
- **BloodType**: VARCHAR(5)
- **Address**: VARCHAR(500)
- **City**: VARCHAR(50)
- **Country**: VARCHAR(50)
- **PhoneNumber**: VARCHAR(20)
- **Email**: VARCHAR(100)
- **EmergencyContactName**: VARCHAR(100)
- **EmergencyContactPhone**: VARCHAR(20)
- **InsuranceProvider**: VARCHAR(100)
- **InsuranceNumber**: VARCHAR(50)
- **CreatedAt**: TIMESTAMP
- **UpdatedAt**: TIMESTAMP
- **IsActive**: BOOLEAN

### 5. Departments (Departmanlar/Poliklinikler)
- **Id**: UUID (Primary Key)
- **Name**: VARCHAR(100) (Unique)
- **Description**: VARCHAR(500)
- **IsActive**: BOOLEAN
- **CreatedAt**: TIMESTAMP
- **UpdatedAt**: TIMESTAMP

### 6. Doctors (Doktorlar)
- **Id**: UUID (Primary Key)
- **UserId**: UUID (Foreign Key -> Users.Id)
- **DepartmentId**: UUID (Foreign Key -> Departments.Id)
- **SpecialtyId**: UUID (Foreign Key -> Specialties.Id)
- **LicenseNumber**: VARCHAR(50) (Unique)
- **Education**: VARCHAR(500)
- **Biography**: TEXT
- **ConsultationFee**: DECIMAL(10,2)
- **IsAvailableForAppointment**: BOOLEAN
- **CreatedAt**: TIMESTAMP
- **UpdatedAt**: TIMESTAMP

### 7. Specialties (Uzmanlık Alanları)
- **Id**: UUID (Primary Key)
- **Name**: VARCHAR(100) (Unique)
- **Description**: VARCHAR(500)

### 8. DoctorSchedules (Doktor Çalışma Saatleri)
- **Id**: UUID (Primary Key)
- **DoctorId**: UUID (Foreign Key -> Doctors.Id)
- **DayOfWeek**: INTEGER (0-6, Pazar-Cumartesi)
- **StartTime**: TIME
- **EndTime**: TIME
- **IsAvailable**: BOOLEAN
- **MaxAppointments**: INTEGER

### 9. DoctorLeaves (Doktor İzinleri)
- **Id**: UUID (Primary Key)
- **DoctorId**: UUID (Foreign Key -> Doctors.Id)
- **StartDate**: DATE
- **EndDate**: DATE
- **Reason**: VARCHAR(500)
- **Status**: VARCHAR(20) (Pending, Approved, Rejected)
- **ApprovedById**: UUID (Foreign Key -> Users.Id)
- **CreatedAt**: TIMESTAMP
- **UpdatedAt**: TIMESTAMP

### 10. Appointments (Randevular)
- **Id**: UUID (Primary Key)
- **PatientId**: UUID (Foreign Key -> Patients.Id)
- **DoctorId**: UUID (Foreign Key -> Doctors.Id)
- **DepartmentId**: UUID (Foreign Key -> Departments.Id)
- **AppointmentDate**: DATE
- **StartTime**: TIME
- **EndTime**: TIME
- **Status**: VARCHAR(20) (Scheduled, Completed, Cancelled, No-Show)
- **Type**: VARCHAR(20) (Regular, Follow-up, Emergency)
- **Notes**: TEXT
- **CreatedById**: UUID (Foreign Key -> Users.Id)
- **CreatedAt**: TIMESTAMP
- **UpdatedAt**: TIMESTAMP

### 11. MedicalRecords (Tıbbi Kayıtlar)
- **Id**: UUID (Primary Key)
- **PatientId**: UUID (Foreign Key -> Patients.Id)
- **DoctorId**: UUID (Foreign Key -> Doctors.Id)
- **AppointmentId**: UUID (Foreign Key -> Appointments.Id)
- **VisitDate**: DATE
- **ChiefComplaint**: TEXT
- **Diagnosis**: TEXT
- **Treatment**: TEXT
- **Prescription**: TEXT
- **Notes**: TEXT
- **FollowUpDate**: DATE
- **CreatedAt**: TIMESTAMP
- **UpdatedAt**: TIMESTAMP

### 12. Prescriptions (Reçeteler)
- **Id**: UUID (Primary Key)
- **PatientId**: UUID (Foreign Key -> Patients.Id)
- **DoctorId**: UUID (Foreign Key -> Doctors.Id)
- **MedicalRecordId**: UUID (Foreign Key -> MedicalRecords.Id)
- **PrescriptionDate**: DATE
- **Status**: VARCHAR(20) (Active, Filled, Expired)
- **Notes**: TEXT
- **CreatedAt**: TIMESTAMP
- **UpdatedAt**: TIMESTAMP

### 13. PrescriptionItems (Reçete Kalemleri)
- **Id**: UUID (Primary Key)
- **PrescriptionId**: UUID (Foreign Key -> Prescriptions.Id)
- **MedicationId**: UUID (Foreign Key -> Medications.Id)
- **Dosage**: VARCHAR(100)
- **Frequency**: VARCHAR(100)
- **Duration**: VARCHAR(100)
- **Quantity**: INTEGER
- **Instructions**: TEXT

### 14. Medications (İlaçlar)
- **Id**: UUID (Primary Key)
- **Name**: VARCHAR(200)
- **GenericName**: VARCHAR(200)
- **Description**: TEXT
- **DosageForm**: VARCHAR(50)
- **Strength**: VARCHAR(50)
- **Manufacturer**: VARCHAR(100)
- **StockQuantity**: INTEGER
- **UnitPrice**: DECIMAL(10,2)
- **IsActive**: BOOLEAN
- **CreatedAt**: TIMESTAMP
- **UpdatedAt**: TIMESTAMP

### 15. LabTests (Laboratuvar Testleri)
- **Id**: UUID (Primary Key)
- **Name**: VARCHAR(200)
- **Description**: TEXT
- **Department**: VARCHAR(100)
- **Price**: DECIMAL(10,2)
- **IsActive**: BOOLEAN
- **CreatedAt**: TIMESTAMP
- **UpdatedAt**: TIMESTAMP

### 16. LabTestRequests (Test İstekleri)
- **Id**: UUID (Primary Key)
- **PatientId**: UUID (Foreign Key -> Patients.Id)
- **DoctorId**: UUID (Foreign Key -> Doctors.Id)
- **MedicalRecordId**: UUID (Foreign Key -> MedicalRecords.Id)
- **RequestDate**: DATE
- **Status**: VARCHAR(20) (Requested, In Progress, Completed, Cancelled)
- **Priority**: VARCHAR(20) (Routine, Urgent, STAT)
- **Notes**: TEXT
- **CreatedAt**: TIMESTAMP
- **UpdatedAt**: TIMESTAMP

### 17. LabTestRequestItems (Test İstek Kalemleri)
- **Id**: UUID (Primary Key)
- **LabTestRequestId**: UUID (Foreign Key -> LabTestRequests.Id)
- **LabTestId**: UUID (Foreign Key -> LabTests.Id)
- **Status**: VARCHAR(20) (Pending, In Progress, Completed, Cancelled)
- **ResultDate**: TIMESTAMP
- **ResultValue**: TEXT
- **ReferenceRange**: VARCHAR(100)
- **Remarks**: TEXT
- **TechnicianId**: UUID (Foreign Key -> Users.Id)
- **VerifiedById**: UUID (Foreign Key -> Users.Id)
- **CreatedAt**: TIMESTAMP
- **UpdatedAt**: TIMESTAMP

### 18. Invoices (Faturalar)
- **Id**: UUID (Primary Key)
- **PatientId**: UUID (Foreign Key -> Patients.Id)
- **InvoiceNumber**: VARCHAR(50) (Unique)
- **InvoiceDate**: DATE
- **DueDate**: DATE
- **TotalAmount**: DECIMAL(10,2)
- **PaidAmount**: DECIMAL(10,2)
- **Status**: VARCHAR(20) (Pending, Paid, Partially Paid, Overdue, Cancelled)
- **PaymentMethod**: VARCHAR(50)
- **Notes**: TEXT
- **CreatedById**: UUID (Foreign Key -> Users.Id)
- **CreatedAt**: TIMESTAMP
- **UpdatedAt**: TIMESTAMP

### 19. InvoiceItems (Fatura Kalemleri)
- **Id**: UUID (Primary Key)
- **InvoiceId**: UUID (Foreign Key -> Invoices.Id)
- **Description**: VARCHAR(200)
- **Quantity**: INTEGER
- **UnitPrice**: DECIMAL(10,2)
- **Discount**: DECIMAL(10,2)
- **TaxRate**: DECIMAL(5,2)
- **TotalPrice**: DECIMAL(10,2)
- **ServiceType**: VARCHAR(50) (Consultation, Lab Test, Medication, Procedure)
- **ServiceId**: UUID (Optional, Foreign Key to related service)

### 20. Payments (Ödemeler)
- **Id**: UUID (Primary Key)
- **InvoiceId**: UUID (Foreign Key -> Invoices.Id)
- **PaymentDate**: DATE
- **Amount**: DECIMAL(10,2)
- **PaymentMethod**: VARCHAR(50)
- **TransactionId**: VARCHAR(100)
- **Status**: VARCHAR(20) (Completed, Failed, Refunded)
- **Notes**: TEXT
- **ReceivedById**: UUID (Foreign Key -> Users.Id)
- **CreatedAt**: TIMESTAMP
- **UpdatedAt**: TIMESTAMP

### 21. Notifications (Bildirimler)
- **Id**: UUID (Primary Key)
- **UserId**: UUID (Foreign Key -> Users.Id)
- **Title**: VARCHAR(200)
- **Message**: TEXT
- **Type**: VARCHAR(50) (Appointment, Lab Result, Payment, System)
- **IsRead**: BOOLEAN
- **RelatedEntityType**: VARCHAR(50)
- **RelatedEntityId**: UUID
- **CreatedAt**: TIMESTAMP

### 22. AuditLogs (Denetim Kayıtları)
- **Id**: UUID (Primary Key)
- **UserId**: UUID (Foreign Key -> Users.Id)
- **Action**: VARCHAR(50)
- **EntityType**: VARCHAR(50)
- **EntityId**: UUID
- **OldValues**: JSONB
- **NewValues**: JSONB
- **Timestamp**: TIMESTAMP
- **IpAddress**: VARCHAR(50)

## İlişki Diyagramı

Veritabanı tablolarının ilişkileri aşağıdaki gibidir:

1. **Users** 1--* **UserRoles** *--1 **Roles**
2. **Users** 1--1 **Doctors**
3. **Departments** 1--* **Doctors**
4. **Specialties** 1--* **Doctors**
5. **Doctors** 1--* **DoctorSchedules**
6. **Doctors** 1--* **DoctorLeaves**
7. **Patients** 1--* **Appointments**
8. **Doctors** 1--* **Appointments**
9. **Departments** 1--* **Appointments**
10. **Patients** 1--* **MedicalRecords**
11. **Doctors** 1--* **MedicalRecords**
12. **Appointments** 1--1 **MedicalRecords**
13. **MedicalRecords** 1--* **Prescriptions**
14. **Patients** 1--* **Prescriptions**
15. **Doctors** 1--* **Prescriptions**
16. **Prescriptions** 1--* **PrescriptionItems**
17. **Medications** 1--* **PrescriptionItems**
18. **Patients** 1--* **LabTestRequests**
19. **Doctors** 1--* **LabTestRequests**
20. **MedicalRecords** 1--* **LabTestRequests**
21. **LabTestRequests** 1--* **LabTestRequestItems**
22. **LabTests** 1--* **LabTestRequestItems**
23. **Patients** 1--* **Invoices**
24. **Invoices** 1--* **InvoiceItems**
25. **Invoices** 1--* **Payments**
26. **Users** 1--* **Notifications**
27. **Users** 1--* **AuditLogs**

## Veritabanı İndeksleri

Performans optimizasyonu için aşağıdaki indeksler oluşturulacaktır:

1. **Users**: UserName, Email
2. **Patients**: IdentityNumber, LastName + FirstName
3. **Doctors**: UserId, DepartmentId, SpecialtyId
4. **Appointments**: PatientId, DoctorId, AppointmentDate + StartTime
5. **MedicalRecords**: PatientId, DoctorId, AppointmentId
6. **Prescriptions**: PatientId, DoctorId, MedicalRecordId
7. **LabTestRequests**: PatientId, DoctorId, Status
8. **Invoices**: PatientId, InvoiceNumber, Status
9. **Payments**: InvoiceId, PaymentDate

## Veri Bütünlüğü Kısıtlamaları

1. **Unique Constraints**:
   - Users: UserName, Email
   - Patients: IdentityNumber
   - Doctors: LicenseNumber
   - Departments: Name
   - Specialties: Name
   - Invoices: InvoiceNumber

2. **Check Constraints**:
   - Appointments: EndTime > StartTime
   - DoctorSchedules: EndTime > StartTime
   - DoctorLeaves: EndDate >= StartDate
   - Invoices: TotalAmount >= 0, PaidAmount >= 0
   - Payments: Amount > 0

3. **Foreign Key Constraints**: Tüm ilişkiler için referans bütünlüğü sağlanacaktır.

## Veritabanı Migration Stratejisi

Entity Framework Core Code-First yaklaşımı kullanılarak, aşağıdaki migration stratejisi uygulanacaktır:

1. **Initial Migration**: Temel tablolar ve ilişkiler
2. **Seed Data Migration**: Roller, departmanlar, uzmanlık alanları gibi temel veriler
3. **Feature-based Migrations**: Her yeni özellik için ayrı migration
4. **Schema Evolution**: Şema değişiklikleri için güvenli migration stratejisi

Bu veritabanı şeması, hastane otomasyon sisteminin tüm gereksinimlerini karşılayacak şekilde tasarlanmıştır ve Entity Framework Core ile PostgreSQL veritabanında uygulanacaktır.
