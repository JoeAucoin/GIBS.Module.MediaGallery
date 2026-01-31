using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.EntityFrameworkCore.Migrations.Operations.Builders;
using Oqtane.Databases.Interfaces;
using Oqtane.Migrations;
using Oqtane.Migrations.EntityBuilders;

namespace GIBS.Module.MediaGallery.Migrations.EntityBuilders
{
    public class TagsEntityBuilder : AuditableBaseEntityBuilder<TagsEntityBuilder>
    {
        private const string _entityTableName = "GIBSMediaGallery_Tags";
        private readonly PrimaryKey<TagsEntityBuilder> _primaryKey = new("PK_GIBSMediaGallery_Tags", x => x.TagId);
        private readonly ForeignKey<TagsEntityBuilder> _moduleForeignKey = new("FK_GIBSMediaGallery_Tags_Module", x => x.ModuleId, "Module", "ModuleId", ReferentialAction.Cascade);

        public TagsEntityBuilder(MigrationBuilder migrationBuilder, IDatabase database) : base(migrationBuilder, database)
        {
            EntityTableName = _entityTableName;
            PrimaryKey = _primaryKey;
            ForeignKeys.Add(_moduleForeignKey);
        }

        protected override TagsEntityBuilder BuildTable(ColumnsBuilder table)
        {
            TagId = AddAutoIncrementColumn(table, "TagId");
            ModuleId = AddIntegerColumn(table, "ModuleId");
            TagName = AddStringColumn(table, "TagName", 100);
            AddAuditableColumns(table);
            return this;
        }

        public OperationBuilder<AddColumnOperation> TagId { get; set; }
        public OperationBuilder<AddColumnOperation> ModuleId { get; set; }
        public OperationBuilder<AddColumnOperation> TagName { get; set; }
    }
}
