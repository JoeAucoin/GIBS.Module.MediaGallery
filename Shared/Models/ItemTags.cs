using Oqtane.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace GIBS.Module.MediaGallery.Models
{
    [Table("GIBSMediaGallery_ItemTags")]
    public class ItemTags : IAuditable
    {
        [Key]
        public int ItemTagId { get; set; }
        public int ItemId { get; set; }
        public int TagId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
    }
}
