# Hastane Otomasyon Sistemi Dokümantasyonu

## Genel Bakış

Bu dokümantasyon, ASP.NET Core, Entity Framework Core ve PostgreSQL kullanılarak geliştirilen Hastane Otomasyon Sistemi'nin teknik detaylarını içermektedir. Sistem, SOLID prensiplerine ve MVC mimarisine uygun olarak tasarlanmış ve geliştirilmiştir.

## Sistem Gereksinimleri

- .NET Core SDK 5.0 veya üzeri
- PostgreSQL 12.0 veya üzeri
- Modern bir web tarayıcısı (Chrome, Firefox, Edge vb.)

## Proje Yapısı

Proje, N-Tier (Çok Katmanlı) mimari kullanılarak aşağıdaki katmanlardan oluşmaktadır:

1. **HospitalManagement.Domain**: Entity sınıflarını içeren domain katmanı
2. **HospitalManagement.DataAccess**: Veritabanı erişim katmanı (Repository Pattern ve Unit of Work)
3. **HospitalManagement.Business**: İş mantığı katmanı (Service Layer)
4. **HospitalManagement.Web**: Kullanıcı arayüzü katmanı (MVC)

## Veritabanı Şeması

Sistem, aşağıdaki ana tablolardan oluşan bir PostgreSQL veritabanı kullanmaktadır:

- Users: Sistem kullanıcıları
- Roles: Kullanıcı rolleri
- UserRoles: Kullanıcı-rol ilişkileri
- Patients: Hasta bilgileri
- Departments: Hastane departmanları
- Doctors: Doktor bilgileri
- DoctorSchedules: Doktor çalışma programları
- DoctorLeaves: Doktor izin bilgileri
- Appointments: Randevu bilgileri
- MedicalRecords: Tıbbi kayıtlar
- Prescriptions: Reçeteler
- PrescriptionItems: Reçete detayları
- Medications: İlaç bilgileri
- LabTests: Laboratuvar testleri
- LabTestRequests: Test istekleri
- LabTestRequestItems: Test istek detayları
- Invoices: Faturalar
- InvoiceItems: Fatura detayları
- Payments: Ödemeler
- Notifications: Bildirimler
- AuditLogs: Sistem log kayıtları

## Kurulum

1. PostgreSQL veritabanını kurun ve yeni bir veritabanı oluşturun
2. `appsettings.json` dosyasındaki bağlantı dizesini (connection string) kendi veritabanı bilgilerinizle güncelleyin
3. Aşağıdaki komutları çalıştırarak veritabanını oluşturun:

```bash
dotnet ef migrations add InitialCreate --project HospitalManagement.DataAccess --startup-project HospitalManagement.Web
dotnet ef database update --project HospitalManagement.DataAccess --startup-project HospitalManagement.Web
```

4. Projeyi derleyin ve çalıştırın:

```bash
dotnet build
dotnet run --project HospitalManagement.Web
```

## Kullanım

Sistem, aşağıdaki ana modüllerden oluşmaktadır:

1. **Hasta Yönetimi**: Hasta kayıtları, hasta arama, hasta detayları
2. **Doktor Yönetimi**: Doktor kayıtları, doktor programları, izin yönetimi
3. **Randevu Yönetimi**: Randevu oluşturma, randevu listeleme, randevu iptal etme
4. **Tıbbi Kayıt Yönetimi**: Muayene kayıtları, hasta geçmişi
5. **Reçete Yönetimi**: Reçete oluşturma, ilaç listeleme
6. **Laboratuvar Yönetimi**: Test istekleri, test sonuçları
7. **Faturalama**: Fatura oluşturma, ödeme alma, fatura raporları

## Mimari Detaylar

### Domain Katmanı

Domain katmanı, sistemin temel veri modellerini içerir. Her entity, `BaseEntity` sınıfından türetilmiştir ve temel özellikleri (Id, CreatedAt, UpdatedAt vb.) içerir.

### DataAccess Katmanı

DataAccess katmanı, Entity Framework Core kullanarak veritabanı erişimini sağlar. Repository Pattern ve Unit of Work desenleri kullanılarak veri erişimi soyutlanmıştır.

- **GenericRepository**: Temel CRUD işlemlerini içeren generic repository
- **UnitOfWork**: Transaction yönetimi için Unit of Work deseni

### Business Katmanı

Business katmanı, sistemin iş mantığını içerir. Her servis, ilgili domain alanıyla ilgili işlemleri yönetir.

- **UserService**: Kullanıcı yönetimi
- **PatientService**: Hasta yönetimi
- **DoctorService**: Doktor yönetimi
- **AppointmentService**: Randevu yönetimi
- **MedicalRecordService**: Tıbbi kayıt yönetimi
- **PrescriptionService**: Reçete yönetimi
- **LabTestService**: Laboratuvar test yönetimi
- **BillingService**: Faturalama ve ödeme yönetimi

### Web Katmanı

Web katmanı, ASP.NET Core MVC kullanarak kullanıcı arayüzünü sağlar. Controller'lar, View'lar ve ViewModel'ler bu katmanda yer alır.

- **Controllers**: Kullanıcı isteklerini işleyen controller sınıfları
- **Views**: Kullanıcı arayüzünü oluşturan Razor view'ları
- **ViewModels**: View'lara özel veri modellerini temsil eden ViewModel sınıfları

## SOLID ve MVC Uyumluluğu

Sistem, SOLID prensiplerine ve MVC mimarisine uygun olarak tasarlanmıştır. Detaylı analiz için `docs/solid_mvc_analysis.md` dosyasına bakınız.

## Lisans

Bu proje, MIT lisansı altında lisanslanmıştır. Detaylar için `LICENSE` dosyasına bakınız.
