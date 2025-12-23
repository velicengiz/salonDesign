# Salon Design - Mimari ve Özellikler

## Sistem Mimarisi

```
┌─────────────────────────────────────────────────────────────┐
│                      WinForms UI Layer                       │
│  ┌────────────────────────────────────────────────────────┐ │
│  │           SalonDesignForm.cs                           │ │
│  │  - Canvas Panel (Çizim Alanı)                         │ │
│  │  - Property Panel (Özellik Düzenleme)                 │ │
│  │  - Toolbar (Obje Ekleme Butonları)                    │ │
│  │  - Drag & Drop İşlemleri                              │ │
│  │  - Resize İşlemleri                                   │ │
│  └────────────────────────────────────────────────────────┘ │
└─────────────────────────────────────────────────────────────┘
                            ↓
┌─────────────────────────────────────────────────────────────┐
│                      Services Layer                          │
│  ┌──────────────────┐  ┌──────────────────┐  ┌───────────┐ │
│  │ SalonDesign      │  │ DesignProperty   │  │  Salon    │ │
│  │ Service.cs       │  │ Service.cs       │  │ Repository│ │
│  │ - CreateTable    │  │ - UpdateColor    │  │ .cs       │ │
│  │ - CreateWall     │  │ - UpdateText     │  │ - CRUD    │ │
│  │ - MoveObject     │  │ - UpdateFont     │  │ - Query   │ │
│  │ - ResizeObject   │  │ - GetColor       │  │           │ │
│  └──────────────────┘  └──────────────────┘  └───────────┘ │
└─────────────────────────────────────────────────────────────┘
                            ↓
┌─────────────────────────────────────────────────────────────┐
│                      Data Layer                              │
│  ┌────────────────────────────────────────────────────────┐ │
│  │         SalonDesignContext.cs (EF DbContext)           │ │
│  │  - DbSet<Salon>                                        │ │
│  │  - DbSet<SalonObject>                                  │ │
│  │  - Entity Mappings                                     │ │
│  └────────────────────────────────────────────────────────┘ │
└─────────────────────────────────────────────────────────────┘
                            ↓
┌─────────────────────────────────────────────────────────────┐
│                    Database (SQL Server)                     │
│  ┌──────────────────┐          ┌──────────────────────────┐ │
│  │   sd_salon       │          │      sd_table            │ │
│  │  - id            │          │  - id                    │ │
│  │  - name          │ 1      * │  - salon_id (FK)         │ │
│  │  - description   │──────────│  - object_type           │ │
│  │  - width         │          │  - shape_type            │ │
│  │  - height        │          │  - name                  │ │
│  │  - created_date  │          │  - title                 │ │
│  │  - updated_date  │          │  - table_number          │ │
│  └──────────────────┘          │  - position_x            │ │
│                                 │  - position_y            │ │
│                                 │  - width                 │ │
│                                 │  - height                │ │
│                                 │  - color                 │ │
│                                 │  - text                  │ │
│                                 │  - font_family           │ │
│                                 │  - font_size             │ │
│                                 │  - created_date          │ │
│                                 │  - updated_date          │ │
│                                 └──────────────────────────┘ │
└─────────────────────────────────────────────────────────────┘
```

## Model Yapısı

```
┌─────────────────────────────────────────────────────────────┐
│                      Models Layer                            │
│  ┌──────────────────┐  ┌──────────────────┐  ┌───────────┐ │
│  │   Salon.cs       │  │ SalonObject.cs   │  │  Design   │ │
│  │  - Id            │  │  - Id            │  │ Properties│ │
│  │  - Name          │  │  - SalonId       │  │ .cs       │ │
│  │  - Description   │  │  - ObjectType    │  │ - Color   │ │
│  │  - Width         │  │  - ShapeType     │  │ - Text    │ │
│  │  - Height        │  │  - Name          │  │ - Title   │ │
│  │  - Objects[]     │  │  - Title         │  │ - Font... │ │
│  └──────────────────┘  │  - TableNumber   │  │ - Shape   │ │
│                         │  - PositionX/Y   │  │ - Width   │ │
│                         │  - Width/Height  │  │ - Height  │ │
│                         │  - Color         │  └───────────┘ │
│                         │  - Text          │                │
│                         │  - Font...       │                │
│                         └──────────────────┘                │
└─────────────────────────────────────────────────────────────┘
```

## Enums

```
┌─────────────────────────────────────────────────────────────┐
│                      Enums Layer                             │
│  ┌──────────────────┐          ┌──────────────────────────┐ │
│  │  ObjectType      │          │     ShapeType            │ │
│  │  - Table = 1     │          │  - Circle = 1            │ │
│  │  - Wall = 2      │          │  - Square = 2            │ │
│  │  - Decoration=3  │          │  - Rectangle = 3         │ │
│  └──────────────────┘          └──────────────────────────┘ │
└─────────────────────────────────────────────────────────────┘
```

## Özellik Detayları

### 1. Masa Yönetimi
- **Kare Masa**: ShapeType.Square ile oluşturulur
- **Yuvarlak Masa**: ShapeType.Circle ile oluşturulur
- **Masa Numarası**: TableNumber property'si ile atanır
- **Masa Adı**: Name property'si ile atanır

### 2. Duvar ve Objeler
- **Duvar**: ObjectType.Wall, kareli pattern ile gösterilir
- **Dekorasyon**: ObjectType.Decoration olarak kaydedilir
- **Başlık**: Title property'si ile atanır
- **Şekil**: ShapeType enum ile belirlenir

### 3. Tasarımsal Özellikler (DesignProperties)
```csharp
public class DesignProperties
{
    Color Color;           // Obje rengi
    string Text;           // Obje üzerindeki metin
    string Title;          // Obje başlığı
    string FontFamily;     // Font ailesi (Arial, Verdana, vb.)
    float FontSize;        // Font boyutu
    ShapeType ShapeType;   // Şekil tipi
    int Width;             // Genişlik
    int Height;            // Yükseklik
}
```

### 4. Etkileşim Özellikleri

#### Drag & Drop (Taşıma)
```
MouseDown → isDragging = true
MouseMove → PositionX/Y güncellenir
MouseUp   → Veritabanına kaydedilir
```

#### Resize (Boyutlandırma)
```
4 köşede handle çizilir
Handle'a tıklanınca → isResizing = true
MouseMove → Width/Height güncellenir
MouseUp   → Veritabanına kaydedilir
```

#### Obje Seçimi
```
Canvas'a tıklama
  → GetObjectAtPoint() ile obje bulunur
  → Seçim border'ı çizilir (mavi kesik çizgi)
  → Property paneli güncellenir
  → Resize handle'ları gösterilir
```

### 5. Render İşlemleri

#### Kare/Dikdörtgen Objeler
```csharp
g.FillRectangle(brush, rect);
g.DrawRectangle(pen, rect);
```

#### Yuvarlak Objeler
```csharp
g.FillEllipse(brush, rect);
g.DrawEllipse(pen, rect);
```

#### Duvar (Kareli Pattern)
```csharp
DrawCheckerPattern(g, rect, color)
  → 10x10 piksel karelerle doldurulur
  → İki renk arasında geçiş yapılır
  → Satranç tahtası görünümü
```

#### Metin Render
```csharp
Merkeze hizalanır
Font ve boyut uygulanır
Text/Title/TableNumber gösterilir
```

## Veritabanı İşlemleri

### Repository Pattern
```
SalonRepository
  ├── GetAllSalons()
  ├── GetSalonById(id)
  ├── GetSalonWithObjects(id)
  ├── AddSalon(salon)
  ├── UpdateSalon(salon)
  ├── DeleteSalon(id)
  ├── GetObjectsBySalonId(salonId)
  ├── GetObjectById(id)
  ├── AddObject(obj)
  ├── UpdateObject(obj)
  └── DeleteObject(id)
```

### Service Layer İşlemleri
```
SalonDesignService
  ├── CreateSalon()
  ├── CreateTable()          → ObjectType.Table
  ├── CreateWall()           → ObjectType.Wall
  ├── CreateDecoration()     → ObjectType.Decoration
  ├── MoveObject()           → PositionX/Y güncelle
  ├── ResizeObject()         → Width/Height güncelle
  ├── UpdateObjectDesignProperties()
  ├── GetSalonObjects()
  ├── DeleteObject()
  └── GetAllSalons()
```

## UI Bileşenleri

### Canvas Panel
- **Boyut**: Form'un tamamını kaplar (DockStyle.Fill)
- **Arka Plan**: Beyaz
- **Event'ler**: Paint, MouseDown, MouseMove, MouseUp
- **Çizim**: OnPaint event'inde tüm objeler çizilir

### Property Panel
- **Boyut**: 300px genişlik (DockStyle.Right)
- **İçerik**: 
  - Ad (TextBox)
  - Başlık (TextBox)
  - Metin (TextBox)
  - Masa Numarası (NumericUpDown)
  - Şekil (ComboBox)
  - Renk (Button + ColorDialog)
  - Font Ailesi (ComboBox)
  - Font Boyutu (NumericUpDown)

### Toolbar
- **Butonlar**:
  - Kare Masa
  - Yuvarlak Masa
  - Duvar Ekle
  - Dekorasyon
  - Kaydet
  - Sil

## Kullanım Senaryoları

### Senaryo 1: Yeni Masa Ekleme
1. "Kare Masa" veya "Yuvarlak Masa" butonuna tıkla
2. Sistem varsayılan pozisyonda masa oluşturur
3. Masa veritabanına kaydedilir
4. Canvas yeniden çizilir

### Senaryo 2: Masa Taşıma
1. Masaya tıkla (seçim yapılır)
2. Fare ile sürükle
3. MouseMove'da pozisyon güncellenir
4. Bırakınca veritabanına kaydedilir

### Senaryo 3: Masa Boyutlandırma
1. Masayı seç
2. Köşedeki mavi handle'a tıkla
3. Sürükle
4. Bırakınca veritabanına kaydedilir

### Senaryo 4: Özellik Düzenleme
1. Objeyi seç
2. Sağ panelde özellikler gösterilir
3. Değişiklikleri yap (renk, metin, font vb.)
4. "Kaydet" butonuna tıkla
5. Veritabanı güncellenir

### Senaryo 5: Duvar Ekleme
1. "Duvar Ekle" butonuna tıkla
2. Varsayılan pozisyonda duvar oluşturulur
3. Kareli pattern ile gösterilir
4. Taşınabilir ve boyutlandırılabilir

## Teknik Özellikler

### C# 7.3 Uyumluluğu
- `LangVersion>7.3</LangVersion>` ayarı yapıldı
- Tüm özellikler C# 7.3 ile uyumlu
- Pattern matching, out variables vb. kullanılmadı

### Entity Framework 6.4.4
- Code-First yaklaşımı
- Fluent API ile ilişkiler tanımlandı
- Cascade delete aktif
- Migration desteği

### Type-Safety
- Enum kullanımı (ObjectType, ShapeType)
- Strongly-typed models
- Repository pattern ile veri erişimi

### Performance
- Paint event'inde sadece görünen objeler çizilir
- Lazy loading kullanılabilir
- Transaction desteği

## Genişletme Noktaları

### 1. Ek Özellikler
- Undo/Redo functionality
- Zoom in/out
- Grid snap
- Alignment tools
- Copy/paste

### 2. Export
- PDF export
- Image export (PNG, JPG)
- Print preview

### 3. Multiple Salons
- Salon seçim listesi
- Salon kopyalama
- Salon şablonları

### 4. Advanced Objects
- Resim ekleme
- Custom şekiller
- Gruplandırma
- Layer sistemi

## Sonuç

Bu proje, restaurant salon tasarımı için tam özellikli bir WinForms uygulamasıdır. Tüm gereksinimler karşılanmış, nesne yönelimli tasarım prensipleri uygulanmış ve genişletilebilir bir mimari oluşturulmuştur.
