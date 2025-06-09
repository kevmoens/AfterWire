using System;
using System.Collections.Generic;
using System.Text;

namespace AfterWire
{
	public interface IAfterWireServiceProvider
	{
		TService GetService<TService>() where TService : class;
		TService GetKeyedService<TService>(object key) where TService : class;

		object GetService(Type service);
		object GetKeyedService(Type service, object key);

		TService GetRequiredService<TService>() where TService : class;
		TService GetKeyedRequiredService<TService>(object key) where TService : class;

		object GetRequiredService(Type service);
		object GetKeyedRequiredService(Type service, object key);

		void AddSingleton<TService, TImplementation>(TImplementation instance) 
			where TService : class
			where TImplementation : TService;
		void AddSingleton<TService>(TService instance) where TService : class;
		void AddSingleton<TService, TImplementation>(object key, TImplementation instance) 
			where TService : class
			where TImplementation : TService;
			
		void AddSingleton<TService>(object key, TService instance) where TService : class;

		void AddTransient<TService>() where TService : class;

		void AddTransient<TService, TImplementation>() where TService : class
			where TImplementation : TService;

		void AddTransient<TService>(Func<TService> factory) where TService : class;
		void AddTransient<TService>(object key, Func<TService> factory) where TService : class;
		void AddTransient<TService>(object key) where TService : class;
		void AddTransient<TService, TImplementation>(object key) 
			where TService : class
			where TImplementation : TService;

	}
}
