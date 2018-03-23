using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AbstractSanitaryModel
{
    public class Item
    {
        public int Id { get; set; }

        [Required]
        public string ItemName { get; set; }

        [Required]
        public decimal Price { get; set; }

        [ForeignKey("ItemId")]
        public virtual List<Ordering> Orderings { get; set; }

        [ForeignKey("ItemId")]
        public virtual List<ItemPart> ItemParts { get; set; }
    }
}
