using System.Runtime.Serialization;

namespace AbstractSanitaryService.BindingModels
{
    [DataContract]
    public class WarehouseBindingModel
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string WarehouseName { get; set; }
    }
}
