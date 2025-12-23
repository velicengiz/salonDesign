using System;
using System.Collections.Generic;
using System.Linq;
using SalonDesign.Data;
using SalonDesign.Models;

namespace SalonDesign.Services
{
    /// <summary>
    /// Salon veritabanı işlemlerini yöneten repository sınıfı
    /// </summary>
    public class SalonRepository : IDisposable
    {
        private readonly SalonDesignContext _context;

        public SalonRepository()
        {
            _context = new SalonDesignContext();
        }

        public SalonRepository(SalonDesignContext context)
        {
            _context = context;
        }

        public List<Salon> GetAllSalons()
        {
            return _context.Salons.ToList();
        }

        public Salon GetSalonById(int id)
        {
            return _context.Salons.Find(id);
        }

        public Salon GetSalonWithObjects(int id)
        {
            return _context.Salons
                .Include("Objects")
                .FirstOrDefault(s => s.Id == id);
        }

        public void AddSalon(Salon salon)
        {
            _context.Salons.Add(salon);
            _context.SaveChanges();
        }

        public void UpdateSalon(Salon salon)
        {
            salon.UpdatedDate = DateTime.Now;
            _context.Entry(salon).State = System.Data.Entity.EntityState.Modified;
            _context.SaveChanges();
        }

        public void DeleteSalon(int id)
        {
            var salon = _context.Salons.Find(id);
            if (salon != null)
            {
                _context.Salons.Remove(salon);
                _context.SaveChanges();
            }
        }

        public List<SalonObject> GetObjectsBySalonId(int salonId)
        {
            return _context.SalonObjects
                .Where(o => o.SalonId == salonId)
                .ToList();
        }

        public SalonObject GetObjectById(int id)
        {
            return _context.SalonObjects.Find(id);
        }

        public void AddObject(SalonObject obj)
        {
            _context.SalonObjects.Add(obj);
            _context.SaveChanges();
        }

        public void UpdateObject(SalonObject obj)
        {
            obj.UpdatedDate = DateTime.Now;
            _context.Entry(obj).State = System.Data.Entity.EntityState.Modified;
            _context.SaveChanges();
        }

        public void DeleteObject(int id)
        {
            var obj = _context.SalonObjects.Find(id);
            if (obj != null)
            {
                _context.SalonObjects.Remove(obj);
                _context.SaveChanges();
            }
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}
