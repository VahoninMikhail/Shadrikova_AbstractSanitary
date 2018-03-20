namespace AbstractSanitaryService.ViewModels
{
    public class OrderingViewModel
    {
        public int Id { get; set; }

        public int CustomerId { get; set; }

        public string CustomerFIO { get; set; }

        public int ItemId { get; set; }

        public string ItemName { get; set; }

        public int? PlumberId { get; set; }

        public string PlumberName { get; set; }

        public int Count { get; set; }

        public decimal Sum { get; set; }

        public string Status { get; set; }

        public string DateCreate { get; set; }

        public string DateImplement { get; set; }
    }
}
