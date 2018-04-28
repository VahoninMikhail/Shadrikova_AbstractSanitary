using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace AbstractSanitaryService.ViewModels
{
    [DataContract]
    public class WarehousesLoadViewModel
    {
        [DataMember]
        public string WarehouseName { get; set; }

        [DataMember]
        public int TotalCount { get; set; }

        [DataMember]
        public List<WarehousesPartLoadViewModel> Parts { get; set; }
    }

    [DataContract]
    public class WarehousesPartLoadViewModel
    {
        [DataMember]
        public string PartName { get; set; }

        [DataMember]
        public int Count { get; set; }
    }
}
