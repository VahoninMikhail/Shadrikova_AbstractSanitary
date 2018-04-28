using System.Runtime.Serialization;

namespace AbstractSanitaryService.BindingModels
{
    [DataContract]
    public class PlumberBindingModel
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string PlumberFIO { get; set; }
    }
}
