using AbstractSanitaryModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

namespace AbstractSanitaryService
{
    [Table("AbstractDatabase")]
    public class AbstractDbContext : DbContext
    {
        public AbstractDbContext()
        {
            Configuration.ProxyCreationEnabled = false;
            Configuration.LazyLoadingEnabled = false;
            var ensureDLLIsCopied = System.Data.Entity.SqlServer.SqlProviderServices.Instance;
        }

        public virtual DbSet<Customer> Customers { get; set; }

        public virtual DbSet<Part> Parts { get; set; }

        public virtual DbSet<Plumber> Plumbers { get; set; }

        public virtual DbSet<Ordering> Orderings { get; set; }

        public virtual DbSet<Item> Items { get; set; }

        public virtual DbSet<ItemPart> ItemParts { get; set; }

        public virtual DbSet<Warehouse> Warehouses { get; set; }

        public virtual DbSet<WarehousePart> WarehouseParts { get; set; }
    }
}
