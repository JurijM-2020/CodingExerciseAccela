using Contacts.Interfaces;
using Contacts.Model.Entities;
using Contacts.Repository;
using Contacts.UI.ViewModels;
using System.Windows;
using Unity;

namespace Contacts.UI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            IUnityContainer container = new UnityContainer();
            container.RegisterType<IRepositoryManager, RepositoryManager>();
            container.RegisterType<RepositoryContext>();
            var mainWindowViewModel = container.Resolve<MainContactsViewModel>();
            var window = new MainWindow { DataContext = mainWindowViewModel };
            window.Show();
        }
    }
}
