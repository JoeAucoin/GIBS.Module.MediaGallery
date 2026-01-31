using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.EntityFrameworkCore.Migrations.Operations.Builders;
using Oqtane.Databases.Interfaces;
using Oqtane.Migrations;
using Oqtane.Migrations.EntityBuilders;

namespace GIBS.Module.MediaGallery.Migrations.EntityBuilders
{
    public class ItemTagsEntityBuilder : AuditableBaseEntityBuilder<ItemTagsEntityBuilder>
    {
        private const string _entityTableName = "GIBSMediaGallery_ItemTags";
        private readonly PrimaryKey<ItemTagsEntityBuilder> _primaryKey = new("PK_GIBSMediaGallery_ItemTags", x => x.ItemTagId);
        private readonly ForeignKey<ItemTagsEntityBuilder> _itemForeignKey = new("FK_GIBSMediaGallery_ItemTags_Item", x => x.ItemId, "GIBSMediaGallery_Item", "ItemId", ReferentialAction.NoAction);
        private readonly ForeignKey<ItemTagsEntityBuilder> _tagForeignKey = new("FK_GIBSMediaGallery_ItemTags_Tag", x => x.TagId, "GIBSMediaGallery_Tags", "TagId", ReferentialAction.Cascade);

        public ItemTagsEntityBuilder(MigrationBuilder migrationBuilder, IDatabase database) : base(migrationBuilder, database)
        {
            EntityTableName = _entityTableName;
            PrimaryKey = _primaryKey;
            ForeignKeys.Add(_itemForeignKey);
            ForeignKeys.Add(_tagForeignKey);
        }

        protected override ItemTagsEntityBuilder BuildTable(ColumnsBuilder table)
        {
            ItemTagId = AddAutoIncrementColumn(table, "ItemTagId");
            ItemId = AddIntegerColumn(table, "ItemId");
            TagId = AddIntegerColumn(table, "TagId");
            AddAuditableColumns(table);
            return this;
        }

        public OperationBuilder<AddColumnOperation> ItemTagId { get; set; }
        public OperationBuilder<AddColumnOperation> ItemId { get; set; }
        public OperationBuilder<AddColumnOperation> TagId { get; set; }
    }
}
