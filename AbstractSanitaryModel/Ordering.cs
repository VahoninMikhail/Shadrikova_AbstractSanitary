using System;

namespace AbstractSanitaryModel
{
    public class Ordering
    {
        public int Id { get; set; }

        public int CustomerId { get; set; }

        public int ItemId { get; set; }

        public int? PlumberId { get; set; }

        public int Count { get; set; }

        public decimal Sum { get; set; }

        public OrderingStatus Status { get; set; }

        public DateTime DateCreate { get; set; }

        public DateTime? DateImplement { get; set; }
    }
}
