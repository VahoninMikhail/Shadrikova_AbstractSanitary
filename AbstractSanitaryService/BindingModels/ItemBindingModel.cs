using System.Collections.Generic;
using System.Runtime.Serialization;

namespace AbstractSanitaryService.BindingModels
{
    [DataContract]
    public class ItemBindingModel
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string ItemName { get; set; }

        [DataMember]
        public decimal Price { get; set; }

        [DataMember]
        public List<ItemPartBindingModel> ItemParts { get; set; }
    }
}
