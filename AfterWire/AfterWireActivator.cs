using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AfterWire
{
	internal class AfterWireWireActivator
	{

		public static T CreateInstance<T>(IServiceProvider provider)
		{
			return (T)CreateInstance(provider, typeof(T));
		}
		public static object CreateInstance(IServiceProvider provider, Type serviceType)
		{
			// For simplicity, we take the first available public constructor.
			var afterWireServiceProvider = provider.GetService<IAfterWireServiceProvider>();
			var constructor = serviceType.GetConstructors().First();
			var parametersInfo = constructor.GetParameters();
			object[] args = new object[parametersInfo.Length];

			for (int i = 0; i < parametersInfo.Length; i++)
			{
				var param = parametersInfo[i];

				//check if parameter implements IAfterWireFactory<T>
				if (param.ParameterType.IsGenericType && typeof(IAfterWireFactory<>).IsAssignableFrom(param.ParameterType.GetGenericTypeDefinition()))
				{
					var factoryType = param.ParameterType.GetGenericArguments()[0];
					var factory = Activator.CreateInstance(typeof(AfterWireFactory<>).MakeGenericType(factoryType), provider, provider) as IAfterWireFactory<object>;
					args[i] = factory;
					continue;
				}

				//Check for FromKeyedServiceAttribute
				if (param.GetCustomAttributes(typeof(FromKeyedServicesAttribute), false).FirstOrDefault() is FromKeyedServicesAttribute fromKeyedServiceAttribute)
				{
					// If the attribute is present, use the key to resolve the service.
					var key = fromKeyedServiceAttribute.Key;
					object service = (typeof(Microsoft.Extensions.DependencyInjection.ServiceProviderKeyedServiceExtensions)
						.GetMethod("GetKeyedService", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public)
						?.MakeGenericMethod(param.ParameterType).Invoke(null, new object[] { provider, key }))
						?? afterWireServiceProvider.GetKeyedService(param.ParameterType, key);
					args[i] = service;
					continue;
				}
				// Otherwise, fall back to the default DI resolution.
				args[i] = provider.GetService(param.ParameterType);			
			}

			return Activator.CreateInstance(serviceType, args);
		}
	}
}
