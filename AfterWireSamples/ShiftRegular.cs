using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AfterWireSamples
{
	public class ShiftRegular : IShift
	{
		public string Shift { get; set; } = "1";
		public TimeOnly StartTime { get; set; } = TimeOnly.FromTimeSpan(new TimeSpan(8, 0, 0)); // 8:00 AM
		public TimeOnly EndTime { get; set; } = TimeOnly.FromTimeSpan(new TimeSpan(17, 0, 0)); // 5:00 PM
		public decimal Hours { get; set; } = 8; // 8 hours
		public decimal Grace { get; set; } = 0.25m; // 15 minutes grace period
	}
}
