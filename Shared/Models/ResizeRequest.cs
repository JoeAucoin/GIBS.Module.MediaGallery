namespace GIBS.Module.MediaGallery.Models
{
    public class ResizeRequest
    {
        public int FileId { get; set; }
        public int ItemId { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int ModuleId { get; set; }
    }
}
