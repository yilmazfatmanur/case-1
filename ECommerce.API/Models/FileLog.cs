using System;
using System.ComponentModel.DataAnnotations;

namespace ECommerceAPI.Models
{
    public class FileLog
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(255)]  // 225 deðil, 255!
        public string FileName { get; set; }

        [MaxLength(500)]
        public string OriginalPath { get; set; }

        [MaxLength(500)]
        public string NewPath { get; set; }

        [MaxLength(10)]
        public string Extension { get; set; }

        [MaxLength(50)]
        public string Category { get; set; }

        public long FileSize { get; set; }

        public DateTime MoveDate { get; set; } = DateTime.Now;
    }
}