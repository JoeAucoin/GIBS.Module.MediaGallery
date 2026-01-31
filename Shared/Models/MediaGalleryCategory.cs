using Oqtane.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GIBS.Module.MediaGallery.Models
{
    [Table("GIBSMediaGallery_Category")]
    public class MediaGalleryCategory : IAuditable
    {
        [Key]
        public int CategoryId { get; set; }
        public int ModuleId { get; set; }
        [Required]
        [StringLength(100)]
        public string CategoryName { get; set; }
        [StringLength(500)]
        public string Description { get; set; }
        public int SortOrder { get; set; } = 0;
        public bool IsActive { get; set; } = true;
        public int? ParentCategoryId { get; set; }
        [StringLength(500)]
        public string ViewRoles { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
    }
}
