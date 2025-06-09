using System;
using System.Collections.Generic;
using System.Text;

namespace AfterWire
{

	public interface IAfterWireFactory<T>
	{
		T GetService();
		T GetRequiredService();
		T GetKeyedService(object key);
		T GetKeyedRequiredService(object key);
	}
}
