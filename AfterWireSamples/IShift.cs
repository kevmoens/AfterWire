using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AfterWireSamples
{
	public interface IShift
	{
		string Shift { get; set; }
		TimeOnly StartTime { get; set; }
		TimeOnly EndTime { get; set; }
		Decimal Hours { get; set; }
		Decimal Grace { get; set; }
	}
}
