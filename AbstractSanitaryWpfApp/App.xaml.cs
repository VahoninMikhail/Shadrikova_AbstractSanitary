using AbstractSanitaryService.ImplementationsList;
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
