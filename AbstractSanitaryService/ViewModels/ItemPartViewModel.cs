using System.Runtime.Serialization;

namespace AbstractSanitaryService.ViewModels
{
    [DataContract]
    public class ItemPartViewModel
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public int ItemId { get; set; }

        [DataMember]
        public int PartId { get; set; }

        [DataMember]
        public string PartName { get; set; }

        [DataMember]
        public int Count { get; set; }
    }
}
