using System;
using System.Collections.Generic;
using System.Drawing;
using SalonDesign.Models;

namespace SalonDesign.Services
{
    /// <summary>
    /// Invalidation (yeniden çizim) yönetimi
    /// Sadece değişen bölgeleri yeniden çizer, performans artışı sağlar
    /// </summary>
    public class InvalidationManager
    {
        private List<Rectangle> dirtyRegions;
        private Rectangle canvasBounds;

        public InvalidationManager(Rectangle canvasBounds)
        {
            this.canvasBounds = canvasBounds;
            this.dirtyRegions = new List<Rectangle>();
        }

        /// <summary>
        /// Canvas sınırlarını günceller
        /// </summary>
        public void UpdateCanvasBounds(Rectangle bounds)
        {
            this.canvasBounds = bounds;
        }

        /// <summary>
        /// Kirli bölge ekler (yeniden çizilmesi gereken alan)
        /// </summary>
        /// <param name="region">Kirli bölge</param>
        public void AddDirtyRegion(Rectangle region)
        {
            if (region.Width <= 0 || region.Height <= 0)
                return;

            // Canvas sınırları içinde tut
            region.Intersect(canvasBounds);
            
            if (region.Width > 0 && region.Height > 0)
            {
                dirtyRegions.Add(region);
            }
        }

        /// <summary>
        /// Obje için kirli bölge ekler
        /// </summary>
        /// <param name="obj">Salon objesi</param>
        /// <param name="padding">Ekstra padding (resize handle vs için)</param>
        public void AddObjectDirtyRegion(SalonObject obj, int padding = 10)
        {
            Rectangle region = new Rectangle(
                obj.PositionX - padding,
                obj.PositionY - padding,
                obj.Width + (padding * 2),
                obj.Height + (padding * 2)
            );
            AddDirtyRegion(region);
        }

        /// <summary>
        /// Obje hareket ettiğinde eski ve yeni konumlarını kirli bölge olarak ekler
        /// </summary>
        /// <param name="obj">Hareket eden obje</param>
        /// <param name="oldPosition">Eski pozisyon</param>
        /// <param name="padding">Ekstra padding</param>
        public void AddMovedObjectDirtyRegions(SalonObject obj, Point oldPosition, int padding = 10)
        {
            // Eski konum
            Rectangle oldRegion = new Rectangle(
                oldPosition.X - padding,
                oldPosition.Y - padding,
                obj.Width + (padding * 2),
                obj.Height + (padding * 2)
            );
            AddDirtyRegion(oldRegion);

            // Yeni konum
            AddObjectDirtyRegion(obj, padding);
        }

        /// <summary>
        /// Obje boyutu değiştiğinde eski ve yeni boyutları kirli bölge olarak ekler
        /// </summary>
        /// <param name="obj">Boyutu değişen obje</param>
        /// <param name="oldSize">Eski boyut</param>
        /// <param name="padding">Ekstra padding</param>
        public void AddResizedObjectDirtyRegions(SalonObject obj, Size oldSize, int padding = 10)
        {
            // Eski boyut alanı
            Rectangle oldRegion = new Rectangle(
                obj.PositionX - padding,
                obj.PositionY - padding,
                oldSize.Width + (padding * 2),
                oldSize.Height + (padding * 2)
            );
            AddDirtyRegion(oldRegion);

            // Yeni boyut alanı
            AddObjectDirtyRegion(obj, padding);
        }

        /// <summary>
        /// Tüm kirli bölgeleri birleştirilmiş tek bir region olarak döndürür
        /// </summary>
        /// <returns>Birleştirilmiş kirli bölge</returns>
        public Region GetDirtyRegion()
        {
            if (dirtyRegions.Count == 0)
                return new Region(Rectangle.Empty);

            Region combinedRegion = new Region(dirtyRegions[0]);
            
            for (int i = 1; i < dirtyRegions.Count; i++)
            {
                combinedRegion.Union(dirtyRegions[i]);
            }

            return combinedRegion;
        }

        /// <summary>
        /// Tüm kirli bölgeleri liste olarak döndürür
        /// </summary>
        /// <returns>Kirli bölgeler listesi</returns>
        public List<Rectangle> GetDirtyRectangles()
        {
            return new List<Rectangle>(dirtyRegions);
        }

        /// <summary>
        /// Kirli bölgeleri optimize eder (çakışan bölgeleri birleştirir)
        /// </summary>
        public void OptimizeDirtyRegions()
        {
            if (dirtyRegions.Count <= 1)
                return;

            List<Rectangle> optimized = new List<Rectangle>();
            
            foreach (Rectangle rect in dirtyRegions)
            {
                bool merged = false;
                
                for (int i = 0; i < optimized.Count; i++)
                {
                    if (rect.IntersectsWith(optimized[i]))
                    {
                        // Birleştir
                        optimized[i] = Rectangle.Union(optimized[i], rect);
                        merged = true;
                        break;
                    }
                }
                
                if (!merged)
                {
                    optimized.Add(rect);
                }
            }

            dirtyRegions = optimized;
        }

        /// <summary>
        /// Kirli bölgeleri temizler
        /// </summary>
        public void Clear()
        {
            dirtyRegions.Clear();
        }

        /// <summary>
        /// Kirli bölge var mı kontrol eder
        /// </summary>
        public bool HasDirtyRegions
        {
            get { return dirtyRegions.Count > 0; }
        }

        /// <summary>
        /// Kirli bölge sayısını döndürür
        /// </summary>
        public int DirtyRegionCount
        {
            get { return dirtyRegions.Count; }
        }
    }
}
