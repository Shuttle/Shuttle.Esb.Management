using System.Collections.Generic;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Shuttle.Core.Data;
using Shuttle.Core.Infrastructure;
using Shuttle.Management.Shell;

namespace Shuttle.Management.Subscriptions
{
	public class SubscriptionsManagementModule : IManagementModule
	{
		private readonly WindsorContainer container = new WindsorContainer();

		public void Configure(IManagementConfiguration managementConfiguration)
		{
			if (!managementConfiguration.HasDataStoreRepository)
			{
				Log.Warning(string.Format(ManagementResources.DataStoreRepositoryRequired,
					"Shuttle.Management.Subscriptions"));
			}

			container.Register(Component.For<IReflectionService>()
				.ImplementedBy<ReflectionService>());

			container.Register(Component.For<IDatabaseContextCache>()
				.ImplementedBy<ThreadStaticDatabaseContextCache>());

			container.Register(Component.For<IDatabaseGateway>().ImplementedBy<DatabaseGateway>());
			container.Register(Component.For<IDatabaseContextFactory>().ImplementedBy<DatabaseContextFactory>());
			container.Register(Component.For(typeof (IDataRepository<>)).ImplementedBy(typeof (DataRepository<>)));

			container.Register(
				Classes
					.FromAssemblyNamed("Shuttle.Core.Data")
					.Pick()
					.If(type => type.Name.EndsWith("Factory"))
					.Configure(configurer => configurer.Named(configurer.Implementation.Name.ToLower()))
					.WithService.Select((type, basetype) => new[] {type.InterfaceMatching(@".*Factory\Z")}));

			container.Register(
				Classes
					.FromThisAssembly()
					.BasedOn(typeof (IDataRowMapper<>))
					.WithServiceFirstInterface());

			container.Register(
				Classes
					.FromThisAssembly()
					.Pick()
					.If(type => type.Name.EndsWith("Repository"))
					.WithServiceFirstInterface());

			container.Register(
				Classes
					.FromThisAssembly()
					.Pick()
					.If(type => type.Name.EndsWith("Query"))
					.WithServiceFirstInterface());

			container.Register(
				Classes
					.FromThisAssembly()
					.Pick()
					.If(type => type.Name.EndsWith("QueryFactory"))
					.WithServiceFirstInterface());


			container.Register(
				Classes
					.FromThisAssembly()
					.BasedOn<IManagementModulePresenter>()
					.WithServiceAllInterfaces());
		}

		public IEnumerable<IManagementModulePresenter> Presenters
		{
			get { return container.ResolveAll<IManagementModulePresenter>(); }
		}
	}
}