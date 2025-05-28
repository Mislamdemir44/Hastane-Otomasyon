# Hastane Otomasyon Sistemi

Bu proje, ASP.NET Core, Entity Framework Core ve PostgreSQL kullanılarak geliştirilmiş kapsamlı bir hastane otomasyon sistemidir. SOLID prensiplerine ve MVC mimarisine uygun olarak tasarlanmış olup, hastane yönetimi için gerekli tüm temel modülleri içermektedir.

## Özellikler

- **Hasta Yönetimi**: Hasta kayıtları, hasta arama, hasta detayları
- **Doktor Yönetimi**: Doktor kayıtları, doktor programları, izin yönetimi
- **Randevu Yönetimi**: Randevu oluşturma, randevu listeleme, randevu iptal etme
- **Tıbbi Kayıt Yönetimi**: Muayene kayıtları, hasta geçmişi
- **Reçete Yönetimi**: Reçete oluşturma, ilaç listeleme
- **Laboratuvar Yönetimi**: Test istekleri, test sonuçları
- **Faturalama**: Fatura oluşturma, ödeme alma, fatura raporları

## Teknoloji Stack

- **Backend**: ASP.NET Core 5.0
- **ORM**: Entity Framework Core 5.0
- **Veritabanı**: PostgreSQL 12.0+
- **Frontend**: ASP.NET Core MVC, Bootstrap 4, jQuery
- **Mimari**: N-Tier Architecture, Repository Pattern, Unit of Work

## Sistem Gereksinimleri

- .NET Core SDK 5.0 veya üzeri
- PostgreSQL 12.0 veya üzeri
- Modern bir web tarayıcısı (Chrome, Firefox, Edge vb.)

## Kurulum

1. **Veritabanı Kurulumu**
   ```sql
   CREATE DATABASE HospitalManagementDB;
   ```

2. **Proje Yapılandırması**
   - `appsettings.json` dosyasındaki bağlantı dizesini (connection string) kendi veritabanı bilgilerinizle güncelleyin:
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Host=localhost;Port=5432;Database=HospitalManagementDB;Username=KULLANICI_ADI;Password=SIFRE;"
   }
   ```

3. **Veritabanı Migrasyonu**
   ```bash
   dotnet ef migrations add InitialCreate --project HospitalManagement.DataAccess --startup-project HospitalManagement.Web
   dotnet ef database update --project HospitalManagement.DataAccess --startup-project HospitalManagement.Web
   ```

4. **Projeyi Çalıştırma**
   ```bash
   dotnet build
   dotnet run --project HospitalManagement.Web
   ```

5. **Tarayıcıda Açma**
   - Tarayıcınızda `https://localhost:5001` adresine gidin

## Proje Yapısı

```
HospitalManagementSystem/
├── src/
│   ├── HospitalManagement.Domain/        # Entity sınıfları
│   ├── HospitalManagement.DataAccess/    # Veritabanı erişim katmanı
│   ├── HospitalManagement.Business/      # İş mantığı katmanı
│   └── HospitalManagement.Web/           # Kullanıcı arayüzü katmanı (MVC)
└── docs/
    ├── requirements.md                   # Sistem gereksinimleri
    ├── architecture.md                   # Mimari tasarım
    ├── database_schema.md                # Veritabanı şeması
    ├── documentation.md                  # Teknik dokümantasyon
    ├── user_guide.md                     # Kullanım kılavuzu
    └── solid_mvc_analysis.md             # SOLID ve MVC analizi
```

## Katmanlar

### Domain Katmanı
Domain katmanı, sistemin temel veri modellerini içerir. Her entity, `BaseEntity` sınıfından türetilmiştir ve temel özellikleri (Id, CreatedAt, UpdatedAt vb.) içerir.

### DataAccess Katmanı
DataAccess katmanı, Entity Framework Core kullanarak veritabanı erişimini sağlar. Repository Pattern ve Unit of Work desenleri kullanılarak veri erişimi soyutlanmıştır.

### Business Katmanı
Business katmanı, sistemin iş mantığını içerir. Her servis, ilgili domain alanıyla ilgili işlemleri yönetir.

### Web Katmanı
Web katmanı, ASP.NET Core MVC kullanarak kullanıcı arayüzünü sağlar. Controller'lar, View'lar ve ViewModel'ler bu katmanda yer alır.

## Kullanım

Detaylı kullanım bilgileri için `docs/user_guide.md` dosyasına bakınız.

## SOLID ve MVC Uyumluluğu

Sistem, SOLID prensiplerine ve MVC mimarisine uygun olarak tasarlanmıştır. Detaylı analiz için `docs/solid_mvc_analysis.md` dosyasına bakınız.

## Katkıda Bulunma

1. Bu repo'yu fork edin
2. Feature branch'inizi oluşturun (`git checkout -b feature/amazing-feature`)
3. Değişikliklerinizi commit edin (`git commit -m 'Add some amazing feature'`)
4. Branch'inize push edin (`git push origin feature/amazing-feature`)
5. Pull Request oluşturun

## Lisans

Bu proje MIT lisansı altında lisanslanmıştır. Detaylar için `LICENSE` dosyasına bakınız.

## İletişim

Proje Sahibi - [m.islamdemir44@gmail.com]

Proje Linki: [https://github.com/Mislamdemir44/Hastane-Otomasyon/](https://github.com/Mislamdemir44/Hastane-Otomasyon)
