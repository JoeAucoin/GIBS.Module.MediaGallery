using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Oqtane.Databases.Interfaces;
using Oqtane.Migrations;
using GIBS.Module.MediaGallery.Migrations.EntityBuilders;
using GIBS.Module.MediaGallery.Repository;

namespace GIBS.Module.MediaGallery.Migrations
{
    [DbContext(typeof(MediaGalleryContext))]
    [Migration("GIBS.Module.MediaGallery.01.00.00.00")]
    public class InitializeModule : MultiDatabaseMigration
    {
        public InitializeModule(IDatabase database) : base(database)
        {
        }

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var categoryEntityBuilder = new MediaGalleryCategoryEntityBuilder(migrationBuilder, ActiveDatabase);
            categoryEntityBuilder.Create();

            var tagsEntityBuilder = new TagsEntityBuilder(migrationBuilder, ActiveDatabase);
            tagsEntityBuilder.Create();

            var itemEntityBuilder = new MediaGalleryItemEntityBuilder(migrationBuilder, ActiveDatabase);
            itemEntityBuilder.Create();

            var itemTagsEntityBuilder = new ItemTagsEntityBuilder(migrationBuilder, ActiveDatabase);
            itemTagsEntityBuilder.Create();
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            var itemTagsEntityBuilder = new ItemTagsEntityBuilder(migrationBuilder, ActiveDatabase);
            itemTagsEntityBuilder.Drop();

            var tagsEntityBuilder = new TagsEntityBuilder(migrationBuilder, ActiveDatabase);
            tagsEntityBuilder.Drop();

            var itemEntityBuilder = new MediaGalleryItemEntityBuilder(migrationBuilder, ActiveDatabase);
            itemEntityBuilder.Drop();

            var categoryEntityBuilder = new MediaGalleryCategoryEntityBuilder(migrationBuilder, ActiveDatabase);
            categoryEntityBuilder.Drop();
        }
    }
}
