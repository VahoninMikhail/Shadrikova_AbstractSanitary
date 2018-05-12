namespace AbstractSanitaryService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FirstMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Customers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CustomerFIO = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Orderings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CustomerId = c.Int(nullable: false),
                        ItemId = c.Int(nullable: false),
                        PlumberId = c.Int(),
                        Count = c.Int(nullable: false),
                        Sum = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Status = c.Int(nullable: false),
                        DateCreate = c.DateTime(nullable: false),
                        DateImplement = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Customers", t => t.CustomerId, cascadeDelete: true)
                .ForeignKey("dbo.Items", t => t.ItemId, cascadeDelete: true)
                .ForeignKey("dbo.Plumbers", t => t.PlumberId)
                .Index(t => t.CustomerId)
                .Index(t => t.ItemId)
                .Index(t => t.PlumberId);
            
            CreateTable(
                "dbo.Items",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ItemName = c.String(nullable: false),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ItemParts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ItemId = c.Int(nullable: false),
                        PartId = c.Int(nullable: false),
                        Count = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Items", t => t.ItemId, cascadeDelete: true)
                .ForeignKey("dbo.Parts", t => t.PartId, cascadeDelete: true)
                .Index(t => t.ItemId)
                .Index(t => t.PartId);
            
            CreateTable(
                "dbo.Parts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PartName = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.WarehouseParts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        WarehouseId = c.Int(nullable: false),
                        PartId = c.Int(nullable: false),
                        Count = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Parts", t => t.PartId, cascadeDelete: true)
                .ForeignKey("dbo.Warehouses", t => t.WarehouseId, cascadeDelete: true)
                .Index(t => t.WarehouseId)
                .Index(t => t.PartId);
            
            CreateTable(
                "dbo.Warehouses",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        WarehouseName = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Plumbers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PlumberFIO = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Orderings", "PlumberId", "dbo.Plumbers");
            DropForeignKey("dbo.Orderings", "ItemId", "dbo.Items");
            DropForeignKey("dbo.WarehouseParts", "WarehouseId", "dbo.Warehouses");
            DropForeignKey("dbo.WarehouseParts", "PartId", "dbo.Parts");
            DropForeignKey("dbo.ItemParts", "PartId", "dbo.Parts");
            DropForeignKey("dbo.ItemParts", "ItemId", "dbo.Items");
            DropForeignKey("dbo.Orderings", "CustomerId", "dbo.Customers");
            DropIndex("dbo.WarehouseParts", new[] { "PartId" });
            DropIndex("dbo.WarehouseParts", new[] { "WarehouseId" });
            DropIndex("dbo.ItemParts", new[] { "PartId" });
            DropIndex("dbo.ItemParts", new[] { "ItemId" });
            DropIndex("dbo.Orderings", new[] { "PlumberId" });
            DropIndex("dbo.Orderings", new[] { "ItemId" });
            DropIndex("dbo.Orderings", new[] { "CustomerId" });
            DropTable("dbo.Plumbers");
            DropTable("dbo.Warehouses");
            DropTable("dbo.WarehouseParts");
            DropTable("dbo.Parts");
            DropTable("dbo.ItemParts");
            DropTable("dbo.Items");
            DropTable("dbo.Orderings");
            DropTable("dbo.Customers");
        }
    }
}
