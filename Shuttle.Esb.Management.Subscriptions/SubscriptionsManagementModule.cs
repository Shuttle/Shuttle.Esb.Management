using System.Collections.Generic;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Shuttle.Core.Data;
using Shuttle.Core.Infrastructure;
using Shuttle.Esb.Management.Shell;

namespace Shuttle.Esb.Management.Subscriptions
{
	public class SubscriptionsManagementModule : IManagementModule
	{
		private readonly WindsorContainer _container = new WindsorContainer();

		public void Configure(IManagementConfiguration managementConfiguration)
		{
			if (!managementConfiguration.HasDataStoreRepository)
			{
				Log.Warning(string.Format(ManagementResources.DataStoreRepositoryRequired,
					"Shuttle.Esb.Management.Subscriptions"));
			}

			_container.Register(Component.For<IReflectionService>()
				.ImplementedBy<ReflectionService>());

			_container.Register(Component.For<IDatabaseContextCache>()
				.ImplementedBy<ThreadStaticDatabaseContextCache>());

			_container.Register(Component.For<IDatabaseGateway>().ImplementedBy<DatabaseGateway>());
			_container.Register(Component.For<IDatabaseContextFactory>().ImplementedBy<DatabaseContextFactory>());
			_container.Register(Component.For(typeof (IDataRepository<>)).ImplementedBy(typeof (DataRepository<>)));

			_container.Register(
				Classes
					.FromAssemblyNamed("Shuttle.Core.Data")
					.Pick()
					.If(type => type.Name.EndsWith("Factory"))
					.Configure(configurer => configurer.Named(configurer.Implementation.Name.ToLower()))
					.WithService.Select((type, basetype) => new[] {type.InterfaceMatching(@".*Factory\Z")}));

			_container.Register(
				Classes
					.FromThisAssembly()
					.BasedOn(typeof (IDataRowMapper<>))
					.WithServiceFirstInterface());

			_container.Register(
				Classes
					.FromThisAssembly()
					.Pick()
					.If(type => type.Name.EndsWith("Repository"))
					.WithServiceFirstInterface());

			_container.Register(
				Classes
					.FromThisAssembly()
					.Pick()
					.If(type => type.Name.EndsWith("Query"))
					.WithServiceFirstInterface());

			_container.Register(
				Classes
					.FromThisAssembly()
					.Pick()
					.If(type => type.Name.EndsWith("QueryFactory"))
					.WithServiceFirstInterface());


			_container.Register(
				Classes
					.FromThisAssembly()
					.BasedOn<IManagementModulePresenter>()
					.WithServiceAllInterfaces());
		}

		public IEnumerable<IManagementModulePresenter> Presenters
		{
			get { return _container.ResolveAll<IManagementModulePresenter>(); }
		}
	}
}