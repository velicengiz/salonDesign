using System.Drawing;
using SalonDesign.Enums;

namespace SalonDesign.Models
{
    /// <summary>
    /// Obje tasarım özelliklerini yöneten sınıf
    /// Runtimeda kullanılacak özellikler için
    /// </summary>
    public class DesignProperties
    {
        public Color Color { get; set; }
        public string Text { get; set; }
        public string Title { get; set; }
        public string FontFamily { get; set; }
        public float FontSize { get; set; }
        public ShapeType ShapeType { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public DesignProperties()
        {
            Color = Color.LightGray;
            FontFamily = "Arial";
            FontSize = 10f;
            ShapeType = ShapeType.Rectangle;
            Width = 100;
            Height = 100;
        }

        public static DesignProperties FromSalonObject(SalonObject obj)
        {
            var props = new DesignProperties
            {
                ShapeType = obj.ShapeType,
                Text = obj.Text,
                Title = obj.Title,
                FontFamily = obj.FontFamily ?? "Arial",
                FontSize = obj.FontSize > 0 ? obj.FontSize : 10f,
                Width = obj.Width,
                Height = obj.Height
            };

            if (!string.IsNullOrEmpty(obj.Color))
            {
                props.Color = ColorTranslator.FromHtml(obj.Color);
            }

            return props;
        }

        public void ApplyToSalonObject(SalonObject obj)
        {
            obj.ShapeType = ShapeType;
            obj.Text = Text;
            obj.Title = Title;
            obj.FontFamily = FontFamily;
            obj.FontSize = FontSize;
            obj.Width = Width;
            obj.Height = Height;
            obj.Color = ColorTranslator.ToHtml(Color);
        }
    }
}
