namespace AbstractSanitaryModel
{
    public class ItemPart
    {
        public int Id { get; set; }

        public int ItemId { get; set; }

        public int PartId { get; set; }

        public int Count { get; set; }

        public virtual Item Item { get; set; }

        public virtual Part Part { get; set; }
    }
}
