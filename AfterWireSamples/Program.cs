using AfterWire;
using AfterWireSamples;
using Microsoft.Extensions.DependencyInjection;

IServiceCollection services = new ServiceCollection();

services.AddAfterWire();
services.AddAfterWireTransient<IEmployee, Employee>();

IServiceProvider serviceProvider = services.BuildServiceProvider();

IAfterWireServiceProvider afterWireServiceProvider = serviceProvider.GetRequiredService<IAfterWireServiceProvider>();
IAfterWireFactory<IShift> shiftFactory = serviceProvider.GetRequiredService<IAfterWireFactory<IShift>>();

afterWireServiceProvider.AddTransient<IShift, ShiftZero>("0");
afterWireServiceProvider.AddTransient<IShift, ShiftRegular>("1");

IEmployee test = serviceProvider.GetRequiredService<IEmployee>();

test.Name = "Kevin";

Console.WriteLine(shiftFactory.GetKeyedRequiredService("1").StartTime);


Console.WriteLine(test.Name);

