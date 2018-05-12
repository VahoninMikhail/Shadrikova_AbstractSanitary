using System.Runtime.Serialization;

namespace AbstractSanitaryService.BindingModels
{
    [DataContract]
    public class OrderingBindingModel
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public int CustomerId { get; set; }

        [DataMember]
        public int ItemId { get; set; }

        [DataMember]
        public int? PlumberId { get; set; }

        [DataMember]
        public int Count { get; set; }

        [DataMember]
        public decimal Sum { get; set; }
    }
}
