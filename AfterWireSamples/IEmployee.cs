using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AfterWireSamples
{
	public interface IEmployee
	{
		string Name { get; set; }
		IShift Shift { get; set; }
	}
}
