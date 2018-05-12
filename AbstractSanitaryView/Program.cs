using AbstractSanitaryService;
using AbstractSanitaryService.ImplementationsBD;
using AbstractSanitaryService.Interfaces;
using System;
using System.Data.Entity;
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
            currentContainer.RegisterType<DbContext, AbstractDbContext>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<ICustomerService, CustomerServiceBD>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<IPartService, PartServiceBD>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<IPlumberService, PlumberServiceBD>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<IItemService, ItemServiceBD>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<IWarehouseService, WarehouseServiceBD>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<IBasicService, BasicServiceBD>(new HierarchicalLifetimeManager());

            return currentContainer;
        }
    }
}
