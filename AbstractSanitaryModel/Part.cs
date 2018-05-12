using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AbstractSanitaryModel
{
    public class Part
    {
        public int Id { get; set; }

        [Required]
        public string PartName { get; set; }

        [ForeignKey("PartId")]
        public virtual List<ItemPart> ItemParts { get; set; }

        [ForeignKey("PartId")]
        public virtual List<WarehousePart> WarehouseParts { get; set; }
    }
}
