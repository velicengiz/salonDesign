# Test Senaryoları ve Doğrulama

## Manuel Test Checklist

### 1. Kurulum ve Başlatma
- [ ] Visual Studio'da proje açılıyor
- [ ] NuGet paketleri geri yüklenebiliyor
- [ ] Database migration çalıştırılabiliyor
- [ ] Uygulama başlatılıyor (hata yok)
- [ ] Ana form görüntüleniyor

### 2. Masa Ekleme Testleri

#### 2.1 Kare Masa
- [ ] "Kare Masa" butonuna tıklandığında kare masa oluşuyor
- [ ] Masa varsayılan pozisyonda (100, 100) görünüyor
- [ ] Masa boyutu 80x80 piksel
- [ ] Masa üzerinde "Masa 1" yazısı görünüyor
- [ ] Masa veritabanına kaydediliyor

#### 2.2 Yuvarlak Masa
- [ ] "Yuvarlak Masa" butonuna tıklandığında yuvarlak masa oluşuyor
- [ ] Masa varsayılan pozisyonda (200, 100) görünüyor
- [ ] Masa boyutu 80x80 piksel (daire)
- [ ] Masa üzerinde masa numarası görünüyor
- [ ] Masa veritabanına kaydediliyor

#### 2.3 Çoklu Masa
- [ ] Birden fazla masa eklenebiliyor
- [ ] Her masa farklı numara alıyor (1, 2, 3...)
- [ ] Masalar üst üste binebiliyor (normal)

### 3. Duvar Ekleme Testleri

#### 3.1 Duvar Oluşturma
- [ ] "Duvar Ekle" butonuna tıklandığında duvar oluşuyor
- [ ] Duvar kareli (checker) pattern ile gösteriliyor
- [ ] Duvar rengi kahverengi tonunda (#8B4513)
- [ ] Duvar varsayılan boyutu 150x20 piksel
- [ ] Duvar veritabanına kaydediliyor

#### 3.2 Duvar Pattern
- [ ] Kareli pattern doğru görünüyor
- [ ] Her kare 10x10 piksel
- [ ] İki farklı ton renk kullanılıyor
- [ ] Satranç tahtası efekti var

### 4. Dekorasyon Ekleme Testleri
- [ ] "Dekorasyon" butonuna tıklandığında obje oluşuyor
- [ ] Varsayılan şekil yuvarlak
- [ ] Varsayılan boyut 50x50 piksel
- [ ] Dekorasyon veritabanına kaydediliyor

### 5. Obje Seçme Testleri

#### 5.1 Seçim İşlemi
- [ ] Objeye tıklandığında seçiliyor
- [ ] Seçili obje etrafında mavi kesik çizgi border görünüyor
- [ ] 4 köşede mavi kare handle'lar görünüyor
- [ ] Property paneli seçili obje ile doluyor
- [ ] Başka objeye tıklandığında seçim değişiyor

#### 5.2 Seçim - Farklı Şekiller
- [ ] Kare masa seçilebiliyor
- [ ] Yuvarlak masa seçilebiliyor (tıklama testi)
- [ ] Dikdörtgen duvar seçilebiliyor
- [ ] Yuvarlak dekorasyon seçilebiliyor

#### 5.3 Yuvarlak Obje Seçimi
- [ ] Yuvarlak objenin içine tıklandığında seçiliyor
- [ ] Yuvarlak objenin dışına (ama dikdörtgen alanda) tıklandığında seçilmiyor

### 6. Drag & Drop (Taşıma) Testleri

#### 6.1 Temel Taşıma
- [ ] Obje fare ile sürüklenebiliyor
- [ ] Sürükleme sırasında obje hareket ediyor
- [ ] Bırakıldığında yeni pozisyonda kalıyor
- [ ] Yeni pozisyon veritabanına kaydediliyor

#### 6.2 Farklı Obje Tipleri
- [ ] Kare masa taşınabiliyor
- [ ] Yuvarlak masa taşınabiliyor
- [ ] Duvar taşınabiliyor
- [ ] Dekorasyon taşınabiliyor

#### 6.3 Çoklu Taşıma
- [ ] Bir obje taşındıktan sonra başka obje taşınabiliyor
- [ ] Taşıma işlemi canvas sınırlarını aşabiliyor (normal)

### 7. Resize (Boyutlandırma) Testleri

#### 7.1 Temel Resize
- [ ] Köşe handle'ına tıklanıp sürüklenebiliyor
- [ ] Obje boyutu değişiyor
- [ ] Minimum boyut 30x30 piksel
- [ ] Yeni boyut veritabanına kaydediliyor

#### 7.2 Handle Pozisyonları
- [ ] Sol üst köşe handle'ı çalışıyor
- [ ] Sağ üst köşe handle'ı çalışıyor
- [ ] Sol alt köşe handle'ı çalışıyor
- [ ] Sağ alt köşe handle'ı çalışıyor

#### 7.3 Farklı Şekiller
- [ ] Kare masa resize edilebiliyor
- [ ] Yuvarlak masa resize edilebiliyor (daire kalıyor)
- [ ] Duvar resize edilebiliyor
- [ ] Dekorasyon resize edilebiliyor

### 8. Property Panel Testleri

#### 8.1 Ad (Name) Özelliği
- [ ] TextBox'a yazılan ad objeye atanıyor
- [ ] Ad değiştirildiğinde obje güncelleniyor
- [ ] Boş ad kabul ediliyor

#### 8.2 Başlık (Title) Özelliği
- [ ] TextBox'a yazılan başlık objeye atanıyor
- [ ] Başlık obje üzerinde görüntüleniyor
- [ ] Başlık değiştirilebiliyor

#### 8.3 Metin (Text) Özelliği
- [ ] TextBox'a yazılan metin objeye atanıyor
- [ ] Metin obje üzerinde görüntüleniyor
- [ ] Metin değiştirilebiliyor
- [ ] Uzun metin obje içinde gösteriliyor

#### 8.4 Masa Numarası
- [ ] NumericUpDown ile masa numarası değiştirilebiliyor
- [ ] Minimum değer 1
- [ ] Maximum değer 999
- [ ] Masa üzerinde numara görünüyor

#### 8.5 Şekil (Shape) Özelliği
- [ ] ComboBox'ta 3 seçenek var (Yuvarlak, Kare, Dikdörtgen)
- [ ] Yuvarlak seçildiğinde obje yuvarlak oluyor
- [ ] Kare seçildiğinde obje kare oluyor
- [ ] Dikdörtgen seçildiğinde obje dikdörtgen oluyor
- [ ] Şekil değişimi canvas'ta görüntüleniyor

#### 8.6 Renk Özelliği
- [ ] "Renk Seç" butonuna tıklandığında ColorDialog açılıyor
- [ ] Renk seçildiğinde obje rengi değişiyor
- [ ] Seçilen renk canvas'ta görünüyor
- [ ] Renk hex formatında (#RRGGBB) kaydediliyor

#### 8.7 Font Ailesi
- [ ] ComboBox'ta font seçenekleri var (Arial, Times New Roman, Verdana, Calibri)
- [ ] Seçilen font obje metni için uygulanıyor
- [ ] Font değişimi canvas'ta görünüyor

#### 8.8 Font Boyutu
- [ ] NumericUpDown ile font boyutu değiştirilebiliyor
- [ ] Minimum değer 6
- [ ] Maximum değer 72
- [ ] Font boyutu obje metni için uygulanıyor

### 9. Kaydet Butonu Testleri
- [ ] Obje seçili değilken buton çalışmıyor (normal)
- [ ] Obje seçiliyken buton çalışıyor
- [ ] Kaydet butonuna tıklandığında veritabanı güncelleniyor
- [ ] Başarı mesajı gösteriliyor
- [ ] Değişiklikler kalıcı oluyor

### 10. Sil Butonu Testleri
- [ ] Obje seçili değilken buton çalışmıyor (normal)
- [ ] Obje seçiliyken buton çalışıyor
- [ ] Onay dialog'u gösteriliyor
- [ ] "Evet" seçildiğinde obje siliniyor
- [ ] "Hayır" seçildiğinde obje silinmiyor
- [ ] Silinen obje canvas'tan kalkıyor
- [ ] Silinen obje veritabanından siliniyor

### 11. Veritabanı Testleri

#### 11.1 Salon Tablosu (sd_salon)
- [ ] İlk açılışta otomatik salon oluşuyor
- [ ] Salon adı "Ana Salon"
- [ ] Salon boyutları 1000x700
- [ ] CreatedDate atanıyor
- [ ] Tablo doğru oluşuyor

#### 11.2 Obje Tablosu (sd_table)
- [ ] Yeni obje eklendiğinde kayıt oluşuyor
- [ ] SalonId foreign key çalışıyor
- [ ] ObjectType doğru kaydediliyor (1=Masa, 2=Duvar, 3=Dekorasyon)
- [ ] ShapeType doğru kaydediliyor (1=Yuvarlak, 2=Kare, 3=Dikdörtgen)
- [ ] Pozisyon bilgileri kaydediliyor
- [ ] Tasarım özellikleri kaydediliyor
- [ ] CreatedDate ve UpdatedDate çalışıyor

#### 11.3 CRUD İşlemleri
- [ ] Create (Ekleme) çalışıyor
- [ ] Read (Okuma) çalışıyor
- [ ] Update (Güncelleme) çalışıyor
- [ ] Delete (Silme) çalışıyor
- [ ] Cascade delete çalışıyor (salon silindiğinde objeler de siliniyor)

### 12. Uygulama Kapanış Testleri
- [ ] Form kapatıldığında repository dispose ediliyor
- [ ] Veritabanı bağlantısı kapanıyor
- [ ] Memory leak yok
- [ ] Değişiklikler kaydedilmiş durumda

### 13. Edge Case Testleri

#### 13.1 Sınır Değerler
- [ ] Minimum resize boyutu (30x30) kontrol ediliyor
- [ ] Canvas dışına taşınan objeler taşınabiliyor
- [ ] Çok küçük obje seçilebiliyor
- [ ] Çok büyük obje oluşturulabiliyor

#### 13.2 Boş Değerler
- [ ] Boş ad ile obje oluşturabiliyor
- [ ] Boş başlık ile obje oluşturabiliyor
- [ ] Boş metin ile obje oluşturabiliyor

#### 13.3 Çoklu İşlemler
- [ ] Hızlı ardışık tıklamalar çalışıyor
- [ ] Hızlı sürükleme işlemleri çalışıyor
- [ ] Aynı anda sadece bir obje seçilebiliyor

### 14. Görsel Testler

#### 14.1 Render Kalitesi
- [ ] Anti-aliasing aktif (SmoothingMode.AntiAlias)
- [ ] Çizgiler düzgün
- [ ] Renkler doğru
- [ ] Metin okunaklı

#### 14.2 Metin Hizalama
- [ ] Metin objede ortada
- [ ] Metin taşmıyor (normal durumda)
- [ ] Font boyutu doğru uygulanıyor

#### 14.3 Selection Border
- [ ] Mavi kesik çizgi border görünüyor
- [ ] Border objeyi tamamen çevreliyor
- [ ] Border kalınlığı 2 piksel

#### 14.4 Resize Handles
- [ ] 4 köşede mavi kare handle'lar var
- [ ] Handle boyutu 8x8 piksel
- [ ] Handle'lar obje köşelerine hizalı

### 15. Performance Testleri
- [ ] 10 obje ile hızlı çalışıyor
- [ ] 50 obje ile performans kabul edilebilir
- [ ] Sürükleme smooth (akıcı)
- [ ] Resize smooth (akıcı)
- [ ] Paint işlemi hızlı

## Otomatik Test Önerileri

### Unit Test'ler
```csharp
[TestClass]
public class SalonServiceTests
{
    [TestMethod]
    public void CreateTable_ShouldReturnTable()
    {
        // Arrange, Act, Assert
    }
    
    [TestMethod]
    public void MoveObject_ShouldUpdatePosition()
    {
        // Arrange, Act, Assert
    }
    
    [TestMethod]
    public void ResizeObject_ShouldUpdateDimensions()
    {
        // Arrange, Act, Assert
    }
}

[TestClass]
public class RepositoryTests
{
    [TestMethod]
    public void AddSalon_ShouldSaveToDatabase()
    {
        // Arrange, Act, Assert
    }
    
    [TestMethod]
    public void GetSalonWithObjects_ShouldIncludeObjects()
    {
        // Arrange, Act, Assert
    }
}

[TestClass]
public class DesignPropertiesTests
{
    [TestMethod]
    public void FromSalonObject_ShouldMapCorrectly()
    {
        // Arrange, Act, Assert
    }
    
    [TestMethod]
    public void ApplyToSalonObject_ShouldUpdateObject()
    {
        // Arrange, Act, Assert
    }
}
```

### Integration Test'ler
```csharp
[TestClass]
public class DatabaseIntegrationTests
{
    [TestMethod]
    public void EndToEnd_CreateAndRetrieveSalon()
    {
        // Arrange, Act, Assert
    }
    
    [TestMethod]
    public void CascadeDelete_ShouldDeleteObjects()
    {
        // Arrange, Act, Assert
    }
}
```

## Bilinen Sınırlamalar

1. **Canvas Sınırları**: Objeler canvas dışına taşınabilir (kaydırma yoksa görünmez)
2. **Çoklu Seçim**: Şu anda desteklenmiyor
3. **Undo/Redo**: Şu anda desteklenmiyor
4. **Zoom**: Şu anda desteklenmiyor
5. **Grid Snap**: Şu anda desteklenmiyor
6. **Collision Detection**: Objeler üst üste binebilir

## Geliştirme Önerileri

1. Undo/Redo mekanizması ekle
2. Zoom in/out özelliği ekle
3. Grid snap özelliği ekle
4. Çoklu seçim desteği
5. Keyboard shortcuts (Delete, Ctrl+C, Ctrl+V)
6. Obje gruplandırma
7. Canvas kaydırma (scrolling)
8. PDF/Image export
9. Print preview
10. Template sistemi
