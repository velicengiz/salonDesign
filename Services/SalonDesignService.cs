using System;
using System.Collections.Generic;
using SalonDesign.Enums;
using SalonDesign.Models;

namespace SalonDesign.Services
{
    /// <summary>
    /// Salon tasarım iş mantığını yöneten servis sınıfı
    /// </summary>
    public class SalonDesignService
    {
        private readonly SalonRepository _repository;

        public SalonDesignService(SalonRepository repository)
        {
            _repository = repository;
        }

        public Salon CreateSalon(string name, string description, int width, int height)
        {
            var salon = new Salon
            {
                Name = name,
                Description = description,
                Width = width,
                Height = height
            };

            _repository.AddSalon(salon);
            return salon;
        }

        public void UpdateSalonDimensions(int salonId, int width, int height)
        {
            var salon = _repository.GetSalonById(salonId);
            if (salon != null)
            {
                salon.Width = width;
                salon.Height = height;
                _repository.UpdateSalon(salon);
            }
        }

        public SalonObject CreateTable(int salonId, ShapeType shape, string name, int tableNumber, int x, int y, int width, int height)
        {
            var table = new SalonObject
            {
                SalonId = salonId,
                ObjectType = ObjectType.Table,
                ShapeType = shape,
                Name = name,
                TableNumber = tableNumber,
                PositionX = x,
                PositionY = y,
                Width = width,
                Height = height,
                Text = $"Masa {tableNumber}"
            };

            _repository.AddObject(table);
            return table;
        }

        public SalonObject CreateWall(int salonId, ShapeType shape, string title, int x, int y, int width, int height)
        {
            var wall = new SalonObject
            {
                SalonId = salonId,
                ObjectType = ObjectType.Wall,
                ShapeType = shape,
                Title = title,
                PositionX = x,
                PositionY = y,
                Width = width,
                Height = height,
                Color = "#8B4513"
            };

            _repository.AddObject(wall);
            return wall;
        }

        public SalonObject CreateDecoration(int salonId, ShapeType shape, string title, int x, int y, int width, int height)
        {
            var decoration = new SalonObject
            {
                SalonId = salonId,
                ObjectType = ObjectType.Decoration,
                ShapeType = shape,
                Title = title,
                PositionX = x,
                PositionY = y,
                Width = width,
                Height = height
            };

            _repository.AddObject(decoration);
            return decoration;
        }

        public void MoveObject(int objectId, int newX, int newY)
        {
            var obj = _repository.GetObjectById(objectId);
            if (obj != null)
            {
                obj.PositionX = newX;
                obj.PositionY = newY;
                _repository.UpdateObject(obj);
            }
        }

        public void ResizeObject(int objectId, int newWidth, int newHeight)
        {
            var obj = _repository.GetObjectById(objectId);
            if (obj != null)
            {
                obj.Width = newWidth;
                obj.Height = newHeight;
                _repository.UpdateObject(obj);
            }
        }

        public void UpdateObjectDesignProperties(int objectId, DesignProperties properties)
        {
            var obj = _repository.GetObjectById(objectId);
            if (obj != null)
            {
                properties.ApplyToSalonObject(obj);
                _repository.UpdateObject(obj);
            }
        }

        public List<SalonObject> GetSalonObjects(int salonId)
        {
            return _repository.GetObjectsBySalonId(salonId);
        }

        public void DeleteObject(int objectId)
        {
            _repository.DeleteObject(objectId);
        }

        public Salon GetSalon(int salonId)
        {
            return _repository.GetSalonById(salonId);
        }

        public List<Salon> GetAllSalons()
        {
            return _repository.GetAllSalons();
        }
    }
}
