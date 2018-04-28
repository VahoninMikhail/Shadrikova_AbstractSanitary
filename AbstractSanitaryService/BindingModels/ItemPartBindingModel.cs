using System.Runtime.Serialization;

namespace AbstractSanitaryService.BindingModels
{
    [DataContract]
    public class ItemPartBindingModel
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public int ItemId { get; set; }

        [DataMember]
        public int PartId { get; set; }

        [DataMember]
        public int Count { get; set; }
    }
}
