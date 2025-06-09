# AfterWire Dependency Injection Extensions

The `AfterWire` library provides a set of extensions for `IServiceCollection` to simplify the registration of services that can happen after the ServiceProvider is built. It integrates seamlessly with `Microsoft.Extensions.DependencyInjection`.

## Features

- **Dynamic Service Registration**: Register services dynamically after ServiceProvider is already created.
- **Factories**: Allows for multiple object instantiation of registered classes using generics.  
- **Keyed Services**: Support for keyed service registration and resolution.
- **Scoped, Transient, and Singleton Lifetimes**: Easily register services with different lifetimes.

## Installation

Add the `AfterWire` Nuget to your solution and reference it in your application.

## Usage

### Adding AfterWire to Your Service Collection

To use `AfterWire`, call the `AddAfterWire` extension method on your `IServiceCollection`:

```csharp
IServiceCollection services = new ServiceCollection();
services.AddAfterWire();
```

### Registering Services 


#### Registering before ServiceProvider is built.

```csharp
services.AddAfterWireTransient<IEmployee, Employee>();
```

#### Registering after ServiceProvider is built.

```csharp
IAfterWireServiceProvider afterWireServiceProvider = serviceProvider.GetRequiredService<IAfterWireServiceProvider>();
afterWireServiceProvider.AddTransient<IShift, ShiftZero>("0");
afterWireServiceProvider.AddTransient<IShift, ShiftRegular>("1");
```

### Resolving Services

Use the `IAfterWireServiceProvider` or `IAfterWireFactory<T>` to resolve services, including keyed services:

```csharp
IEmployee employee = serviceProvider.GetRequiredService<IEmployee>();

//Using Factory
IAfterWireFactory<IShift> shiftFactory = serviceProvider.GetRequiredService<IAfterWireFactory<IShift>>();
IShift shift = shiftFactory.GetKeyedRequiredService("1");
```

### Factories

IAfterWireFactory is a useful way in code to create new instances of objects in places where you are creating objects based on a collection or user interaction.  

One large advantage of using IAfterWireFactory<T> is that you can avoid the Service Locator anti-pattern and also have your constructor show the true dependencies.


```csharp
//Using Factory
IAfterWireFactory<IShift> shiftFactory = serviceProvider.GetRequiredService<IAfterWireFactory<IShift>>();
IShift shift = shiftFactory.GetKeyedRequiredService("1"); //1 is the shift.  This could be something entered in by the user
```

### Full Example

Here is a complete example from the `AfterWireSamples.Program.cs` file:

```csharp
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
```

## License

This project is licensed under the MIT License.  