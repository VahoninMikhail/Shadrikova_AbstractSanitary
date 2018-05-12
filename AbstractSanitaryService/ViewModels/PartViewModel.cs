using System.Runtime.Serialization;

namespace AbstractSanitaryService.ViewModels
{
    [DataContract]
    public class PartViewModel
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string PartName { get; set; }
    }
}
