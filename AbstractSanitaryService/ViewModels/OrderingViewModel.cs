using System.Runtime.Serialization;

namespace AbstractSanitaryService.ViewModels
{
    [DataContract]
    public class OrderingViewModel
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public int CustomerId { get; set; }

        [DataMember]
        public string CustomerFIO { get; set; }

        [DataMember]
        public int ItemId { get; set; }

        [DataMember]
        public string ItemName { get; set; }

        [DataMember]
        public int? PlumberId { get; set; }

        [DataMember]
        public string PlumberName { get; set; }

        [DataMember]
        public int Count { get; set; }

        [DataMember]
        public decimal Sum { get; set; }

        [DataMember]
        public string Status { get; set; }

        [DataMember]
        public string DateCreate { get; set; }

        [DataMember]
        public string DateImplement { get; set; }
    }
}
