using System.Runtime.Serialization;

namespace AbstractSanitaryService.ViewModels
{
    [DataContract]
    public class PlumberViewModel
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string PlumberFIO { get; set; }
    }
}
