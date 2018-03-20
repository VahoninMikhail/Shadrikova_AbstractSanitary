using System.Collections.Generic;

namespace AbstractSanitaryService.BindingModels
{
    public class ItemBindingModel
    {
        public int Id { get; set; }

        public string ItemName { get; set; }

        public decimal Price { get; set; }

        public List<ItemPartBindingModel> ItemParts { get; set; }
    }
}
