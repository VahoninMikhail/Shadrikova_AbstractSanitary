using AbstractSanitaryService.ImplementationsBD;
using AbstractSanitaryService.Interfaces;
using System;
using System.Windows;
using Unity;
using Unity.Lifetime;

namespace AbstractSanitaryWpfApp
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        [STAThread]
        public static void Main()
        {
            var container = BuildUnityContainer();

            var application = new App();
            application.Run(container.Resolve<WindowBasic>());
        }

        public static IUnityContainer BuildUnityContainer()
        {
            var currentContainer = new UnityContainer();
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
