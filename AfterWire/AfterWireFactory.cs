using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace AfterWire
{
	public  class AfterWireFactory<T>: IAfterWireFactory<T> where T : class
	{
		private readonly IServiceProvider _serviceProvider;
		private readonly IAfterWireServiceProvider _dynamicServiceProvider;

		public AfterWireFactory(IServiceProvider serviceProvider, IAfterWireServiceProvider dynamicServiceProvider)
		{
			_serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
			_dynamicServiceProvider = dynamicServiceProvider ?? throw new ArgumentNullException(nameof(dynamicServiceProvider));
		}
		public T GetService()
		{
			T instance = _serviceProvider.GetService<T>() ?? _dynamicServiceProvider.GetService<T>();
			return instance;
		}
		public T GetRequiredService()
		{
			T instance = _serviceProvider.GetRequiredService<T>() ?? _dynamicServiceProvider.GetRequiredService<T>();
			return instance;
		}
		public T GetKeyedService(object key)
		{
			T instance = _serviceProvider.GetKeyedService<T>(key) ?? _dynamicServiceProvider.GetKeyedService<T>(key);
			return instance;
		}

		public T GetKeyedRequiredService(object key)
		{
			T instance = GetKeyedService(key) ?? throw new KeyNotFoundException($"Required service of type {typeof(T).Name} with key '{key}' was not registered.");
			return instance;
		}
	}
}
