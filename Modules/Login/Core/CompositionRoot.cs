using System.Data.Entity;
using EntityFramework.Patterns;
using FieldReporting.Modules.Authentication.Core.Repositories;
using Microsoft.Practices.Unity;

namespace FieldReporting.Modules.Authentication.Core
{
    public static class CompositionRoot
    {
        private static readonly IUnityContainer UnityContainer = new UnityContainer();

        public static IUnityContainer Container
        {
            get { return UnityContainer; }
        }

        public static void RegisterServices()
        {
            // Registering interfaces of Unit Of Work & Generic Repository
            UnityContainer.RegisterType(typeof(IRepository<>), typeof(Repository<>));
            UnityContainer.RegisterType(typeof(IUnitOfWork), typeof(UnitOfWork));

            // Every time we ask for a EF context, we'll pass our own Context.
            UnityContainer.RegisterType(typeof(DbContext), typeof(AuthenticationDbContext));

            // Tricky part.
            // Your repositories and unit of work must share the same DbContextAdapter, so we register an instance that will always be used
            // on subsequente resolve.
            // Note : you should not use ContainerControlledLifetimeManager when using ASP.NET or MVC
            // and use a per request lifetime manager
            UnityContainer.RegisterInstance(new DbContextAdapter(UnityContainer.Resolve<DbContext>()),
                                            new ContainerControlledLifetimeManager());

            UnityContainer.RegisterType<IObjectSetFactory>(
                new InjectionFactory(con => con.Resolve<DbContextAdapter>())
                );

            UnityContainer.RegisterType<IObjectContext>(
                new InjectionFactory(con => con.Resolve<DbContextAdapter>())
                );

            UnityContainer.RegisterType<IUserCredentialRepository, UserCredentialRepository>();
        }
    }
}