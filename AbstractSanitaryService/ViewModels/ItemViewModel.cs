using System.Collections.Generic;
using System.Runtime.Serialization;

namespace AbstractSanitaryService.ViewModels
{
    [DataContract]
    public class ItemViewModel
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string ItemName { get; set; }

        [DataMember]
        public decimal Price { get; set; }

        [DataMember]
        public List<ItemPartViewModel> ItemParts { get; set; }
    }
}
