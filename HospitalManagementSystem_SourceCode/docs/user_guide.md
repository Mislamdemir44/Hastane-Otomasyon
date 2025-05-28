# Kurulum ve Kullanım Kılavuzu

## Kurulum Adımları

1. **Gereksinimler**
   - .NET Core SDK 5.0 veya üzeri
   - PostgreSQL 12.0 veya üzeri
   - Modern bir web tarayıcısı (Chrome, Firefox, Edge vb.)

2. **Veritabanı Kurulumu**
   - PostgreSQL'i kurun ve yeni bir veritabanı oluşturun:
   ```sql
   CREATE DATABASE HospitalManagementDB;
   ```
   - Veritabanı kullanıcı adı ve şifresini not alın

3. **Proje Yapılandırması**
   - `appsettings.json` dosyasındaki bağlantı dizesini (connection string) kendi veritabanı bilgilerinizle güncelleyin:
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Host=localhost;Port=5432;Database=HospitalManagementDB;Username=KULLANICI_ADI;Password=SIFRE;"
   }
   ```

4. **Veritabanı Migrasyonu**
   - Aşağıdaki komutları çalıştırarak veritabanı şemasını oluşturun:
   ```bash
   dotnet ef migrations add InitialCreate --project HospitalManagement.DataAccess --startup-project HospitalManagement.Web
   dotnet ef database update --project HospitalManagement.DataAccess --startup-project HospitalManagement.Web
   ```

5. **Projeyi Çalıştırma**
   - Projeyi derleyin ve çalıştırın:
   ```bash
   dotnet build
   dotnet run --project HospitalManagement.Web
   ```
   - Tarayıcınızda `https://localhost:5001` adresine gidin

## Kullanım Kılavuzu

### 1. Hasta Yönetimi

- **Yeni Hasta Kaydı**: Ana sayfadaki "Yeni Hasta Kaydı" butonuna tıklayın veya Hastalar > Yeni Hasta menüsünü kullanın
- **Hasta Listesi**: Hastalar > Hasta Listesi menüsünden tüm hastaları görüntüleyin
- **Hasta Arama**: Hasta listesi sayfasındaki arama kutusunu kullanarak hasta arayın
- **Hasta Detayları**: Hasta listesinden bir hastanın detaylarını görüntülemek için "Detaylar" butonuna tıklayın

### 2. Randevu Yönetimi

- **Yeni Randevu**: Ana sayfadaki "Yeni Randevu" butonuna tıklayın veya Randevular > Yeni Randevu menüsünü kullanın
- **Randevu Listesi**: Randevular > Tüm Randevular menüsünden randevuları görüntüleyin
- **Tarihe Göre Randevular**: Randevular > Tarihe Göre Randevular menüsünden belirli bir tarihteki randevuları görüntüleyin
- **Randevu İptal**: Randevu detaylarında "İptal Et" butonuna tıklayarak randevuyu iptal edin

### 3. Tıbbi Kayıt Yönetimi

- **Muayene Başlatma**: Randevu listesinden bir randevu için "Muayene Başlat" butonuna tıklayın
- **Tıbbi Kayıt Oluşturma**: Muayene formunu doldurun ve kaydedin
- **Tıbbi Kayıt Listesi**: Tıbbi İşlemler > Tıbbi Kayıtlar menüsünden tüm tıbbi kayıtları görüntüleyin
- **Hasta Tıbbi Geçmişi**: Hasta detaylarında "Tıbbi Kayıtlar" butonuna tıklayarak hastanın tıbbi geçmişini görüntüleyin

### 4. Reçete Yönetimi

- **Reçete Oluşturma**: Tıbbi kayıt detaylarında "Reçete Oluştur" butonuna tıklayın
- **Reçete Listesi**: Tıbbi İşlemler > Reçeteler menüsünden tüm reçeteleri görüntüleyin
- **Reçete Detayları**: Reçete listesinden bir reçetenin detaylarını görüntülemek için "Detaylar" butonuna tıklayın

### 5. Laboratuvar Yönetimi

- **Test İsteği Oluşturma**: Tıbbi kayıt detaylarında "Test İsteği" butonuna tıklayın
- **Test İstekleri**: Tıbbi İşlemler > Laboratuvar Testleri > Test İstekleri menüsünden tüm test isteklerini görüntüleyin
- **Test Sonuçları Girme**: Test isteği detaylarında "Sonuç Gir" butonuna tıklayarak test sonuçlarını girin

### 6. Faturalama

- **Fatura Oluşturma**: Hasta detaylarında "Fatura Oluştur" butonuna tıklayın
- **Fatura Listesi**: Faturalama > Tüm Faturalar menüsünden tüm faturaları görüntüleyin
- **Ödeme Alma**: Fatura detaylarında "Ödeme Al" butonuna tıklayarak ödeme kaydı oluşturun
- **Gelir Raporu**: Faturalama > Gelir Raporu menüsünden gelir raporlarını görüntüleyin

## Sorun Giderme

- **Veritabanı Bağlantı Hatası**: `appsettings.json` dosyasındaki bağlantı dizesini kontrol edin
- **Migrasyon Hatası**: Migrasyon komutlarını doğru sırayla çalıştırdığınızdan emin olun
- **Uygulama Çalışmıyor**: .NET Core SDK sürümünüzün uyumlu olduğundan emin olun

## Destek

Herhangi bir sorun veya soru için lütfen iletişime geçin.
