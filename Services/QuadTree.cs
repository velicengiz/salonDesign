using System;
using System.Collections.Generic;
using System.Drawing;
using SalonDesign.Models;

namespace SalonDesign.Services
{
    /// <summary>
    /// QuadTree veri yapısı - Spatial indexing için kullanılır
    /// Görünür objeleri hızlı bir şekilde bulmak için optimize edilmiştir
    /// </summary>
    public class QuadTree
    {
        private const int MAX_OBJECTS = 10;
        private const int MAX_LEVELS = 5;

        private int level;
        private List<SalonObject> objects;
        private Rectangle bounds;
        private QuadTree[] nodes;

        /// <summary>
        /// QuadTree constructor
        /// </summary>
        /// <param name="level">QuadTree seviyesi</param>
        /// <param name="bounds">Bu node'un kapsadığı alan</param>
        public QuadTree(int level, Rectangle bounds)
        {
            this.level = level;
            this.objects = new List<SalonObject>();
            this.bounds = bounds;
            this.nodes = new QuadTree[4];
        }

        /// <summary>
        /// QuadTree'yi temizler
        /// </summary>
        public void Clear()
        {
            objects.Clear();

            for (int i = 0; i < nodes.Length; i++)
            {
                if (nodes[i] != null)
                {
                    nodes[i].Clear();
                    nodes[i] = null;
                }
            }
        }

        /// <summary>
        /// Node'u 4 alt node'a böler
        /// </summary>
        private void Split()
        {
            int subWidth = bounds.Width / 2;
            int subHeight = bounds.Height / 2;
            int x = bounds.X;
            int y = bounds.Y;

            nodes[0] = new QuadTree(level + 1, new Rectangle(x + subWidth, y, subWidth, subHeight));
            nodes[1] = new QuadTree(level + 1, new Rectangle(x, y, subWidth, subHeight));
            nodes[2] = new QuadTree(level + 1, new Rectangle(x, y + subHeight, subWidth, subHeight));
            nodes[3] = new QuadTree(level + 1, new Rectangle(x + subWidth, y + subHeight, subWidth, subHeight));
        }

        /// <summary>
        /// Objenin hangi node'a ait olduğunu belirler
        /// </summary>
        /// <param name="obj">Salon objesi</param>
        /// <returns>Node index (-1 = parent node'da kalmalı)</returns>
        private int GetIndex(SalonObject obj)
        {
            int index = -1;
            Rectangle objRect = new Rectangle(obj.PositionX, obj.PositionY, obj.Width, obj.Height);

            double verticalMidpoint = bounds.X + (bounds.Width / 2.0);
            double horizontalMidpoint = bounds.Y + (bounds.Height / 2.0);

            bool topQuadrant = (objRect.Y < horizontalMidpoint && objRect.Y + objRect.Height < horizontalMidpoint);
            bool bottomQuadrant = (objRect.Y > horizontalMidpoint);

            if (objRect.X < verticalMidpoint && objRect.X + objRect.Width < verticalMidpoint)
            {
                if (topQuadrant)
                {
                    index = 1;
                }
                else if (bottomQuadrant)
                {
                    index = 2;
                }
            }
            else if (objRect.X > verticalMidpoint)
            {
                if (topQuadrant)
                {
                    index = 0;
                }
                else if (bottomQuadrant)
                {
                    index = 3;
                }
            }

            return index;
        }

        /// <summary>
        /// QuadTree'ye obje ekler
        /// </summary>
        /// <param name="obj">Eklenecek obje</param>
        public void Insert(SalonObject obj)
        {
            if (nodes[0] != null)
            {
                int index = GetIndex(obj);

                if (index != -1)
                {
                    nodes[index].Insert(obj);
                    return;
                }
            }

            objects.Add(obj);

            if (objects.Count > MAX_OBJECTS && level < MAX_LEVELS)
            {
                if (nodes[0] == null)
                {
                    Split();
                }

                int i = 0;
                while (i < objects.Count)
                {
                    int index = GetIndex(objects[i]);
                    if (index != -1)
                    {
                        nodes[index].Insert(objects[i]);
                        objects.RemoveAt(i);
                    }
                    else
                    {
                        i++;
                    }
                }
            }
        }

        /// <summary>
        /// Belirtilen alanda bulunan objeleri döndürür
        /// </summary>
        /// <param name="returnObjects">Sonuç listesi</param>
        /// <param name="rect">Arama alanı</param>
        /// <returns>Bulunan objeler</returns>
        public List<SalonObject> Retrieve(List<SalonObject> returnObjects, Rectangle rect)
        {
            if (nodes[0] != null)
            {
                int index = GetIndexForRect(rect);
                
                if (index != -1)
                {
                    nodes[index].Retrieve(returnObjects, rect);
                }
                else
                {
                    // Birden fazla node'u kapsıyor, hepsini kontrol et
                    for (int i = 0; i < nodes.Length; i++)
                    {
                        if (nodes[i] != null && RectIntersects(nodes[i].bounds, rect))
                        {
                            nodes[i].Retrieve(returnObjects, rect);
                        }
                    }
                }
            }

            returnObjects.AddRange(objects);
            return returnObjects;
        }

        /// <summary>
        /// Rectangle için index bulur
        /// </summary>
        private int GetIndexForRect(Rectangle rect)
        {
            int index = -1;
            double verticalMidpoint = bounds.X + (bounds.Width / 2.0);
            double horizontalMidpoint = bounds.Y + (bounds.Height / 2.0);

            bool topQuadrant = (rect.Y < horizontalMidpoint && rect.Y + rect.Height < horizontalMidpoint);
            bool bottomQuadrant = (rect.Y > horizontalMidpoint);

            if (rect.X < verticalMidpoint && rect.X + rect.Width < verticalMidpoint)
            {
                if (topQuadrant)
                {
                    index = 1;
                }
                else if (bottomQuadrant)
                {
                    index = 2;
                }
            }
            else if (rect.X > verticalMidpoint)
            {
                if (topQuadrant)
                {
                    index = 0;
                }
                else if (bottomQuadrant)
                {
                    index = 3;
                }
            }

            return index;
        }

        /// <summary>
        /// İki rectangle'ın kesişip kesişmediğini kontrol eder
        /// </summary>
        private bool RectIntersects(Rectangle r1, Rectangle r2)
        {
            return r1.IntersectsWith(r2);
        }

        /// <summary>
        /// QuadTree'den obje siler
        /// </summary>
        /// <param name="obj">Silinecek obje</param>
        /// <returns>Obje silindiyse true</returns>
        public bool Remove(SalonObject obj)
        {
            if (objects.Remove(obj))
            {
                return true;
            }

            if (nodes[0] != null)
            {
                int index = GetIndex(obj);
                if (index != -1)
                {
                    return nodes[index].Remove(obj);
                }
                else
                {
                    // Birden fazla node'da olabilir, hepsini kontrol et
                    for (int i = 0; i < nodes.Length; i++)
                    {
                        if (nodes[i] != null && nodes[i].Remove(obj))
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }
    }
}
