using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Oqtane.Modules;
using Oqtane.Repository;
using Oqtane.Infrastructure;
using Oqtane.Repository.Databases.Interfaces;

namespace GIBS.Module.MediaGallery.Repository
{
    public class MediaGalleryContext : DBContextBase, ITransientService, IMultiDatabase
    {
        public virtual DbSet<Models.MediaGalleryCategory> MediaGalleryCategory { get; set; }
        public virtual DbSet<Models.MediaGalleryItem> MediaGalleryItem { get; set; }
        public virtual DbSet<Models.ItemTags> ItemTags { get; set; }
        public virtual DbSet<Models.Tags> Tags { get; set; }

        public MediaGalleryContext(IDBContextDependencies DBContextDependencies) : base(DBContextDependencies)
        {
            // ContextBase handles multi-tenant database connections
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Models.MediaGalleryCategory>().ToTable(ActiveDatabase.RewriteName("GIBSMediaGallery_Category"));
            builder.Entity<Models.MediaGalleryItem>().ToTable(ActiveDatabase.RewriteName("GIBSMediaGallery_Item"));
            builder.Entity<Models.ItemTags>().ToTable(ActiveDatabase.RewriteName("GIBSMediaGallery_ItemTags"));
            builder.Entity<Models.Tags>().ToTable(ActiveDatabase.RewriteName("GIBSMediaGallery_Tags"));

            // ItemTags -> MediaGalleryItem relationship (many-to-one)
            builder.Entity<Models.ItemTags>()
                .HasOne<Models.MediaGalleryItem>()
                .WithMany()
                .HasForeignKey(it => it.ItemId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
