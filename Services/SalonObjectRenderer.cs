using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using SalonDesign.Enums;
using SalonDesign.Models;

namespace SalonDesign.Services
{
    /// <summary>
    /// Optimize edilmiş obje çizim yöntemleri
    /// Caching ve performans optimizasyonları içerir
    /// </summary>
    public class SalonObjectRenderer
    {
        private DesignPropertyService propertyService;
        
        // Render ayarları
        private bool useAntiAliasing;
        private bool useHighQuality;

        public SalonObjectRenderer(DesignPropertyService propertyService)
        {
            this.propertyService = propertyService;
            this.useAntiAliasing = true;
            this.useHighQuality = true;
        }

        /// <summary>
        /// Anti-aliasing kullanımını ayarlar
        /// </summary>
        public bool UseAntiAliasing
        {
            get { return useAntiAliasing; }
            set { useAntiAliasing = value; }
        }

        /// <summary>
        /// Yüksek kalite rendering kullanımını ayarlar
        /// </summary>
        public bool UseHighQuality
        {
            get { return useHighQuality; }
            set { useHighQuality = value; }
        }

        /// <summary>
        /// Graphics nesnesini optimize eder
        /// </summary>
        /// <param name="g">Graphics nesnesi</param>
        public void OptimizeGraphics(Graphics g)
        {
            if (useAntiAliasing)
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
            }
            else
            {
                g.SmoothingMode = SmoothingMode.HighSpeed;
                g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault;
            }

            if (useHighQuality)
            {
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.CompositingQuality = CompositingQuality.HighQuality;
            }
            else
            {
                g.InterpolationMode = InterpolationMode.Low;
                g.CompositingQuality = CompositingQuality.HighSpeed;
            }

            g.PixelOffsetMode = PixelOffsetMode.HighSpeed;
        }

        /// <summary>
        /// Objeyi çizer
        /// </summary>
        /// <param name="g">Graphics nesnesi</param>
        /// <param name="obj">Çizilecek obje</param>
        public void DrawObject(Graphics g, SalonObject obj)
        {
            Color color = propertyService.GetColor(obj);
            
            using (Brush brush = new SolidBrush(color))
            using (Pen pen = new Pen(Color.Black, 2))
            {
                Rectangle rect = new Rectangle(obj.PositionX, obj.PositionY, obj.Width, obj.Height);

                // Şekle göre çiz
                DrawShape(g, obj, rect, brush, pen);

                // Metni çiz
                DrawText(g, obj, rect);
            }
        }

        /// <summary>
        /// Şekli çizer
        /// </summary>
        private void DrawShape(Graphics g, SalonObject obj, Rectangle rect, Brush brush, Pen pen)
        {
            if (obj.ShapeType == ShapeType.Circle)
            {
                g.FillEllipse(brush, rect);
                g.DrawEllipse(pen, rect);
            }
            else if (obj.ShapeType == ShapeType.Square || obj.ShapeType == ShapeType.Rectangle)
            {
                if (obj.ObjectType == ObjectType.Wall)
                {
                    DrawCheckerPattern(g, rect, ((SolidBrush)brush).Color);
                }
                else
                {
                    g.FillRectangle(brush, rect);
                }
                g.DrawRectangle(pen, rect);
            }
        }

        /// <summary>
        /// Checker pattern çizer (duvarlar için)
        /// </summary>
        private void DrawCheckerPattern(Graphics g, Rectangle rect, Color baseColor)
        {
            int checkSize = 10;
            Color color1 = baseColor;
            Color color2 = ControlPaint.Light(baseColor);

            using (Brush brush1 = new SolidBrush(color1))
            using (Brush brush2 = new SolidBrush(color2))
            {
                for (int x = rect.Left; x < rect.Right; x += checkSize)
                {
                    for (int y = rect.Top; y < rect.Bottom; y += checkSize)
                    {
                        bool useColor1 = ((x - rect.Left) / checkSize + (y - rect.Top) / checkSize) % 2 == 0;
                        
                        int width = Math.Min(checkSize, rect.Right - x);
                        int height = Math.Min(checkSize, rect.Bottom - y);
                        
                        g.FillRectangle(useColor1 ? brush1 : brush2, x, y, width, height);
                    }
                }
            }
        }

        /// <summary>
        /// Obje üzerindeki metni çizer
        /// </summary>
        private void DrawText(Graphics g, SalonObject obj, Rectangle rect)
        {
            string displayText = GetDisplayText(obj);

            if (!string.IsNullOrEmpty(displayText))
            {
                using (Font font = propertyService.GetFont(obj))
                {
                    SizeF textSize = g.MeasureString(displayText, font);
                    PointF textPos = new PointF(
                        rect.X + (rect.Width - textSize.Width) / 2,
                        rect.Y + (rect.Height - textSize.Height) / 2
                    );
                    
                    using (Brush textBrush = new SolidBrush(Color.Black))
                    {
                        g.DrawString(displayText, font, textBrush, textPos);
                    }
                }
            }
        }

        /// <summary>
        /// Obje için görüntülenecek metni döndürür
        /// </summary>
        private string GetDisplayText(SalonObject obj)
        {
            if (!string.IsNullOrEmpty(obj.Text))
                return obj.Text;
            else if (!string.IsNullOrEmpty(obj.Title))
                return obj.Title;
            else if (obj.TableNumber.HasValue)
                return obj.TableNumber.Value.ToString();
            
            return string.Empty;
        }

        /// <summary>
        /// Seçim çerçevesi çizer
        /// </summary>
        /// <param name="g">Graphics nesnesi</param>
        /// <param name="obj">Seçili obje</param>
        public void DrawSelectionBorder(Graphics g, SalonObject obj)
        {
            Rectangle rect = new Rectangle(obj.PositionX - 2, obj.PositionY - 2, obj.Width + 4, obj.Height + 4);
            using (Pen pen = new Pen(Color.Blue, 2) { DashStyle = DashStyle.Dash })
            {
                g.DrawRectangle(pen, rect);
            }
        }

        /// <summary>
        /// Resize handle'larını çizer
        /// </summary>
        /// <param name="g">Graphics nesnesi</param>
        /// <param name="obj">Seçili obje</param>
        /// <param name="handleSize">Handle boyutu</param>
        public void DrawResizeHandles(Graphics g, SalonObject obj, int handleSize = 8)
        {
            Rectangle[] handles = GetResizeHandles(obj, handleSize);
            
            using (Brush handleBrush = new SolidBrush(Color.Blue))
            using (Pen handlePen = new Pen(Color.White, 1))
            {
                foreach (var handle in handles)
                {
                    g.FillRectangle(handleBrush, handle);
                    g.DrawRectangle(handlePen, handle);
                }
            }
        }

        /// <summary>
        /// Resize handle rectangle'larını döndürür
        /// </summary>
        private Rectangle[] GetResizeHandles(SalonObject obj, int handleSize)
        {
            int x = obj.PositionX;
            int y = obj.PositionY;
            int w = obj.Width;
            int h = obj.Height;
            int hs = handleSize;

            return new Rectangle[]
            {
                new Rectangle(x - hs/2, y - hs/2, hs, hs), // Top-left
                new Rectangle(x + w - hs/2, y - hs/2, hs, hs), // Top-right
                new Rectangle(x - hs/2, y + h - hs/2, hs, hs), // Bottom-left
                new Rectangle(x + w - hs/2, y + h - hs/2, hs, hs), // Bottom-right
            };
        }

        /// <summary>
        /// Belirtilen region'da objeyi çizer (clipping ile)
        /// </summary>
        /// <param name="g">Graphics nesnesi</param>
        /// <param name="obj">Çizilecek obje</param>
        /// <param name="clipRegion">Clipping region</param>
        public void DrawObjectWithClipping(Graphics g, SalonObject obj, Region clipRegion)
        {
            // Clipping region'u ayarla
            Region oldClip = g.Clip;
            g.SetClip(clipRegion, CombineMode.Intersect);
            
            // Objeyi çiz
            DrawObject(g, obj);
            
            // Eski clipping'i geri yükle
            g.Clip = oldClip;
        }
    }
}
