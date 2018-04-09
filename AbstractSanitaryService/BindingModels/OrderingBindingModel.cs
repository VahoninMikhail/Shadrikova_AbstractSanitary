namespace AbstractSanitaryService.BindingModels
{
    public class OrderingBindingModel
    {
        public int Id { get; set; }

        public int CustomerId { get; set; }

        public int ItemId { get; set; }

        public int? PlumberId { get; set; }

        public int Count { get; set; }

        public decimal Sum { get; set; }
    }
}
