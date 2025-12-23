using System.Drawing;
using SalonDesign.Models;

namespace SalonDesign.Services
{
    /// <summary>
    /// Tasarım özelliklerini yöneten servis sınıfı
    /// </summary>
    public class DesignPropertyService
    {
        public void UpdateColor(SalonObject obj, Color color)
        {
            if (obj != null)
            {
                obj.Color = ColorTranslator.ToHtml(color);
            }
        }

        public void UpdateText(SalonObject obj, string text)
        {
            if (obj != null)
            {
                obj.Text = text;
            }
        }

        public void UpdateTitle(SalonObject obj, string title)
        {
            if (obj != null)
            {
                obj.Title = title;
            }
        }

        public void UpdateFont(SalonObject obj, string fontFamily, float fontSize)
        {
            if (obj != null)
            {
                obj.FontFamily = fontFamily;
                obj.FontSize = fontSize;
            }
        }

        public Color GetColor(SalonObject obj)
        {
            if (obj != null && !string.IsNullOrEmpty(obj.Color))
            {
                try
                {
                    return ColorTranslator.FromHtml(obj.Color);
                }
                catch
                {
                    return Color.LightGray;
                }
            }
            return Color.LightGray;
        }

        public Font GetFont(SalonObject obj)
        {
            if (obj != null)
            {
                string family = string.IsNullOrEmpty(obj.FontFamily) ? "Arial" : obj.FontFamily;
                float size = obj.FontSize > 0 ? obj.FontSize : 10f;
                return new Font(family, size);
            }
            return new Font("Arial", 10f);
        }
    }
}
