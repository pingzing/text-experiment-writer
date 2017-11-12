using TextGameTool.Services;
using TextGameTool.Views;
using Autofac;
using CommonServiceLocator;
using Autofac.Extras.CommonServiceLocator;

namespace TextGameTool.ViewModels
{
    public class ViewModelLocator
    {
        private ContainerBuilder _builder;
        public static IContainer Container;

        public ViewModelLocator()
        {
            _builder = new ContainerBuilder();

            //Services
            _builder.RegisterType<NavigationServiceEx>().AsSelf().SingleInstance();
            _builder.RegisterType<DialogueFilesService>().As<IDialogueFileService>().SingleInstance();                                    

            // Pages
            _builder.RegisterType<ShellViewModel>().AsSelf().SingleInstance();
            _builder.RegisterType<MasterDetailViewModel>().AsSelf().SingleInstance();
            _builder.RegisterType<SettingsViewModel>().AsSelf().SingleInstance();            

            // Child VMs
            _builder.RegisterType<DialogueViewModel>().AsSelf().InstancePerLifetimeScope();

            Container = _builder.Build();
            ServiceLocator.SetLocatorProvider(() => new AutofacServiceLocator(Container));

            RegisterWithNavigation<MasterDetailViewModel, MasterDetailPage>();
            RegisterWithNavigation<SettingsViewModel, SettingsPage>();
        }

        public SettingsViewModel SettingsViewModel => Container.Resolve<SettingsViewModel>();

        public MasterDetailViewModel MasterDetailViewModel => Container.Resolve<MasterDetailViewModel>();

        public ShellViewModel ShellViewModel => Container.Resolve<ShellViewModel>();

        public NavigationServiceEx NavigationService => Container.Resolve<NavigationServiceEx>();

        public void RegisterWithNavigation<VM, V>()
            where VM : class
        {            
            NavigationService.Configure(typeof(VM).FullName, typeof(V));
        }
    }
}
