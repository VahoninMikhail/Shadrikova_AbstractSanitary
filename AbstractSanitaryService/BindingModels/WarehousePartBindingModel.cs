namespace AbstractSanitaryService.BindingModels
{
    public class WarehousePartBindingModel
    {
        public int Id { get; set; }

        public int WarehouseId { get; set; }

        public int PartId { get; set; }

        public int Count { get; set; }
    }
}
