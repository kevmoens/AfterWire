using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AfterWireSamples
{
	public class Employee([FromKeyedServices("0")] IShift shift) : IEmployee
	{

		public string Name { get; set; } = string.Empty;
		public IShift Shift { get; set; } = shift;

	}
}
