# Salon Design Application

Restaurant için salon tasarım uygulaması - C# 7.3 ile geliştirilmiştir.

## Özellikler

### Masa Yönetimi
- Kare masa ekleme
- Yuvarlak masa ekleme
- Masa numarası ve isim atama

### Duvar ve Objeler
- Duvar ekleme (kareli pattern)
- Dekoratif obje ekleme
- Objelere başlık/title ekleme
- Yuvarlak, kare ve dikdörtgen şekil seçenekleri

### Tasarımsal Özellikler
- Renk değiştirme
- Metin ekleme
- Font ayarları (font ailesi ve boyut)
- Başlık ekleme
- Şekil değiştirme

### Etkileşim
- Drag & drop ile obje taşıma
- Objeleri resize etme (köşelerden boyutlandırma)
- Obje seçimi ve özellik düzenleme

### Veritabanı
- `sd_salon` tablosu: Salon bilgileri
- `sd_table` tablosu: Masa/obje pozisyonları ve özellikleri
- Entity Framework 6.4.4 ile veritabanı yönetimi

## Proje Yapısı

```
SalonDesign/
├── Models/
│   ├── Salon.cs                  # sd_salon tablosu
│   ├── SalonObject.cs            # sd_table tablosu
│   └── DesignProperties.cs       # Tasarım özellikleri yönetimi
├── Enums/
│   ├── ObjectType.cs             # Masa, Duvar, Dekorasyon tipleri
│   └── ShapeType.cs              # Yuvarlak, Kare, Dikdörtgen şekilleri
├── Data/
│   └── SalonDesignContext.cs     # Entity Framework DbContext
├── Services/
│   ├── SalonRepository.cs        # Veritabanı işlemleri
│   ├── SalonDesignService.cs     # İş mantığı
│   └── DesignPropertyService.cs  # Tasarım özellikleri servisi
├── Forms/
│   └── SalonDesignForm.cs        # Ana WinForms UI
├── Program.cs                     # Uygulama giriş noktası
├── App.config                     # Yapılandırma ve connection string
└── SalonDesign.csproj            # Proje dosyası
```

## Teknik Detaylar

- **Dil**: C# 7.3
- **Framework**: .NET Framework 4.7.2
- **UI**: Windows Forms
- **ORM**: Entity Framework 6.4.4
- **Veritabanı**: SQL Server LocalDB

## Kurulum

### Gereksinimler
- Visual Studio 2019 veya üzeri
- .NET Framework 4.7.2
- SQL Server LocalDB

### Kurulum Adımları

1. Projeyi Visual Studio'da açın:
   ```
   SalonDesign.sln
   ```

2. NuGet paketlerini geri yükleyin:
   ```
   Tools > NuGet Package Manager > Restore NuGet Packages
   ```

3. Veritabanını oluşturmak için Package Manager Console'da:
   ```
   Enable-Migrations
   Add-Migration InitialCreate
   Update-Database
   ```

4. Projeyi çalıştırın: `F5`

## Kullanım

### Masa Ekleme
1. "Kare Masa" veya "Yuvarlak Masa" butonuna tıklayın
2. Canvas üzerinde masa oluşturulur
3. Masayı sürükleyerek taşıyın
4. Köşelerden sürükleyerek boyutlandırın

### Duvar Ekleme
1. "Duvar Ekle" butonuna tıklayın
2. Kareli pattern ile duvar oluşturulur
3. Sürükle-bırak ve resize işlemleri yapılabilir

### Özellik Düzenleme
1. Bir objeye tıklayın
2. Sağ panelde özellikler görüntülenir:
   - Ad
   - Başlık
   - Metin
   - Masa Numarası
   - Şekil
   - Renk
   - Font Ailesi
   - Font Boyutu
3. Değişiklikleri yapın
4. "Kaydet" butonuna tıklayın

### Obje Silme
1. Silinecek objeyi seçin
2. "Sil" butonuna tıklayın
3. Onaylayın

## Veritabanı Şeması

### sd_salon Tablosu
```sql
CREATE TABLE sd_salon (
    id INT PRIMARY KEY IDENTITY(1,1),
    name NVARCHAR(200) NOT NULL,
    description NVARCHAR(500),
    width INT,
    height INT,
    created_date DATETIME,
    updated_date DATETIME
);
```

### sd_table Tablosu
```sql
CREATE TABLE sd_table (
    id INT PRIMARY KEY IDENTITY(1,1),
    salon_id INT NOT NULL,
    object_type INT NOT NULL,
    shape_type INT NOT NULL,
    name NVARCHAR(100),
    title NVARCHAR(100),
    table_number INT,
    position_x INT,
    position_y INT,
    width INT,
    height INT,
    color NVARCHAR(50),
    text NVARCHAR(200),
    font_family NVARCHAR(100),
    font_size FLOAT,
    created_date DATETIME,
    updated_date DATETIME,
    FOREIGN KEY (salon_id) REFERENCES sd_salon(id) ON DELETE CASCADE
);
```

## Özellikler

### ObjectType Enum
- `Table = 1`: Masa
- `Wall = 2`: Duvar
- `Decoration = 3`: Dekorasyon

### ShapeType Enum
- `Circle = 1`: Yuvarlak
- `Square = 2`: Kare
- `Rectangle = 3`: Dikdörtgen

## Geliştirme Notları

- C# 7.3 uyumlu kod yazıldı
- Type-safe özellik yönetimi için DesignProperties sınıfı kullanıldı
- Drag & drop için MouseDown, MouseMove, MouseUp event'leri kullanıldı
- Resize için köşelerde handle'lar çizildi
- Duvarlar için kareli (checker) pattern implementasyonu yapıldı
- Entity Framework ile veritabanı işlemleri yönetildi
- Repository pattern kullanıldı
- Service katmanı ile iş mantığı ayrıştırıldı

## Lisans

Bu proje eğitim amaçlı geliştirilmiştir.
