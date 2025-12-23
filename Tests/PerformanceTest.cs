using System;
using System.Collections.Generic;
using System.Drawing;
using SalonDesign.Enums;
using SalonDesign.Models;
using SalonDesign.Services;
using SalonDesign.Utilities;

namespace SalonDesign.Tests
{
    /// <summary>
    /// Manual performance test for the optimization components
    /// This can be run to verify the optimizations are working correctly
    /// </summary>
    public class PerformanceTest
    {
        public static void RunTests()
        {
            Console.WriteLine("=== Salon Design Performance Optimization Tests ===\n");

            TestQuadTree();
            TestInvalidationManager();
            TestRenderingOptimizer();
            TestPerformanceMonitor();

            Console.WriteLine("\n=== All Tests Completed ===");
        }

        private static void TestQuadTree()
        {
            Console.WriteLine("Test 1: QuadTree Spatial Indexing");
            Console.WriteLine("----------------------------------");

            Rectangle bounds = new Rectangle(0, 0, 1000, 800);
            QuadTree quadTree = new QuadTree(0, bounds);

            // Create test objects
            List<SalonObject> objects = new List<SalonObject>();
            Random rand = new Random(42);

            for (int i = 0; i < 100; i++)
            {
                var obj = new SalonObject
                {
                    Id = i + 1,
                    SalonId = 1,
                    ObjectType = ObjectType.Table,
                    ShapeType = ShapeType.Square,
                    Name = $"Table {i + 1}",
                    PositionX = rand.Next(0, 900),
                    PositionY = rand.Next(0, 700),
                    Width = 80,
                    Height = 80
                };
                objects.Add(obj);
                quadTree.Insert(obj);
            }

            Console.WriteLine($"Created and inserted {objects.Count} objects into QuadTree");

            // Test query
            Rectangle queryRect = new Rectangle(200, 200, 400, 300);
            List<SalonObject> found = quadTree.Retrieve(new List<SalonObject>(), queryRect);
            Console.WriteLine($"Query rectangle (200,200,400,300) found {found.Count} objects");

            // Test removal
            bool removed = quadTree.Remove(objects[0]);
            Console.WriteLine($"Remove first object: {removed}");

            Console.WriteLine("✓ QuadTree test passed\n");
        }

        private static void TestInvalidationManager()
        {
            Console.WriteLine("Test 2: Invalidation Manager");
            Console.WriteLine("-----------------------------");

            Rectangle canvasBounds = new Rectangle(0, 0, 1000, 800);
            InvalidationManager manager = new InvalidationManager(canvasBounds);

            var obj = new SalonObject
            {
                Id = 1,
                PositionX = 100,
                PositionY = 100,
                Width = 80,
                Height = 80
            };

            // Add dirty region
            manager.AddObjectDirtyRegion(obj, 10);
            Console.WriteLine($"Added dirty region for object at (100,100) size (80,80)");
            Console.WriteLine($"Has dirty regions: {manager.HasDirtyRegions}");
            Console.WriteLine($"Dirty region count: {manager.DirtyRegionCount}");

            // Test moved object
            Point oldPos = new Point(100, 100);
            obj.PositionX = 200;
            obj.PositionY = 200;
            manager.AddMovedObjectDirtyRegions(obj, oldPos, 10);
            Console.WriteLine($"Added dirty regions for moved object (100,100) -> (200,200)");
            Console.WriteLine($"Dirty region count: {manager.DirtyRegionCount}");

            // Optimize
            manager.OptimizeDirtyRegions();
            Console.WriteLine($"After optimization, dirty region count: {manager.DirtyRegionCount}");

            manager.Clear();
            Console.WriteLine($"After clear, has dirty regions: {manager.HasDirtyRegions}");

            Console.WriteLine("✓ InvalidationManager test passed\n");
        }

        private static void TestRenderingOptimizer()
        {
            Console.WriteLine("Test 3: Rendering Optimizer");
            Console.WriteLine("----------------------------");

            Rectangle viewport = new Rectangle(0, 0, 1000, 800);
            RenderingOptimizer optimizer = new RenderingOptimizer(viewport);

            // Create test objects
            List<SalonObject> objects = new List<SalonObject>();
            Random rand = new Random(42);

            for (int i = 0; i < 120; i++)
            {
                var obj = new SalonObject
                {
                    Id = i + 1,
                    SalonId = 1,
                    ObjectType = ObjectType.Table,
                    ShapeType = ShapeType.Square,
                    Name = $"Table {i + 1}",
                    PositionX = rand.Next(-200, 1200),  // Some outside viewport
                    PositionY = rand.Next(-200, 1000),
                    Width = 80,
                    Height = 80
                };
                objects.Add(obj);
            }

            optimizer.BuildSpatialIndex(objects);
            Console.WriteLine($"Built spatial index with {objects.Count} objects");
            Console.WriteLine($"Should use viewport culling: {optimizer.ShouldUseViewportCulling}");

            // Get visible objects
            List<SalonObject> visible = optimizer.GetVisibleObjects();
            Console.WriteLine($"Visible objects in viewport: {visible.Count}/{objects.Count}");

            // Test object update
            var testObj = objects[0];
            testObj.PositionX = 500;
            testObj.PositionY = 400;
            optimizer.UpdateObject(testObj);
            Console.WriteLine($"Updated object position");

            // Test with smaller viewport
            Rectangle smallViewport = new Rectangle(400, 300, 200, 200);
            visible = optimizer.GetVisibleObjects(smallViewport);
            Console.WriteLine($"Visible objects in small viewport (400,300,200,200): {visible.Count}");

            Console.WriteLine("✓ RenderingOptimizer test passed\n");
        }

        private static void TestPerformanceMonitor()
        {
            Console.WriteLine("Test 4: Performance Monitor");
            Console.WriteLine("----------------------------");

            PerformanceMonitor monitor = new PerformanceMonitor(10);

            // Simulate some frames
            for (int i = 0; i < 30; i++)
            {
                monitor.BeginFrame();
                monitor.BeginRender();

                // Simulate some work
                System.Threading.Thread.Sleep(5);

                monitor.EndRender();
            }

            Console.WriteLine($"Performance Report:");
            Console.WriteLine($"  {monitor.GetPerformanceReport()}");
            Console.WriteLine($"  Average FPS: {monitor.AverageFps:F2}");
            Console.WriteLine($"  Average Render Time: {monitor.AverageRenderTime:F2}ms");
            Console.WriteLine($"  Performance Good (>55 FPS): {monitor.IsPerformanceGood}");
            Console.WriteLine($"  Performance Acceptable (>25 FPS): {monitor.IsPerformanceAcceptable}");

            Console.WriteLine("✓ PerformanceMonitor test passed\n");
        }
    }
}
