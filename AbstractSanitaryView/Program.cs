using AbstractSanitaryService.ImplementationsList;
using AbstractSanitaryService.Interfaces;
using System;
using System.Windows.Forms;
using Unity;
using Unity.Lifetime;

namespace AbstractSanitaryView
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            var container = BuildUnityContainer();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(container.Resolve<FormBasic>());
        }

        public static IUnityContainer BuildUnityContainer()
        {
            var currentContainer = new UnityContainer();
            currentContainer.RegisterType<ICustomerService, CustomerServiceList>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<IPartService, PartServiceList>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<IPlumberService, PlumberServiceList>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<IItemService, ItemServiceList>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<IWarehouseService, WarehouseServiceList>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<IBasicService, BasicServiceList>(new HierarchicalLifetimeManager());

            return currentContainer;
        }
    }
}
