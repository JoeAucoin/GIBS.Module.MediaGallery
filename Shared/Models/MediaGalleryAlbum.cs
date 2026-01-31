using Oqtane.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GIBS.Module.MediaGallery.Models
{
    [NotMapped]
    public class MediaGalleryAlbum
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string ThumbnailPath { get; set; }
        public int ItemCount { get; set; }
    }
}
