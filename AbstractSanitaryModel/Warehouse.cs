using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AbstractSanitaryModel
{
    public class Warehouse
    {
        public int Id { get; set; }

        [Required]
        public string WarehouseName { get; set; }

        [ForeignKey("WarehouseId")]
        public virtual List<WarehousePart> WarehouseParts { get; set; }
    }
}
