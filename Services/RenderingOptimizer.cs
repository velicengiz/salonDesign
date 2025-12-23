using System;
using System.Collections.Generic;
using System.Drawing;
using SalonDesign.Models;

namespace SalonDesign.Services
{
    /// <summary>
    /// Rendering optimizasyonu için viewport culling ve görünür obje filtreleme
    /// Sadece ekranda görünen objeleri render eder
    /// </summary>
    public class RenderingOptimizer
    {
        private QuadTree quadTree;
        private Rectangle viewport;
        private int objectCount;

        public RenderingOptimizer(Rectangle initialViewport)
        {
            this.viewport = initialViewport;
            this.quadTree = new QuadTree(0, initialViewport);
            this.objectCount = 0;
        }

        /// <summary>
        /// Viewport'u günceller
        /// </summary>
        /// <param name="newViewport">Yeni viewport</param>
        public void UpdateViewport(Rectangle newViewport)
        {
            if (this.viewport != newViewport)
            {
                this.viewport = newViewport;
                
                // Viewport değiştiyse QuadTree'yi yeniden oluştur
                RebuildQuadTree();
            }
        }

        /// <summary>
        /// QuadTree'yi yeniden oluşturur
        /// </summary>
        private void RebuildQuadTree()
        {
            // Mevcut objeleri al
            List<SalonObject> allObjects = quadTree.Retrieve(new List<SalonObject>(), viewport);
            
            // QuadTree'yi temizle ve yeniden oluştur
            quadTree.Clear();
            quadTree = new QuadTree(0, viewport);
            
            // Objeleri tekrar ekle
            foreach (var obj in allObjects)
            {
                quadTree.Insert(obj);
            }
        }

        /// <summary>
        /// Tüm objeleri QuadTree'ye ekler
        /// </summary>
        /// <param name="objects">Eklenecek objeler</param>
        public void BuildSpatialIndex(List<SalonObject> objects)
        {
            quadTree.Clear();
            objectCount = 0;

            foreach (var obj in objects)
            {
                quadTree.Insert(obj);
                objectCount++;
            }
        }

        /// <summary>
        /// QuadTree'ye obje ekler
        /// </summary>
        /// <param name="obj">Eklenecek obje</param>
        public void AddObject(SalonObject obj)
        {
            quadTree.Insert(obj);
            objectCount++;
        }

        /// <summary>
        /// QuadTree'den obje siler
        /// </summary>
        /// <param name="obj">Silinecek obje</param>
        public void RemoveObject(SalonObject obj)
        {
            if (quadTree.Remove(obj))
            {
                objectCount--;
            }
        }

        /// <summary>
        /// Obje güncellendiğinde çağrılır (pozisyon veya boyut değişikliği)
        /// </summary>
        /// <param name="obj">Güncellenen obje</param>
        public void UpdateObject(SalonObject obj)
        {
            // Objeyi sil ve tekrar ekle (pozisyon değişmiş olabilir)
            quadTree.Remove(obj);
            quadTree.Insert(obj);
        }

        /// <summary>
        /// Viewport'ta görünür objeleri döndürür
        /// </summary>
        /// <returns>Görünür objeler</returns>
        public List<SalonObject> GetVisibleObjects()
        {
            return GetVisibleObjects(viewport);
        }

        /// <summary>
        /// Belirtilen alanda görünür objeleri döndürür
        /// </summary>
        /// <param name="region">Kontrol edilecek alan</param>
        /// <returns>Görünür objeler</returns>
        public List<SalonObject> GetVisibleObjects(Rectangle region)
        {
            List<SalonObject> visibleObjects = new List<SalonObject>();
            quadTree.Retrieve(visibleObjects, region);
            return visibleObjects;
        }

        /// <summary>
        /// Objenin viewport içinde olup olmadığını kontrol eder
        /// </summary>
        /// <param name="obj">Kontrol edilecek obje</param>
        /// <returns>Görünür ise true</returns>
        public bool IsObjectVisible(SalonObject obj)
        {
            Rectangle objRect = new Rectangle(obj.PositionX, obj.PositionY, obj.Width, obj.Height);
            return viewport.IntersectsWith(objRect);
        }

        /// <summary>
        /// QuadTree'yi tamamen yeniden oluşturur
        /// </summary>
        /// <param name="objects">Tüm objeler</param>
        public void Rebuild(List<SalonObject> objects)
        {
            BuildSpatialIndex(objects);
        }

        /// <summary>
        /// QuadTree'yi temizler
        /// </summary>
        public void Clear()
        {
            quadTree.Clear();
            objectCount = 0;
        }

        /// <summary>
        /// Toplam obje sayısını döndürür
        /// </summary>
        public int ObjectCount
        {
            get { return objectCount; }
        }

        /// <summary>
        /// Mevcut viewport'u döndürür
        /// </summary>
        public Rectangle Viewport
        {
            get { return viewport; }
        }

        /// <summary>
        /// Viewport culling'in aktif olup olmadığını belirler
        /// Az sayıda obje varsa culling'e gerek yok
        /// </summary>
        public bool ShouldUseViewportCulling
        {
            get { return objectCount > 50; }
        }
    }
}
