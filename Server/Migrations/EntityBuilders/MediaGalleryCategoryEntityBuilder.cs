using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.EntityFrameworkCore.Migrations.Operations.Builders;
using Oqtane.Databases.Interfaces;
using Oqtane.Migrations;
using Oqtane.Migrations.EntityBuilders;

namespace GIBS.Module.MediaGallery.Migrations.EntityBuilders
{
    public class MediaGalleryCategoryEntityBuilder : AuditableBaseEntityBuilder<MediaGalleryCategoryEntityBuilder>
    {
        private const string _entityTableName = "GIBSMediaGallery_Category";
        private readonly PrimaryKey<MediaGalleryCategoryEntityBuilder> _primaryKey = new("PK_GIBSMediaGallery_Category", x => x.CategoryId);
        private readonly ForeignKey<MediaGalleryCategoryEntityBuilder> _moduleForeignKey = new("FK_GIBSMediaGallery_Category_Module", x => x.ModuleId, "Module", "ModuleId", ReferentialAction.Cascade);
        private readonly ForeignKey<MediaGalleryCategoryEntityBuilder> _parentCategoryForeignKey = new("FK_GIBSMediaGallery_Category_Parent", x => x.ParentCategoryId, _entityTableName, "CategoryId", ReferentialAction.NoAction);

        public MediaGalleryCategoryEntityBuilder(MigrationBuilder migrationBuilder, IDatabase database) : base(migrationBuilder, database)
        {
            EntityTableName = _entityTableName;
            PrimaryKey = _primaryKey;
            ForeignKeys.Add(_moduleForeignKey);
            ForeignKeys.Add(_parentCategoryForeignKey);
        }

        protected override MediaGalleryCategoryEntityBuilder BuildTable(ColumnsBuilder table)
        {
            CategoryId = AddAutoIncrementColumn(table, "CategoryId");
            ModuleId = AddIntegerColumn(table, "ModuleId");
            CategoryName = AddStringColumn(table, "CategoryName", 100);
            Description = AddStringColumn(table, "Description", 500, true);
            SortOrder = AddIntegerColumn(table, "SortOrder", false, 0);
            IsActive = AddBooleanColumn(table, "IsActive", false, true);
            ParentCategoryId = AddIntegerColumn(table, "ParentCategoryId", true);
            ViewRoles = AddStringColumn(table, "ViewRoles", 500, true);
            AddAuditableColumns(table);
            return this;
        }

        public OperationBuilder<AddColumnOperation> CategoryId { get; set; }
        public OperationBuilder<AddColumnOperation> ModuleId { get; set; }
        public OperationBuilder<AddColumnOperation> CategoryName { get; set; }
        public OperationBuilder<AddColumnOperation> Description { get; set; }
        public OperationBuilder<AddColumnOperation> SortOrder { get; set; }
        public OperationBuilder<AddColumnOperation> IsActive { get; set; }
        public OperationBuilder<AddColumnOperation> ParentCategoryId { get; set; }
        public OperationBuilder<AddColumnOperation> ViewRoles { get; set; }
    }
}
