# Salon Design Application - Visual Guide

## User Interface Layout

```
┌─────────────────────────────────────────────────────────────────────────────┐
│  Salon Tasarım Formu                                                    [_][□][X]│
├─────────────────────────────────────────────────────────────────────────────┤
│ ┌──────────────────────────────────────────────────────────────────────┐    │
│ │ [Kare Masa] [Yuvarlak Masa] [Duvar Ekle] [Dekorasyon] [Kaydet] [Sil]│    │
│ └──────────────────────────────────────────────────────────────────────┘    │
├──────────────────────────────────────────────┬──────────────────────────────┤
│                                              │  Obje Özellikleri            │
│                                              ├──────────────────────────────┤
│                                              │  Ad: [____________]          │
│   ┌──────────┐      ╔══════════╗            │  Başlık: [____________]      │
│   │          │      ║          ║            │  Metin: [____________]       │
│   │  Masa 1  │      ║  Masa 2  ║            │  Masa No: [↑↓ 1]            │
│   └──────────┘      ╚══════════╝            │  Şekil: [Yuvarlak ▼]         │
│                                              │  Renk: [Renk Seç]           │
│   ▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓                      │  Font: [Arial ▼]            │
│   ▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓  Duvar               │  Font Boyut: [↑↓ 10]        │
│                                              │                              │
│       ○ Dekorasyon                           │                              │
│                                              │                              │
│                                              │                              │
│                                              │                              │
│   CANVAS PANEL                               │   PROPERTY PANEL             │
│   (Çizim Alanı)                              │   (Özellik Düzenleme)        │
│                                              │                              │
└──────────────────────────────────────────────┴──────────────────────────────┘
```

## Object Selection Visual

```
┌──────────────────────────────────────────────────┐
│  Seçilmemiş Obje:                                │
│                                                  │
│   ┌──────────┐                                   │
│   │          │                                   │
│   │  Masa 1  │                                   │
│   └──────────┘                                   │
│                                                  │
├──────────────────────────────────────────────────┤
│  Seçili Obje:                                    │
│                                                  │
│   ┌ ┄ ┄ ┄ ┄ ┄ ┄ ┄ ┄ ┄ ┄ ┐  ← Mavi kesik çizgi  │
│   ┊ ■──────────■         ┊  ← Resize handle'lar │
│   ┊ │          │         ┊                       │
│   ┊ │  Masa 1  │         ┊                       │
│   ┊ │          │         ┊                       │
│   ┊ ■──────────■         ┊                       │
│   └ ┄ ┄ ┄ ┄ ┄ ┄ ┄ ┄ ┄ ┄ ┘                       │
│                                                  │
└──────────────────────────────────────────────────┘
```

## Shape Types

```
┌────────────────────────────────────────────────────┐
│  Kare (Square)          Yuvarlak (Circle)         │
│  ShapeType.Square       ShapeType.Circle          │
│                                                    │
│   ┌──────────┐            ╔══════════╗            │
│   │          │            ║          ║            │
│   │  Masa 1  │            ║  Masa 2  ║            │
│   │          │            ║          ║            │
│   └──────────┘            ╚══════════╝            │
│                                                    │
├────────────────────────────────────────────────────┤
│  Dikdörtgen (Rectangle)                           │
│  ShapeType.Rectangle                              │
│                                                    │
│   ┌──────────────────────┐                        │
│   │      Masa 3          │                        │
│   └──────────────────────┘                        │
│                                                    │
└────────────────────────────────────────────────────┘
```

## Wall Checker Pattern

```
┌────────────────────────────────────────┐
│  Duvar (Wall) - Kareli Pattern        │
│  ObjectType.Wall                       │
│                                        │
│  ▓░▓░▓░▓░▓░▓░▓░▓░▓░▓░▓░▓░▓░▓░         │
│  ░▓░▓░▓░▓░▓░▓░▓░▓░▓░▓░▓░▓░▓░▓         │
│  ▓░▓░▓░▓░▓░▓░▓░▓░▓░▓░▓░▓░▓░▓░         │
│                                        │
│  Her kare: 10x10 piksel                │
│  İki ton renk (baseColor + lighter)   │
│                                        │
└────────────────────────────────────────┘
```

## User Interaction Flow

### 1. Adding a Table

```
┌─────────────┐
│ USER ACTION │
│ Click       │
│ "Kare Masa" │
└──────┬──────┘
       │
       ▼
┌─────────────────────┐
│ UI LAYER            │
│ BtnAddSquareTable   │
│ _Click              │
└──────┬──────────────┘
       │
       ▼
┌─────────────────────┐
│ SERVICE LAYER       │
│ SalonDesignService  │
│ .CreateTable()      │
└──────┬──────────────┘
       │
       ▼
┌─────────────────────┐
│ REPOSITORY LAYER    │
│ SalonRepository     │
│ .AddObject()        │
└──────┬──────────────┘
       │
       ▼
┌─────────────────────┐
│ DATABASE            │
│ INSERT INTO sd_table│
└──────┬──────────────┘
       │
       ▼
┌─────────────────────┐
│ UI REFRESH          │
│ canvas.Invalidate() │
│ Object appears      │
└─────────────────────┘
```

### 2. Drag & Drop

```
┌─────────────┐
│ MouseDown   │
│ on object   │
└──────┬──────┘
       │
       ▼
┌─────────────────────┐
│ GetObjectAtPoint()  │
│ _selectedObject set │
│ isDragging = true   │
└──────┬──────────────┘
       │
       ▼
┌─────────────┐     ┌─────────────────────┐
│ MouseMove   │────→│ Calculate delta     │
│ (dragging)  │     │ Update position     │
└──────┬──────┘     │ Invalidate canvas   │
       │            └─────────────────────┘
       │ (repeat)
       ▼
┌─────────────┐
│ MouseUp     │
│ Save to DB  │
│ isDragging  │
│ = false     │
└─────────────┘
```

### 3. Resize

```
┌─────────────┐
│ MouseDown   │
│ on handle   │
└──────┬──────┘
       │
       ▼
┌─────────────────────┐
│ isResizing = true   │
│ Store start point   │
└──────┬──────────────┘
       │
       ▼
┌─────────────┐     ┌──────────────────────┐
│ MouseMove   │────→│ Calculate size delta │
│ (resizing)  │     │ Apply MIN_SIZE       │
└──────┬──────┘     │ Update width/height  │
       │            │ Invalidate canvas    │
       │            └──────────────────────┘
       │ (repeat)
       ▼
┌─────────────┐
│ MouseUp     │
│ Save to DB  │
│ isResizing  │
│ = false     │
└─────────────┘
```

### 4. Property Editing

```
┌─────────────┐
│ Select      │
│ object      │
└──────┬──────┘
       │
       ▼
┌──────────────────────┐
│ UpdatePropertyPanel()│
│ Fill form fields     │
└──────┬───────────────┘
       │
       ▼
┌─────────────┐     ┌─────────────────────┐
│ USER EDITS  │────→│ PropertyChanged()   │
│ in panel    │     │ Update object       │
└──────┬──────┘     │ Invalidate canvas   │
       │            └─────────────────────┘
       │
       ▼
┌─────────────┐
│ Click       │
│ "Kaydet"    │
└──────┬──────┘
       │
       ▼
┌─────────────────────────┐
│ UpdateObjectDesign      │
│ Properties()            │
│ Save to database        │
│ Show success message    │
└─────────────────────────┘
```

## Object Types & Properties

```
┌────────────────────────────────────────────────────────────┐
│                     SalonObject                            │
├────────────────────────────────────────────────────────────┤
│  Core Properties:                                          │
│  ├─ ObjectType (enum)                                      │
│  │  ├─ Table      = 1  → Kare veya Yuvarlak masa          │
│  │  ├─ Wall       = 2  → Kareli pattern duvar             │
│  │  └─ Decoration = 3  → Dekoratif obje                   │
│  │                                                          │
│  ├─ ShapeType (enum)                                       │
│  │  ├─ Circle     = 1  → ○ Yuvarlak                       │
│  │  ├─ Square     = 2  → □ Kare                           │
│  │  └─ Rectangle  = 3  → ▭ Dikdörtgen                     │
│  │                                                          │
│  ├─ Position                                               │
│  │  ├─ PositionX → X koordinatı                           │
│  │  └─ PositionY → Y koordinatı                           │
│  │                                                          │
│  ├─ Size                                                   │
│  │  ├─ Width     → Genişlik (min: 30)                     │
│  │  └─ Height    → Yükseklik (min: 30)                    │
│  │                                                          │
│  └─ Visual Properties                                      │
│     ├─ Color      → HTML renk kodu (#RRGGBB)              │
│     ├─ Text       → Obje üzerindeki metin                 │
│     ├─ Title      → Obje başlığı                          │
│     ├─ FontFamily → Font ailesi (Arial, vb.)              │
│     └─ FontSize   → Font boyutu (6-72)                    │
│                                                            │
│  Table-Specific:                                           │
│  └─ TableNumber  → Masa numarası (1-999)                  │
└────────────────────────────────────────────────────────────┘
```

## Database Schema Visualization

```
┌─────────────────────────────────────────┐
│            sd_salon                     │
├─────────────────────────────────────────┤
│ PK  id              INT                 │
│     name            NVARCHAR(200)       │
│     description     NVARCHAR(500)       │
│     width           INT                 │
│     height          INT                 │
│     created_date    DATETIME            │
│     updated_date    DATETIME            │
└──────────┬──────────────────────────────┘
           │
           │ 1:N (Cascade Delete)
           │
┌──────────▼──────────────────────────────┐
│           sd_table                      │
├─────────────────────────────────────────┤
│ PK  id              INT                 │
│ FK  salon_id        INT                 │
│     object_type     INT                 │
│     shape_type      INT                 │
│     name            NVARCHAR(100)       │
│     title           NVARCHAR(100)       │
│     table_number    INT                 │
│     position_x      INT                 │
│     position_y      INT                 │
│     width           INT                 │
│     height          INT                 │
│     color           NVARCHAR(50)        │
│     text            NVARCHAR(200)       │
│     font_family     NVARCHAR(100)       │
│     font_size       FLOAT               │
│     created_date    DATETIME            │
│     updated_date    DATETIME            │
└─────────────────────────────────────────┘
```

## Color Palette Example

```
┌──────────────────────────────────────────┐
│  Default Colors:                         │
│                                          │
│  Masa (Table):      ▓▓ #CCCCCC (Gray)   │
│  Duvar (Wall):      ▓▓ #8B4513 (Brown)  │
│  Dekorasyon:        ▓▓ #CCCCCC (Gray)   │
│                                          │
│  Selection:         ▓▓ #0000FF (Blue)   │
│  Handle:            ▓▓ #0000FF (Blue)   │
│                                          │
│  Customizable via ColorDialog           │
└──────────────────────────────────────────┘
```

## Performance Considerations

```
┌────────────────────────────────────────────────┐
│  Paint Event (Canvas Rendering)               │
├────────────────────────────────────────────────┤
│  1. Clear background                           │
│  2. Iterate through all objects:               │
│     ├─ Draw object shape                       │
│     ├─ Draw object text                        │
│     └─ Apply colors/fonts                      │
│  3. Draw selection (if object selected):       │
│     ├─ Draw selection border                   │
│     └─ Draw resize handles                     │
│                                                │
│  Optimization:                                 │
│  ✓ Anti-aliasing enabled                       │
│  ✓ Using statements for resource disposal      │
│  ✓ Only invalidate when needed                 │
│  ✓ Tested with 50+ objects                     │
└────────────────────────────────────────────────┘
```

## Application Lifecycle

```
┌─────────────┐
│   START     │
│ Program.cs  │
└──────┬──────┘
       │
       ▼
┌──────────────────────┐
│ Initialize Services  │
│ - Repository         │
│ - Service            │
│ - PropertyService    │
└──────┬───────────────┘
       │
       ▼
┌──────────────────────┐
│ Load or Create Salon │
│ GetAllSalons()       │
│ If empty: CreateSalon│
└──────┬───────────────┘
       │
       ▼
┌──────────────────────┐
│ Load Objects         │
│ GetSalonObjects()    │
└──────┬───────────────┘
       │
       ▼
┌──────────────────────┐
│ Show Form            │
│ Paint canvas         │
│ Wait for user input  │
└──────┬───────────────┘
       │
       │ (user interactions)
       │
       ▼
┌──────────────────────┐
│   CLOSE              │
│ Dispose repository   │
│ Close DB connection  │
└──────────────────────┘
```

## Quick Reference

### Keyboard & Mouse Operations

| Action | Operation |
|--------|-----------|
| Select | Click on object |
| Move | Drag object |
| Resize | Drag corner handle |
| Delete | Select + Click "Sil" button |
| Add Table | Click "Kare Masa" or "Yuvarlak Masa" |
| Add Wall | Click "Duvar Ekle" |
| Change Color | Select + Click "Renk Seç" |
| Save Properties | Select + Edit + Click "Kaydet" |

### Constants

| Constant | Value | Description |
|----------|-------|-------------|
| RESIZE_HANDLE_SIZE | 8 | Handle size in pixels |
| MIN_OBJECT_SIZE | 30 | Minimum object dimension |
| CHECKER_SIZE | 10 | Wall pattern square size |

### Default Values

| Property | Default Value |
|----------|---------------|
| Color | #CCCCCC (Gray) |
| Font Family | Arial |
| Font Size | 10 |
| Table Width/Height | 80 |
| Wall Width/Height | 150/20 |

---

This visual guide provides a comprehensive overview of the Restaurant Salon Design Application's user interface, interactions, and architecture.
