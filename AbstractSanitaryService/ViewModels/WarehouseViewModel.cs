using System.Collections.Generic;
using System.Runtime.Serialization;

namespace AbstractSanitaryService.ViewModels
{
    [DataContract]
    public class WarehouseViewModel
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string WarehouseName { get; set; }

        [DataMember]
        public List<WarehousePartViewModel> WarehouseParts { get; set; }
    }
}
