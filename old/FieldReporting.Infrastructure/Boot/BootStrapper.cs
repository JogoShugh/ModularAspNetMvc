using FieldReporting.Infrastructure.Eventing;
using FieldReporting.Infrastructure.Messaging;
using FieldReporting.Infrastructure.Ui.Mvc.Modularity;

namespace FieldReporting.Infrastructure.Boot
{
    public class BootStrapper
    {
        //private readonly IUnityContainer _container = new UnityContainer();

        //public IUnityContainer Container
        //{
        //    get { return _container; }
        //}

        public void Boot()
        {
            //Container.RegisterType(typeof (IRepository<>), typeof (Repository<>));
            //Container.RegisterType(typeof(IUnitOfWork), typeof(UnitOfWork));

            //UnityContainer.RegisterType(typeof(DbContext), typeof(Context));

            //// Tricky part.
            //// Your repositories and unit of work must share the same DbContextAdapter, so we register an instance that will always be used
            //// on subsequente resolve.
            //// Note : you should not use ContainerControlledLifetimeManager when using ASP.NET or MVC
            //// and use a per request lifetime manager
            //UnityContainer.RegisterInstance(new DbContextAdapter(UnityContainer.Resolve<DbContext>()), new ContainerControlledLifetimeManager());

            //UnityContainer.RegisterType<IObjectSetFactory>(
            //    new InjectionFactory(con => con.Resolve<DbContextAdapter>())
            //    );

            //UnityContainer.RegisterType<IObjectContext>(
            //    new InjectionFactory(con => con.Resolve<DbContextAdapter>())
            //    );






            var moduleLoader = new ModuleLoader();
            moduleLoader.LoadAllModules();

            var messageHandlerWiring = new MessageHandlerWiring();
            messageHandlerWiring.WireMessageHandlers(MessageProcessor.Instance);

            var eventSubscriberWiring = new EventSubscriberWiring();
            eventSubscriberWiring.WireEventListeners(EventAggregator.Instance);
        }

        public void RegisterComponents()
        {
            

        }
    }
}
