using Oqtane.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GIBS.Module.MediaGallery.Models
{
    [Table("GIBSMediaGallery_Tags")]
    public class Tags : IAuditable
    {
        [Key]
        public int TagId { get; set; }
        public int ModuleId { get; set; }
        [Required]
        [StringLength(100)]
        public string TagName { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
    }
}
