using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.EntityFrameworkCore.Migrations.Operations.Builders;
using Oqtane.Databases.Interfaces;
using Oqtane.Migrations;
using Oqtane.Migrations.EntityBuilders;

namespace GIBS.Module.MediaGallery.Migrations.EntityBuilders
{
    public class MediaGalleryItemEntityBuilder : AuditableBaseEntityBuilder<MediaGalleryItemEntityBuilder>
    {
        private const string _entityTableName = "GIBSMediaGallery_Item";
        private readonly PrimaryKey<MediaGalleryItemEntityBuilder> _primaryKey = new("PK_GIBSMediaGallery_Item", x => x.ItemId);
        private readonly ForeignKey<MediaGalleryItemEntityBuilder> _moduleForeignKey = new("FK_GIBSMediaGallery_Item_Module", x => x.ModuleId, "Module", "ModuleId", ReferentialAction.Cascade);
        private readonly ForeignKey<MediaGalleryItemEntityBuilder> _categoryForeignKey = new("FK_GIBSMediaGallery_Item_Category", x => x.CategoryId, "GIBSMediaGallery_Category", "CategoryId", ReferentialAction.Restrict);

        public MediaGalleryItemEntityBuilder(MigrationBuilder migrationBuilder, IDatabase database) : base(migrationBuilder, database)
        {
            EntityTableName = _entityTableName;
            PrimaryKey = _primaryKey;
            ForeignKeys.Add(_moduleForeignKey);
            ForeignKeys.Add(_categoryForeignKey);
        }

        protected override MediaGalleryItemEntityBuilder BuildTable(ColumnsBuilder table)
        {
            ItemId = AddAutoIncrementColumn(table, "ItemId");
            ModuleId = AddIntegerColumn(table, "ModuleId");
            CategoryId = AddIntegerColumn(table, "CategoryId", true);
            Title = AddStringColumn(table, "Title", 200);
            Description = AddStringColumn(table, "Description", 2000, true);
            MediaType = AddStringColumn(table, "MediaType", 20);
            FileId = AddIntegerColumn(table, "FileId");
            FilePath = AddStringColumn(table, "FilePath", 500);
            ThumbnailFileId = AddIntegerColumn(table, "ThumbnailFileId", true);
            ThumbnailPath = AddStringColumn(table, "ThumbnailPath", 500, true);
            SortOrder = AddIntegerColumn(table, "SortOrder", false, 0);
            IsActive = AddBooleanColumn(table, "IsActive", false, true);
            ViewCount = AddIntegerColumn(table, "ViewCount", false, 0);
            AddAuditableColumns(table);
            return this;
        }

        public OperationBuilder<AddColumnOperation> ItemId { get; set; }
        public OperationBuilder<AddColumnOperation> ModuleId { get; set; }
        public OperationBuilder<AddColumnOperation> CategoryId { get; set; }
        public OperationBuilder<AddColumnOperation> Title { get; set; }
        public OperationBuilder<AddColumnOperation> Description { get; set; }
        public OperationBuilder<AddColumnOperation> MediaType { get; set; }
        public OperationBuilder<AddColumnOperation> FileId { get; set; }
        public OperationBuilder<AddColumnOperation> FilePath { get; set; }
        public OperationBuilder<AddColumnOperation> ThumbnailFileId { get; set; }
        public OperationBuilder<AddColumnOperation> ThumbnailPath { get; set; }
        public OperationBuilder<AddColumnOperation> SortOrder { get; set; }
        public OperationBuilder<AddColumnOperation> IsActive { get; set; }
        public OperationBuilder<AddColumnOperation> ViewCount { get; set; }
    }
}
