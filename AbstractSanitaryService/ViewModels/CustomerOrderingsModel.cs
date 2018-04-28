﻿using System.Runtime.Serialization;

namespace AbstractSanitaryService.ViewModels
{
    [DataContract]
    public class CustomerOrderingsModel
    {
        [DataMember]
        public string CustomerName { get; set; }

        [DataMember]
        public string DateCreate { get; set; }

        [DataMember]
        public string ItemName { get; set; }

        [DataMember]
        public int Count { get; set; }

        [DataMember]
        public decimal Sum { get; set; }

        [DataMember]
        public string Status { get; set; }
    }
}