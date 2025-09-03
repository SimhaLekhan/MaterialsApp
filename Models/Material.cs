using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MaterialsApp.Models
{
    public class Material
    {
        public int MaterialId { get; set; }

        [Required, StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required, StringLength(50)]
        public string Category { get; set; } = string.Empty; // Cement/Steel/Sand...

        [StringLength(100)]
        public string? Brand { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [Range(0.01, double.MaxValue, ErrorMessage = "UnitPrice must be > 0")]
        public decimal UnitPrice { get; set; }

        [Required, StringLength(20)]
        public string UnitOfMeasure { get; set; } = "Bag"; // Bag/Kg/Ton

        public int InStockQty { get; set; }
        public int ReorderLevel { get; set; }

        [Column(TypeName = "decimal(5,2)")]
        [Range(0, 28, ErrorMessage = "GST must be 0–28")]
        public decimal GstPercent { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime AddedOn { get; set; } = DateTime.UtcNow;
    }
}
