using AbstractSanitaryModel;
using System.Collections.Generic;

namespace AbstractSanitaryService
{
    class ListDataSingleton
    {
        private static ListDataSingleton instance;

        public List<Customer> Customers { get; set; }

        public List<Part> Parts { get; set; }

        public List<Plumber> Plumbers { get; set; }

        public List<Ordering> Orderings { get; set; }

        public List<Item> Items { get; set; }

        public List<ItemPart> ItemParts { get; set; }

        public List<Warehouse> Warehouses { get; set; }

        public List<WarehousePart> WarehouseParts { get; set; }

        private ListDataSingleton()
        {
            Customers = new List<Customer>();
            Parts = new List<Part>();
            Plumbers = new List<Plumber>();
            Orderings = new List<Ordering>();
            Items = new List<Item>();
            ItemParts = new List<ItemPart>();
            Warehouses = new List<Warehouse>();
            WarehouseParts = new List<WarehousePart>();
        }

        public static ListDataSingleton GetInstance()
        {
            if (instance == null)
            {
                instance = new ListDataSingleton();
            }

            return instance;
        }
    }
}
