using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
using SalonDesign.Enums;
using SalonDesign.Models;
using SalonDesign.Services;
using SalonDesign.Utilities;

namespace SalonDesign.Forms
{
    public partial class SalonDesignForm : Form
    {
        private SalonRepository _repository;
        private SalonDesignService _service;
        private DesignPropertyService _propertyService;
        private Salon _currentSalon;
        private List<SalonObject> _objects;
        private SalonObject _selectedObject;
        private Point _dragStartPoint;
        private bool _isDragging;
        private bool _isResizing;
        private const int RESIZE_HANDLE_SIZE = 8;
        private const int MIN_OBJECT_SIZE = 30;

        // Performance optimization components
        private RenderingOptimizer _renderingOptimizer;
        private InvalidationManager _invalidationManager;
        private SalonObjectRenderer _objectRenderer;
        private PerformanceMonitor _performanceMonitor;
        private Point _lastDragPosition;
        private Size _lastResizeSize;

        private Panel canvasPanel;
        private Panel propertyPanel;
        private ToolStrip toolStrip;
        private Button btnAddSquareTable;
        private Button btnAddRoundTable;
        private Button btnAddWall;
        private Button btnAddDecoration;
        private Button btnSave;
        private Button btnDelete;
        
        private TextBox txtName;
        private TextBox txtTitle;
        private TextBox txtText;
        private NumericUpDown numTableNumber;
        private ComboBox cmbShape;
        private Button btnColorPicker;
        private ComboBox cmbFontFamily;
        private NumericUpDown numFontSize;
        private Label lblProperties;

        public SalonDesignForm()
        {
            InitializeComponent();
            InitializeServices();
            LoadOrCreateSalon();
        }

        private void InitializeComponent()
        {
            this.Text = "Salon Tasarım Formu";
            this.Size = new Size(1200, 800);
            this.StartPosition = FormStartPosition.CenterScreen;

            // Enable double buffering for flicker-free rendering
            this.DoubleBuffered = true;
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer | 
                          ControlStyles.AllPaintingInWmPaint | 
                          ControlStyles.UserPaint, true);
            this.UpdateStyles();

            // Canvas Panel
            canvasPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };
            // Enable double buffering for canvas panel
            typeof(Panel).InvokeMember("DoubleBuffered",
                System.Reflection.BindingFlags.SetProperty | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic,
                null, canvasPanel, new object[] { true });
            
            canvasPanel.Paint += CanvasPanel_Paint;
            canvasPanel.MouseDown += CanvasPanel_MouseDown;
            canvasPanel.MouseMove += CanvasPanel_MouseMove;
            canvasPanel.MouseUp += CanvasPanel_MouseUp;
            canvasPanel.Resize += CanvasPanel_Resize;

            // Property Panel
            propertyPanel = new Panel
            {
                Dock = DockStyle.Right,
                Width = 300,
                BackColor = Color.WhiteSmoke,
                Padding = new Padding(10)
            };

            // ToolStrip
            toolStrip = new ToolStrip
            {
                Dock = DockStyle.Top
            };

            btnAddSquareTable = new Button
            {
                Text = "Kare Masa",
                Width = 100,
                Height = 30
            };
            btnAddSquareTable.Click += BtnAddSquareTable_Click;

            btnAddRoundTable = new Button
            {
                Text = "Yuvarlak Masa",
                Width = 120,
                Height = 30,
                Left = 110
            };
            btnAddRoundTable.Click += BtnAddRoundTable_Click;

            btnAddWall = new Button
            {
                Text = "Duvar Ekle",
                Width = 100,
                Height = 30,
                Left = 240
            };
            btnAddWall.Click += BtnAddWall_Click;

            btnAddDecoration = new Button
            {
                Text = "Dekorasyon",
                Width = 100,
                Height = 30,
                Left = 350
            };
            btnAddDecoration.Click += BtnAddDecoration_Click;

            btnSave = new Button
            {
                Text = "Kaydet",
                Width = 80,
                Height = 30,
                Left = 460
            };
            btnSave.Click += BtnSave_Click;

            btnDelete = new Button
            {
                Text = "Sil",
                Width = 80,
                Height = 30,
                Left = 550
            };
            btnDelete.Click += BtnDelete_Click;

            var buttonPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 50,
                Padding = new Padding(10)
            };
            buttonPanel.Controls.AddRange(new Control[] { 
                btnAddSquareTable, btnAddRoundTable, btnAddWall, 
                btnAddDecoration, btnSave, btnDelete 
            });

            // Property controls
            lblProperties = new Label
            {
                Text = "Obje Özellikleri",
                Font = new Font("Arial", 12, FontStyle.Bold),
                AutoSize = true,
                Top = 10,
                Left = 10
            };

            var lblName = new Label { Text = "Ad:", Top = 50, Left = 10, Width = 80 };
            txtName = new TextBox { Top = 50, Left = 100, Width = 180 };
            txtName.TextChanged += PropertyChanged;

            var lblTitle = new Label { Text = "Başlık:", Top = 80, Left = 10, Width = 80 };
            txtTitle = new TextBox { Top = 80, Left = 100, Width = 180 };
            txtTitle.TextChanged += PropertyChanged;

            var lblText = new Label { Text = "Metin:", Top = 110, Left = 10, Width = 80 };
            txtText = new TextBox { Top = 110, Left = 100, Width = 180 };
            txtText.TextChanged += PropertyChanged;

            var lblTableNumber = new Label { Text = "Masa No:", Top = 140, Left = 10, Width = 80 };
            numTableNumber = new NumericUpDown { Top = 140, Left = 100, Width = 180, Minimum = 1, Maximum = 999 };
            numTableNumber.ValueChanged += PropertyChanged;

            var lblShape = new Label { Text = "Şekil:", Top = 170, Left = 10, Width = 80 };
            cmbShape = new ComboBox { Top = 170, Left = 100, Width = 180, DropDownStyle = ComboBoxStyle.DropDownList };
            cmbShape.Items.AddRange(new object[] { "Yuvarlak", "Kare", "Dikdörtgen" });
            cmbShape.SelectedIndexChanged += PropertyChanged;

            var lblColor = new Label { Text = "Renk:", Top = 200, Left = 10, Width = 80 };
            btnColorPicker = new Button { Top = 200, Left = 100, Width = 180, Height = 25, Text = "Renk Seç" };
            btnColorPicker.Click += BtnColorPicker_Click;

            var lblFont = new Label { Text = "Font:", Top = 230, Left = 10, Width = 80 };
            cmbFontFamily = new ComboBox { Top = 230, Left = 100, Width = 180, DropDownStyle = ComboBoxStyle.DropDownList };
            cmbFontFamily.Items.AddRange(new object[] { "Arial", "Times New Roman", "Verdana", "Calibri" });
            cmbFontFamily.SelectedIndexChanged += PropertyChanged;

            var lblFontSize = new Label { Text = "Font Boyut:", Top = 260, Left = 10, Width = 80 };
            numFontSize = new NumericUpDown { Top = 260, Left = 100, Width = 180, Minimum = 6, Maximum = 72, Value = 10 };
            numFontSize.ValueChanged += PropertyChanged;

            propertyPanel.Controls.AddRange(new Control[] {
                lblProperties, lblName, txtName, lblTitle, txtTitle, lblText, txtText,
                lblTableNumber, numTableNumber, lblShape, cmbShape, lblColor, btnColorPicker,
                lblFont, cmbFontFamily, lblFontSize, numFontSize
            });

            this.Controls.Add(canvasPanel);
            this.Controls.Add(propertyPanel);
            this.Controls.Add(buttonPanel);
        }

        private void InitializeServices()
        {
            _repository = new SalonRepository();
            _service = new SalonDesignService(_repository);
            _propertyService = new DesignPropertyService();
            _objects = new List<SalonObject>();
            
            // Initialize performance optimization components
            Rectangle initialViewport = new Rectangle(0, 0, 1200, 800);
            _renderingOptimizer = new RenderingOptimizer(initialViewport);
            _invalidationManager = new InvalidationManager(initialViewport);
            _objectRenderer = new SalonObjectRenderer(_propertyService);
            _performanceMonitor = new PerformanceMonitor(60);
        }

        private void LoadOrCreateSalon()
        {
            var salons = _service.GetAllSalons();
            if (salons.Count > 0)
            {
                _currentSalon = salons[0];
            }
            else
            {
                _currentSalon = _service.CreateSalon("Ana Salon", "Restaurant ana salonu", 1000, 700);
            }

            LoadObjects();
        }

        private void LoadObjects()
        {
            _objects = _service.GetSalonObjects(_currentSalon.Id);
            
            // Rebuild spatial index with all objects
            _renderingOptimizer.BuildSpatialIndex(_objects);
            
            canvasPanel.Invalidate();
        }

        private void CanvasPanel_Paint(object sender, PaintEventArgs e)
        {
            _performanceMonitor.BeginFrame();
            _performanceMonitor.BeginRender();

            Graphics g = e.Graphics;
            _objectRenderer.OptimizeGraphics(g);

            // Use viewport culling for better performance with many objects
            List<SalonObject> visibleObjects;
            if (_renderingOptimizer.ShouldUseViewportCulling)
            {
                // Get only visible objects using QuadTree
                Rectangle viewport = canvasPanel.ClientRectangle;
                visibleObjects = _renderingOptimizer.GetVisibleObjects(viewport);
            }
            else
            {
                // For small object counts, render all
                visibleObjects = _objects;
            }

            // Draw all visible objects
            foreach (var obj in visibleObjects)
            {
                _objectRenderer.DrawObject(g, obj);
            }

            // Draw selection and resize handles for selected object
            if (_selectedObject != null)
            {
                _objectRenderer.DrawSelectionBorder(g, _selectedObject);
                _objectRenderer.DrawResizeHandles(g, _selectedObject, RESIZE_HANDLE_SIZE);
            }

            _performanceMonitor.EndRender();
            
            // Optional: Draw performance stats in debug mode
            #if DEBUG
            DrawPerformanceStats(g);
            #endif
        }

        private void DrawPerformanceStats(Graphics g)
        {
            string stats = _performanceMonitor.GetPerformanceReport() + 
                           $" | Objects: {_objects.Count} | Visible: {_renderingOptimizer.ShouldUseViewportCulling}";
            
            using (Font font = new Font("Arial", 8))
            using (Brush brush = new SolidBrush(Color.Black))
            using (Brush bgBrush = new SolidBrush(Color.FromArgb(200, Color.Yellow)))
            {
                SizeF size = g.MeasureString(stats, font);
                g.FillRectangle(bgBrush, 5, 5, size.Width + 10, size.Height + 5);
                g.DrawString(stats, font, brush, 10, 7);
            }
        }

        private void DrawObject(Graphics g, SalonObject obj)
        {
            // Use the optimized renderer instead
            _objectRenderer.DrawObject(g, obj);
        }

        private void DrawSelectionBorder(Graphics g, SalonObject obj)
        {
            // Use the optimized renderer instead
            _objectRenderer.DrawSelectionBorder(g, obj);
        }

        private void DrawResizeHandles(Graphics g, SalonObject obj)
        {
            // Use the optimized renderer instead
            _objectRenderer.DrawResizeHandles(g, obj, RESIZE_HANDLE_SIZE);
        }

        private Rectangle[] GetResizeHandles(SalonObject obj)
        {
            int x = obj.PositionX;
            int y = obj.PositionY;
            int w = obj.Width;
            int h = obj.Height;
            int hs = RESIZE_HANDLE_SIZE;

            return new Rectangle[]
            {
                new Rectangle(x - hs/2, y - hs/2, hs, hs), // Top-left
                new Rectangle(x + w - hs/2, y - hs/2, hs, hs), // Top-right
                new Rectangle(x - hs/2, y + h - hs/2, hs, hs), // Bottom-left
                new Rectangle(x + w - hs/2, y + h - hs/2, hs, hs), // Bottom-right
            };
        }

        private void CanvasPanel_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;

            _selectedObject = GetObjectAtPoint(e.Location);
            
            if (_selectedObject != null)
            {
                UpdatePropertyPanel();
                
                // Save initial state for selective invalidation
                _lastDragPosition = new Point(_selectedObject.PositionX, _selectedObject.PositionY);
                _lastResizeSize = new Size(_selectedObject.Width, _selectedObject.Height);
                
                Rectangle[] handles = GetResizeHandles(_selectedObject);
                foreach (var handle in handles)
                {
                    if (handle.Contains(e.Location))
                    {
                        _isResizing = true;
                        _dragStartPoint = e.Location;
                        return;
                    }
                }

                _isDragging = true;
                _dragStartPoint = e.Location;
            }
            
            canvasPanel.Invalidate();
        }

        private void CanvasPanel_MouseMove(object sender, MouseEventArgs e)
        {
            if (_selectedObject == null) return;

            if (_isDragging)
            {
                int deltaX = e.X - _dragStartPoint.X;
                int deltaY = e.Y - _dragStartPoint.Y;

                // Save old position for dirty region
                Point oldPosition = new Point(_selectedObject.PositionX, _selectedObject.PositionY);

                _selectedObject.PositionX += deltaX;
                _selectedObject.PositionY += deltaY;

                // Update QuadTree
                _renderingOptimizer.UpdateObject(_selectedObject);

                // Add dirty regions (old and new position)
                _invalidationManager.AddMovedObjectDirtyRegions(_selectedObject, oldPosition, RESIZE_HANDLE_SIZE + 5);

                _dragStartPoint = e.Location;
                
                // Selective invalidation - only redraw affected areas
                InvalidateDirtyRegions();
            }
            else if (_isResizing)
            {
                int deltaX = e.X - _dragStartPoint.X;
                int deltaY = e.Y - _dragStartPoint.Y;

                // Save old size for dirty region
                Size oldSize = new Size(_selectedObject.Width, _selectedObject.Height);

                _selectedObject.Width = Math.Max(MIN_OBJECT_SIZE, _selectedObject.Width + deltaX);
                _selectedObject.Height = Math.Max(MIN_OBJECT_SIZE, _selectedObject.Height + deltaY);

                // Update QuadTree
                _renderingOptimizer.UpdateObject(_selectedObject);

                // Add dirty regions (old and new size)
                _invalidationManager.AddResizedObjectDirtyRegions(_selectedObject, oldSize, RESIZE_HANDLE_SIZE + 5);

                _dragStartPoint = e.Location;
                
                // Selective invalidation - only redraw affected areas
                InvalidateDirtyRegions();
            }
        }

        private void CanvasPanel_MouseUp(object sender, MouseEventArgs e)
        {
            if (_isDragging && _selectedObject != null)
            {
                _service.MoveObject(_selectedObject.Id, _selectedObject.PositionX, _selectedObject.PositionY);
                
                // Update QuadTree with final position
                _renderingOptimizer.UpdateObject(_selectedObject);
            }
            else if (_isResizing && _selectedObject != null)
            {
                _service.ResizeObject(_selectedObject.Id, _selectedObject.Width, _selectedObject.Height);
                
                // Update QuadTree with final size
                _renderingOptimizer.UpdateObject(_selectedObject);
            }

            _isDragging = false;
            _isResizing = false;
        }

        /// <summary>
        /// Canvas resize event handler - update viewport and optimizer
        /// </summary>
        private void CanvasPanel_Resize(object sender, EventArgs e)
        {
            Rectangle newViewport = canvasPanel.ClientRectangle;
            _renderingOptimizer.UpdateViewport(newViewport);
            _invalidationManager.UpdateCanvasBounds(newViewport);
        }

        /// <summary>
        /// Invalidate only dirty regions for better performance
        /// </summary>
        private void InvalidateDirtyRegions()
        {
            if (_invalidationManager.HasDirtyRegions)
            {
                // Optimize dirty regions (merge overlapping areas)
                _invalidationManager.OptimizeDirtyRegions();
                
                // Invalidate each dirty rectangle
                foreach (Rectangle rect in _invalidationManager.GetDirtyRectangles())
                {
                    canvasPanel.Invalidate(rect);
                }
                
                // Clear dirty regions after invalidation
                _invalidationManager.Clear();
            }
        }

        private SalonObject GetObjectAtPoint(Point point)
        {
            for (int i = _objects.Count - 1; i >= 0; i--)
            {
                var obj = _objects[i];
                Rectangle rect = new Rectangle(obj.PositionX, obj.PositionY, obj.Width, obj.Height);
                
                if (obj.ShapeType == ShapeType.Circle)
                {
                    int centerX = rect.X + rect.Width / 2;
                    int centerY = rect.Y + rect.Height / 2;
                    int radiusX = rect.Width / 2;
                    int radiusY = rect.Height / 2;
                    
                    double normalizedX = (double)(point.X - centerX) / radiusX;
                    double normalizedY = (double)(point.Y - centerY) / radiusY;
                    
                    if (normalizedX * normalizedX + normalizedY * normalizedY <= 1)
                        return obj;
                }
                else if (rect.Contains(point))
                {
                    return obj;
                }
            }
            return null;
        }

        private void UpdatePropertyPanel()
        {
            if (_selectedObject == null)
            {
                txtName.Text = "";
                txtTitle.Text = "";
                txtText.Text = "";
                numTableNumber.Value = 1;
                cmbShape.SelectedIndex = -1;
                cmbFontFamily.SelectedIndex = -1;
                numFontSize.Value = 10;
                return;
            }

            txtName.Text = _selectedObject.Name ?? "";
            txtTitle.Text = _selectedObject.Title ?? "";
            txtText.Text = _selectedObject.Text ?? "";
            numTableNumber.Value = _selectedObject.TableNumber ?? 1;
            
            cmbShape.SelectedIndex = (int)_selectedObject.ShapeType - 1;
            
            cmbFontFamily.SelectedItem = _selectedObject.FontFamily ?? "Arial";
            if (cmbFontFamily.SelectedIndex == -1)
                cmbFontFamily.SelectedIndex = 0;
                
            numFontSize.Value = (decimal)(_selectedObject.FontSize > 0 ? _selectedObject.FontSize : 10);
        }

        private void PropertyChanged(object sender, EventArgs e)
        {
            if (_selectedObject == null) return;

            _selectedObject.Name = txtName.Text;
            _selectedObject.Title = txtTitle.Text;
            _selectedObject.Text = txtText.Text;
            _selectedObject.TableNumber = (int)numTableNumber.Value;
            
            if (cmbShape.SelectedIndex >= 0)
                _selectedObject.ShapeType = (ShapeType)(cmbShape.SelectedIndex + 1);
                
            _selectedObject.FontFamily = cmbFontFamily.SelectedItem?.ToString() ?? "Arial";
            _selectedObject.FontSize = (float)numFontSize.Value;

            // Selective invalidation for property changes
            _invalidationManager.AddObjectDirtyRegion(_selectedObject, RESIZE_HANDLE_SIZE + 5);
            InvalidateDirtyRegions();
        }

        private void BtnColorPicker_Click(object sender, EventArgs e)
        {
            if (_selectedObject == null) return;

            using (ColorDialog colorDialog = new ColorDialog())
            {
                Color currentColor = _propertyService.GetColor(_selectedObject);
                colorDialog.Color = currentColor;
                
                if (colorDialog.ShowDialog() == DialogResult.OK)
                {
                    _propertyService.UpdateColor(_selectedObject, colorDialog.Color);
                    
                    // Selective invalidation for color changes
                    _invalidationManager.AddObjectDirtyRegion(_selectedObject, RESIZE_HANDLE_SIZE + 5);
                    InvalidateDirtyRegions();
                }
            }
        }

        private void BtnAddSquareTable_Click(object sender, EventArgs e)
        {
            int tableNum = _objects.Count(o => o.ObjectType == ObjectType.Table) + 1;
            var table = _service.CreateTable(_currentSalon.Id, ShapeType.Square, $"Masa {tableNum}", tableNum, 100, 100, 80, 80);
            _objects.Add(table);
            
            // Add to QuadTree
            _renderingOptimizer.AddObject(table);
            
            // Selective invalidation for new object
            _invalidationManager.AddObjectDirtyRegion(table, RESIZE_HANDLE_SIZE + 5);
            InvalidateDirtyRegions();
        }

        private void BtnAddRoundTable_Click(object sender, EventArgs e)
        {
            int tableNum = _objects.Count(o => o.ObjectType == ObjectType.Table) + 1;
            var table = _service.CreateTable(_currentSalon.Id, ShapeType.Circle, $"Masa {tableNum}", tableNum, 200, 100, 80, 80);
            _objects.Add(table);
            
            // Add to QuadTree
            _renderingOptimizer.AddObject(table);
            
            // Selective invalidation for new object
            _invalidationManager.AddObjectDirtyRegion(table, RESIZE_HANDLE_SIZE + 5);
            InvalidateDirtyRegions();
        }

        private void BtnAddWall_Click(object sender, EventArgs e)
        {
            var wall = _service.CreateWall(_currentSalon.Id, ShapeType.Rectangle, "Duvar", 300, 100, 150, 20);
            _objects.Add(wall);
            
            // Add to QuadTree
            _renderingOptimizer.AddObject(wall);
            
            // Selective invalidation for new object
            _invalidationManager.AddObjectDirtyRegion(wall, RESIZE_HANDLE_SIZE + 5);
            InvalidateDirtyRegions();
        }

        private void BtnAddDecoration_Click(object sender, EventArgs e)
        {
            var decoration = _service.CreateDecoration(_currentSalon.Id, ShapeType.Circle, "Dekorasyon", 400, 100, 50, 50);
            _objects.Add(decoration);
            
            // Add to QuadTree
            _renderingOptimizer.AddObject(decoration);
            
            // Selective invalidation for new object
            _invalidationManager.AddObjectDirtyRegion(decoration, RESIZE_HANDLE_SIZE + 5);
            InvalidateDirtyRegions();
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (_selectedObject != null)
            {
                var props = DesignProperties.FromSalonObject(_selectedObject);
                _service.UpdateObjectDesignProperties(_selectedObject.Id, props);
                MessageBox.Show("Özellikler kaydedildi!", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (_selectedObject != null)
            {
                if (MessageBox.Show("Bu objeyi silmek istediğinizden emin misiniz?", "Onay", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    // Add dirty region before removing
                    _invalidationManager.AddObjectDirtyRegion(_selectedObject, RESIZE_HANDLE_SIZE + 5);
                    
                    _service.DeleteObject(_selectedObject.Id);
                    _objects.Remove(_selectedObject);
                    
                    // Remove from QuadTree
                    _renderingOptimizer.RemoveObject(_selectedObject);
                    
                    _selectedObject = null;
                    UpdatePropertyPanel();
                    
                    // Invalidate dirty regions
                    InvalidateDirtyRegions();
                }
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            _repository?.Dispose();
        }
    }
}
