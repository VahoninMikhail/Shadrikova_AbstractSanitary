namespace AbstractSanitaryService.ViewModels
{
    public class CustomerOrderingsViewModel
    {
        public string CustomerName { get; set; }

        public string DateCreate { get; set; }

        public string ItemName { get; set; }

        public int Count { get; set; }

        public decimal Sum { get; set; }

        public string Status { get; set; }
    }
}