using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace SalonDesign.Utilities
{
    /// <summary>
    /// Performans izleme ve metrik toplama
    /// Frame rate, render time ve diğer metrikleri takip eder
    /// </summary>
    public class PerformanceMonitor
    {
        private Stopwatch renderStopwatch;
        private Queue<long> frameTimes;
        private Queue<long> renderTimes;
        private int maxSampleCount;
        private long totalFrames;
        private long lastFrameTime;

        // Metrikler
        private double currentFps;
        private double averageFps;
        private double averageRenderTime;
        private long minRenderTime;
        private long maxRenderTime;

        public PerformanceMonitor(int sampleCount = 60)
        {
            this.maxSampleCount = sampleCount;
            this.frameTimes = new Queue<long>(sampleCount);
            this.renderTimes = new Queue<long>(sampleCount);
            this.renderStopwatch = new Stopwatch();
            this.totalFrames = 0;
            this.lastFrameTime = 0;
            this.minRenderTime = long.MaxValue;
            this.maxRenderTime = 0;
        }

        /// <summary>
        /// Frame başlangıcını işaretler
        /// </summary>
        public void BeginFrame()
        {
            long currentTime = Stopwatch.GetTimestamp();
            
            if (lastFrameTime > 0)
            {
                long frameTime = currentTime - lastFrameTime;
                frameTimes.Enqueue(frameTime);
                
                if (frameTimes.Count > maxSampleCount)
                {
                    frameTimes.Dequeue();
                }
            }
            
            lastFrameTime = currentTime;
            totalFrames++;
        }

        /// <summary>
        /// Render başlangıcını işaretler
        /// </summary>
        public void BeginRender()
        {
            renderStopwatch.Restart();
        }

        /// <summary>
        /// Render bitişini işaretler
        /// </summary>
        public void EndRender()
        {
            renderStopwatch.Stop();
            long renderTime = renderStopwatch.ElapsedTicks;
            
            renderTimes.Enqueue(renderTime);
            
            if (renderTimes.Count > maxSampleCount)
            {
                renderTimes.Dequeue();
            }

            // Min/Max güncelle
            if (renderTime < minRenderTime)
                minRenderTime = renderTime;
            if (renderTime > maxRenderTime)
                maxRenderTime = renderTime;

            // Metrikleri güncelle
            UpdateMetrics();
        }

        /// <summary>
        /// Metrikleri hesaplar
        /// </summary>
        private void UpdateMetrics()
        {
            // FPS hesapla
            if (frameTimes.Count > 0)
            {
                long totalFrameTime = 0;
                foreach (long time in frameTimes)
                {
                    totalFrameTime += time;
                }
                
                double avgFrameTime = (double)totalFrameTime / frameTimes.Count;
                averageFps = Stopwatch.Frequency / avgFrameTime;
                
                // Son frame için FPS
                if (frameTimes.Count > 0)
                {
                    long[] frameArray = frameTimes.ToArray();
                    long lastFrame = frameArray[frameArray.Length - 1];
                    currentFps = Stopwatch.Frequency / (double)lastFrame;
                }
            }

            // Ortalama render time hesapla
            if (renderTimes.Count > 0)
            {
                long totalRenderTime = 0;
                foreach (long time in renderTimes)
                {
                    totalRenderTime += time;
                }
                
                averageRenderTime = (double)totalRenderTime / renderTimes.Count;
                averageRenderTime = (averageRenderTime / Stopwatch.Frequency) * 1000; // milisaniyeye çevir
            }
        }

        /// <summary>
        /// Metrikleri sıfırlar
        /// </summary>
        public void Reset()
        {
            frameTimes.Clear();
            renderTimes.Clear();
            totalFrames = 0;
            lastFrameTime = 0;
            minRenderTime = long.MaxValue;
            maxRenderTime = 0;
            currentFps = 0;
            averageFps = 0;
            averageRenderTime = 0;
        }

        /// <summary>
        /// Performans raporunu string olarak döndürür
        /// </summary>
        public string GetPerformanceReport()
        {
            return string.Format(
                "FPS: {0:F1} (Avg: {1:F1}) | Render: {2:F2}ms (Min: {3:F2}ms, Max: {4:F2}ms) | Frames: {5}",
                currentFps,
                averageFps,
                averageRenderTime,
                (minRenderTime / (double)Stopwatch.Frequency) * 1000,
                (maxRenderTime / (double)Stopwatch.Frequency) * 1000,
                totalFrames
            );
        }

        /// <summary>
        /// Güncel FPS
        /// </summary>
        public double CurrentFps
        {
            get { return currentFps; }
        }

        /// <summary>
        /// Ortalama FPS
        /// </summary>
        public double AverageFps
        {
            get { return averageFps; }
        }

        /// <summary>
        /// Ortalama render süresi (milisaniye)
        /// </summary>
        public double AverageRenderTime
        {
            get { return averageRenderTime; }
        }

        /// <summary>
        /// Minimum render süresi (milisaniye)
        /// </summary>
        public double MinRenderTime
        {
            get { return (minRenderTime / (double)Stopwatch.Frequency) * 1000; }
        }

        /// <summary>
        /// Maximum render süresi (milisaniye)
        /// </summary>
        public double MaxRenderTime
        {
            get { return (maxRenderTime / (double)Stopwatch.Frequency) * 1000; }
        }

        /// <summary>
        /// Toplam frame sayısı
        /// </summary>
        public long TotalFrames
        {
            get { return totalFrames; }
        }

        /// <summary>
        /// Performans iyi mi? (60 FPS hedefi)
        /// </summary>
        public bool IsPerformanceGood
        {
            get { return averageFps >= 55.0; }
        }

        /// <summary>
        /// Performans kabul edilebilir mi? (30 FPS hedefi)
        /// </summary>
        public bool IsPerformanceAcceptable
        {
            get { return averageFps >= 25.0; }
        }
    }
}
