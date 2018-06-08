using System.Runtime.Serialization;

namespace AbstractSanitaryService.BindingModels
{
    [DataContract]
    public class PartBindingModel
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string PartName { get; set; }
    }
}
