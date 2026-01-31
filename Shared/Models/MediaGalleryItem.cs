using Oqtane.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GIBS.Module.MediaGallery.Models
{
    [Table("GIBSMediaGallery_Item")]
    public class MediaGalleryItem : IAuditable
    {
        [Key]
        public int ItemId { get; set; }
        public int ModuleId { get; set; }
        public int? CategoryId { get; set; }
        [Required]
        [StringLength(200)]
        public string Title { get; set; }
        [StringLength(2000)]
        public string Description { get; set; }
        [Required]
        [StringLength(20)]
        public string MediaType { get; set; }
        public int FileId { get; set; }
        [Required]
        [StringLength(500)]
        public string FilePath { get; set; }
        public int? ThumbnailFileId { get; set; }
        [StringLength(500)]
        public string ThumbnailPath { get; set; }
        public int SortOrder { get; set; } = 0;
        public bool IsActive { get; set; } = true;
        public int ViewCount { get; set; } = 0;

        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }

        [NotMapped]
        public string CategoryName { get; set; } // Not mapped, for display only
        [NotMapped]
        public string Tags { get; set; }
    }
}
