using System;
using System.Windows;

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
            APIClient.Connect();
            var application = new App();
            application.Run(new WindowBasic());
        }
    }
}
