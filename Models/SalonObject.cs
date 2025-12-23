using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SalonDesign.Enums;

namespace SalonDesign.Models
{
    /// <summary>
    /// Salon obje bilgilerini tutan model (sd_table tablosu)
    /// Masalar, duvarlar ve dekoratif objeler için kullanılır
    /// </summary>
    [Table("sd_table")]
    public class SalonObject
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("salon_id")]
        public int SalonId { get; set; }

        [Required]
        [Column("object_type")]
        public ObjectType ObjectType { get; set; }

        [Column("shape_type")]
        public ShapeType ShapeType { get; set; }

        [MaxLength(100)]
        [Column("name")]
        public string Name { get; set; }

        [MaxLength(100)]
        [Column("title")]
        public string Title { get; set; }

        [Column("table_number")]
        public int? TableNumber { get; set; }

        [Column("position_x")]
        public int PositionX { get; set; }

        [Column("position_y")]
        public int PositionY { get; set; }

        [Column("width")]
        public int Width { get; set; }

        [Column("height")]
        public int Height { get; set; }

        [Column("color")]
        [MaxLength(50)]
        public string Color { get; set; }

        [Column("text")]
        [MaxLength(200)]
        public string Text { get; set; }

        [Column("font_family")]
        [MaxLength(100)]
        public string FontFamily { get; set; }

        [Column("font_size")]
        public float FontSize { get; set; }

        [Column("created_date")]
        public DateTime CreatedDate { get; set; }

        [Column("updated_date")]
        public DateTime? UpdatedDate { get; set; }

        [ForeignKey("SalonId")]
        public virtual Salon Salon { get; set; }

        public SalonObject()
        {
            CreatedDate = DateTime.Now;
            Color = "#CCCCCC";
            FontFamily = "Arial";
            FontSize = 10f;
        }
    }
}
