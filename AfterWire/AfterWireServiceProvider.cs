using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace AfterWire
{

	public class AfterWireServiceProvider : IAfterWireServiceProvider
	{
		private readonly ConcurrentDictionary<(Type, object), Func<object>> _factories = new ConcurrentDictionary<(Type, object), Func<object>>();
		private readonly IServiceProvider _serviceProvider;
		public AfterWireServiceProvider(IServiceProvider serviceProvider)
		{
			_serviceProvider = serviceProvider;
		}
		public T GetService<T>() where T : class
		{
			return GetKeyedService<T>(string.Empty);
		}

		public object GetService(Type serviceType)
		{
			if (serviceType == null) throw new ArgumentNullException(nameof(serviceType));
			if (!typeof(IAfterWireFactory<>).IsAssignableFrom(serviceType))
			{
				return _serviceProvider.GetService(serviceType);
			}

			var registrationKey = (serviceType, string.Empty);
			if (_factories.TryGetValue(registrationKey, out var factory))
			{
				return factory();
			}
			return null;
		}
		public T GetKeyedService<T>(object key) where T : class
		{
			var registrationKey = (typeof(T), key);

			if (_factories.TryGetValue(registrationKey, out var factory))
			{
				return (T)factory();
			}

			throw new KeyNotFoundException(
				$"Service of type {typeof(T).Name} with key '{key}' was not registered.");
		}
		public object GetKeyedService(Type serviceType, object key)
		{
			if (serviceType == null) throw new ArgumentNullException(nameof(serviceType));
			var registrationKey = (serviceType, key);
			if (_factories.TryGetValue(registrationKey, out var factory))
			{
				return factory();
			}

			return typeof(Microsoft.Extensions.DependencyInjection.ServiceProviderKeyedServiceExtensions)
				.GetMethod("GetKeyedService", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public)
				?.MakeGenericMethod(serviceType).Invoke(null, new object[] { _serviceProvider, key });
		}

		public T GetRequiredService<T>() where T : class
		{
			return GetKeyedRequiredService<T>(string.Empty);
		}

		public object GetRequiredService(Type serviceType)
		{
			object result = GetService(serviceType) ?? throw new KeyNotFoundException($"Required service of type {serviceType.Name} was not registered.");
			return result;
		}
		public T GetKeyedRequiredService<T>(object key) where T : class
		{
			T result = GetKeyedService<T>(key);

			return result ?? throw new KeyNotFoundException(
				$"Required service of type {typeof(T).Name} with key '{key}' was not registered.");
		}
		public object GetKeyedRequiredService(Type serviceType, object key)
		{
			object result = GetKeyedService(serviceType, key) ?? throw new KeyNotFoundException(
					$"Required service of type {serviceType.Name} with key '{key}' was not registered.");
			return result;
		}


		public void AddSingleton<T>(T instance) where T : class
		{
			AddTransient<T>(string.Empty, () => instance);
		}
		public void AddSingleton<TService, TImplementation>(TImplementation instance) 
			where TService : class
			where TImplementation : TService
		{
			AddTransient<TService>(string.Empty, () => instance);
		}

		public void AddSingleton<TService, TImplementation>(object key, TImplementation instance)
			where TService : class
			where TImplementation : TService
		{
			AddTransient<TService>(key, () => instance);
		}
		public void AddSingleton<T>(object key, T instance) where T : class
		{
			AddTransient<T>(key, () => instance);
		}

		public void AddTransient<T>() where T : class
		{
			AddTransient<T>("", () => AfterWireWireActivator.CreateInstance<T>(_serviceProvider));
		}
		public void AddTransient<TService, TImplementation>() where TService : class
			where TImplementation : TService
		{
			AddTransient<TService>("", () => AfterWireWireActivator.CreateInstance<TImplementation>(_serviceProvider));
		}
		public void AddTransient<T>(Func<T> factory) where T : class
		{
			AddTransient(string.Empty, factory);
		}

		public void AddTransient<T>(object key) where T : class
		{
			AddTransient<T>(key, () => AfterWireWireActivator.CreateInstance<T>(_serviceProvider));
		}
		public void AddTransient<TService, TImplementation>(object key)
			where TService : class
			where TImplementation : TService
		{
			AddTransient<TService>(key, () => AfterWireWireActivator.CreateInstance<TImplementation>(_serviceProvider));
		}
		public void AddTransient<T>(object key, Func<T> factory) where T : class
		{
			var registrationKey = (typeof(T), key);

			// TryAdd ensures we're not overwriting existing registrations.
			if (!_factories.TryAdd(registrationKey, () => factory()))
			{
				throw new ArgumentException(
					$"A service of type {typeof(T).Name} with key '{key}' is already registered.");
			}
		}
	}
}
