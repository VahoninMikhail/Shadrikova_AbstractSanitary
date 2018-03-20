using System.Collections.Generic;

namespace AbstractSanitaryService.ViewModels
{
    public class ItemViewModel
    {
        public int Id { get; set; }

        public string ItemName { get; set; }

        public decimal Price { get; set; }

        public List<ItemPartViewModel> ItemParts { get; set; }
    }
}
