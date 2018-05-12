using System.Runtime.Serialization;

namespace AbstractSanitaryService.BindingModels
{
    [DataContract]
    public class WarehousePartBindingModel
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public int WarehouseId { get; set; }

        [DataMember]
        public int PartId { get; set; }

        [DataMember]
        public int Count { get; set; }
    }
}
