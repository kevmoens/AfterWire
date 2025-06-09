
using Microsoft.Extensions.DependencyInjection;
using System;
namespace AfterWire
{
	public static class AfterWireServiceCollectionExtensions
	{
		public static IServiceCollection AddAfterWire(this IServiceCollection services)
		{
			// Register the AfterWireActivator as the default activator for the service provider.

			services.AddSingleton(typeof(IAfterWireFactory<>), typeof(AfterWireFactory<>));
			services.AddSingleton<IAfterWireServiceProvider, AfterWireServiceProvider>();
			return services;
		}

		public static IServiceCollection AddAfterWireSingleton(this IServiceCollection services, Type serviceType, Type impementationType)
		{
			// The lambda uses our custom activator to create TImplementation.
			services.AddSingleton(serviceType, sp => AfterWireWireActivator.CreateInstance(sp, impementationType));
			return services;
		}
		public static IServiceCollection AddAfterWireSingleton<TService, TImplementation>(this IServiceCollection services)
			where TService : class
			where TImplementation : TService
		{
			// The lambda uses our custom activator to create TImplementation.
			services.AddSingleton<TService>(sp => AfterWireWireActivator.CreateInstance<TImplementation>(sp));
			return services;
		}

		public static IServiceCollection AddAfterWireSingleton<TImplementation>(this IServiceCollection services)
			where TImplementation : class
		{
			services.AddSingleton<TImplementation>(sp => AfterWireWireActivator.CreateInstance<TImplementation>(sp));
			return services;
		}
		
		public static IServiceCollection AddAfterWireTransient<TService, TImplementation>(this IServiceCollection services)
			where TService : class
			where TImplementation : TService
		{
			services.AddTransient<TService>(sp => AfterWireWireActivator.CreateInstance<TImplementation>(sp));
			return services;
		}

		public static IServiceCollection AddAfterWireTransient<TImplementation>(this IServiceCollection services)
			where TImplementation : class
		{
			services.AddTransient<TImplementation>(sp => AfterWireWireActivator.CreateInstance<TImplementation>(sp));
			return services;
		}

		public static IServiceCollection AddAfterWireScoped<TService, TImplementation>(this IServiceCollection services)
			where TService : class
			where TImplementation : TService
		{
			services.AddScoped<TService>(sp => AfterWireWireActivator.CreateInstance<TImplementation>(sp));
			return services;
		}

		public static IServiceCollection AddAfterWireScoped<TImplementation>(this IServiceCollection services)
			where TImplementation : class
		{
			services.AddScoped<TImplementation>(sp => AfterWireWireActivator.CreateInstance<TImplementation>(sp));
			return services;
		}

		public static IServiceCollection AddAfterWireKeyedSingleton<TService, TImplementation>(this IServiceCollection services, string key)
			where TService : class
			where TImplementation : TService
		{
			services.AddKeyedSingleton<TService>(key, (sp, ts) =>
					AfterWireWireActivator.CreateInstance<TImplementation>(sp)
				);
			return services;
		}

		public static IServiceCollection AddAfterWireKeyedTransient<TService, TImplementation>(this IServiceCollection services, string key)
			where TService : class
			where TImplementation : TService
		{
			services.AddKeyedTransient<TService>(key, (sp, ts) =>
					AfterWireWireActivator.CreateInstance<TImplementation>(sp)
				);
			return services;
		}

		public static IServiceCollection AddAfterWireKeyedScoped<TService, TImplementation>(this IServiceCollection services, string key)
			where TService : class
			where TImplementation : TService
		{
			services.AddKeyedScoped<TService>(key, (sp, ts) =>
					AfterWireWireActivator.CreateInstance<TImplementation>(sp)
				);
			return services;
		}
	}
}