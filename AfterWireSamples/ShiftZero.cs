using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AfterWireSamples
{
	public class ShiftZero : IShift
	{
		public string Shift { get; set; } = "0";
		public TimeOnly StartTime { get; set; } = TimeOnly.MinValue;
		public TimeOnly EndTime { get; set; } = TimeOnly.MinValue;
		public decimal Hours { get; set; } = 0;
		public decimal Grace { get; set; } = 0.25m;
	}
}
