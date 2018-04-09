namespace AbstractSanitaryService.ViewModels
{
    public class WarehousePartViewModel
    {
        public int Id { get; set; }

        public int WarehouseId { get; set; }

        public int PartId { get; set; }

        public string PartName { get; set; }

        public int Count { get; set; }
    }
}
