using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SalonDesign.Models
{
    /// <summary>
    /// Salon bilgilerini tutan model (sd_salon tablosu)
    /// </summary>
    [Table("sd_salon")]
    public class Salon
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        [Column("name")]
        public string Name { get; set; }

        [Column("description")]
        [MaxLength(500)]
        public string Description { get; set; }

        [Column("width")]
        public int Width { get; set; }

        [Column("height")]
        public int Height { get; set; }

        [Column("created_date")]
        public DateTime CreatedDate { get; set; }

        [Column("updated_date")]
        public DateTime? UpdatedDate { get; set; }

        public virtual ICollection<SalonObject> Objects { get; set; }

        public Salon()
        {
            Objects = new List<SalonObject>();
            CreatedDate = DateTime.Now;
        }
    }
}
