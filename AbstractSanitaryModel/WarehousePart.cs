﻿namespace AbstractSanitaryModel
{
    public class WarehousePart
    {
        public int Id { get; set; }

        public int WarehouseId { get; set; }

        public int PartId { get; set; }

        public int Count { get; set; }

        public virtual Warehouse Warehouse { get; set; }

        public virtual Part Part { get; set; }
    }
}
