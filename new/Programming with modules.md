# What is a module?

Before we even start talking about concepts like Test-Driven Development, Behavior-Driven Development, 
or any of the ideas from Domain-Driven Design and the latest hotness, Command and Query Responsibility Segregation 
(CQRS), we need some lessons on modularity.

A great definition of a [modular programming](http://en.wikipedia.org/wiki/Modular_programming) 
comes from Wikipedia:

> Modular programming (also called "top-down design" and "stepwise refinement") is a software design technique
> that emphasizes separating the functionality of a program into independent, interchangeable modules,
> such that each contains everything necessary to execute only one aspect of the desired functionality.
> Conceptually, modules represent a separation of concerns, and improve maintainability by enforcing logical
> boundaries between components. Modules are typically incorporated into the program through interfaces.
> A module interface expresses the elements that are provided and required by the module.
> The elements defined in the interface are detectable by other modules. The implementation contains the
> working code that corresponds to the elements declared in the interface.

This is a wonderful working definition. Although there is a lot to it, we will break it down by using example
code, without delay.

## Separating functionality of a program into independent, interchangeable modules

Let's start with the canonical calculator example, in C#. We start with an NUnit test project that defined two 
test cases, one for adding decimals, and one for subtracting numbers from a starting number. Our subject is the 
`Calculator` class itself, and the test name describes what the operation should do. Following the test cases, we 
add the implementation of the calculator to make the tests pass.

TODO: instruct how to use MonoDevelop on Windows, etc

```csharp
using NUnit.Framework;
using System;

namespace Modularity
{
  [TestFixture()]
	public class CalculatorTests
	{
		private Calculator _subject = new Calculator();
		
		[Test()]
		public void add_sums_three_numbers()
		{
			var result = _subject.Add (5.5M, 6M, 7M);
			
			Assert.AreEqual (18.5, result);
		}
		
		[Test()]
		public void subtract_removes_two_numbers_from_first()
		{
			var result = _subject.Subtract (10M, 5M, 3M);
			
			Assert.AreEqual (2M, result);
		}
	}

	public class Calculator 
	{
		public decimal Add(params decimal[] args) 
		{
			decimal result = 0M;
			for(var i = 0; i < args.Length; i++) {
				result += args[i];
			}
			return result;
		}

		public decimal Subtract(params decimal[] args)
		{
			decimal result = 0M;
			if (args.Length > 0)
			{
				result = args[0];
				if (args.Length > 1) {
					for(var i = 1; i < args.Length; i++) {
						result = result - args[i];
					}
				}
			}
			return result;
		}
	}
}
```

This is all simple enough, and you might say that we could add all sorts of mathematical methods to this 
`Calculator` class and end up with something like the calculator you have on your iPhone or Android, or the one 
built into your operating system. We could add methods for `Multiple`, `Divide`, `Power`, etc.

## Adding too many methods will lead to a monolithic, beastly application

But, there is a big problem with this way of building a calculator, or any application. It creates tight-coupling and
monolithic applications. Here's an excerpt from the Wikipedia definition of a [Monolothic application]
(http://en.wikipedia.org/wiki/Monolithic_application):

> In software engineering, a monolithic application describes a software application which is designed without
> modularity. Modularity is desirable, in general, as it supports reuse of parts of the application logic and
> also facilitates maintenance by allowing repair or replacement of parts of the application without requiring
> wholesale replacement.

What if you had more sophisiticated mathematical operations to implement, such as scientific algorithms or 
statistical functions? And, what if you had more than one person working on the system? That's almost a foregone 
conclusion for any system of appreciable size and importance. What we need actually achieve the `independent, 
interchangeable modules` part of the definition from above. Let's do that.

# Modules are typically incorporated into the program through interfaces

To achieve `independdent, interchangeable modules`, we will incorporate another part of the definition, where it 
states `Modules are typically incorporated into the program through interfaces`.

Do you seem something in common for both the `Add` and `Subtract` method signatures? Here they are again:

* Add: `public decimal Add(params decimal[] args)`
* Subtract: `Subtract(params decimal[] args)`

Both of them take an array of decimal arguments. The C# keyword `params` specifies that this method can be called 
with any number of decimal arguments.

## Define an Operation interface to encapsulate mathematical operations

When we talk about mathematics, we usually refer to signs like `+`, `-`, `/`, and `*` as operators, or operations.

Let's create a C# interface to capture this language:

```csharp
public interface IOperation
{
  object Execute(params decimal[] args);
}
```

Now we can create small, independent modules for both `Add` and `Subtract` that implement this common interface. 
Recall the definition of a module again when it says: 
`The implementation contains the working code that corresponds to the elements declared in the interface`

Here's the refactored code, implemented in a separate file called `InterfaceBasedCalculatorTests.cs`:

```csharp
using NUnit.Framework;
using System;

namespace Modularity.InterfaceBasedCalculator
{
  [TestFixture()]
	public class InterfaceBasedCalculatorTests
	{
		private Calculator _subject = new Calculator();
		
		[Test()]
		public void add_sums_three_numbers()
		{
			var result = _subject.Add (5.5M, 6M, 7M);
			
			Assert.AreEqual (18.5, result);
		}
		
		[Test()]
		public void subtract_removes_two_numbers_from_first()
		{
			var result = _subject.Subtract (10M, 5M, 3M);
			
			Assert.AreEqual (2M, result);
		}
	}

	public interface IOperation
	{
		decimal Execute(params decimal[] args);
	}

	public class Add : IOperation {
		public decimal Execute(params decimal[] args) {
			decimal result = 0M;
			for(var i = 0; i < args.Length; i++) {
				result += args[i];
			}
			return result;
		}
	}

	public class Subtract : IOperation {
		public decimal Execute(params decimal[] args) {
			decimal result = 0M;
			if (args.Length > 0)
			{
				result = args[0];
				if (args.Length > 1) {
					for(var i = 1; i < args.Length; i++) {
						result = result - args[i];
					}
				}
			}
			return result;
		}
	}

	public class Calculator 
	{
		public decimal Add(params decimal[] args) 
		{
			return new Add ().Execute (args);
		}

		public decimal Subtract(params decimal[] args)
		{
			return new Subtract ().Execute (args);
		}
	}
}
```

Notice that we have introduced the interface, `IOperation`, and then implemented it twice in classes named 
`Add`, and `Subtract`. Our `Calculator` class now instantiates these classes and delegates to their functionality 
to complete the task. Notice that our test cases remain identical.

You might be thinking that this was a lot of extra lines of code to achieve negligible benefits. I agree that there 
is not great benefits for a two-method calculator. But, again, remember what this would be like if we had 10, 25, 
or 50 mathematical operations. It would get very ugly if had all the code for each operation in a single
 `Calculator` class.
 
Still, at this point all we have done is to move code into separate files to reduce the chance of people stepping 
on each other or causing merge conflicts when checking in the `Calculator.cs` file. And, the definition of 
modular programming also said this: `The elements defined in the interface are detectable by other modules.`

While you could say that the `Calculator` class "detects" the operations, that would be twisting the truth. In 
reality, the `Calculator` class takes a hard-dependency on each operation class, even though each class is 
interchangeable, through its `IOperation` interface. In fact, there's nothing preventing us from incorrectly 
instantiating `Add` inside of `Calculator.Subtract`, or vice-versa.

# Loosening up that starchy, static Calculator class

Microsoft added the Managed Extensibility Framework (MEF) to the .NET Framework in version 4.0. MEF is desinged to 
allow you to write more modular, extensible applications with small, loosely-coupled parts that get "composed" at 
run-time to form a complete application. Let's use it now.

All you need to understand, for the moment, is that MEF utilizes two simple concepts: Import and Export. We will
make all of our IOperation implementations Exports, and we will make the Calculator class Import those Exports.

Here's the code:

```csharp
using NUnit.Framework;
using System;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Reflection;

namespace Modularity.MEFCalculator
{
	[TestFixture()]
	public class MEFCalculatorTests
	{
		private Calculator _subject = new Calculator();
		
		[Test()]
		public void add_sums_three_numbers()
		{
			var result = _subject.Add (5.5M, 6M, 7M);
			
			Assert.AreEqual (18.5, result);
		}
		
		[Test()]
		public void subtract_removes_two_numbers_from_first()
		{
			var result = _subject.Subtract (10M, 5M, 3M);
			
			Assert.AreEqual (2M, result);
		}
	}

	public interface IOperation
	{
		decimal Execute(params decimal[] args);
	}

	[Export]
	public class Add : IOperation {
		public decimal Execute(params decimal[] args) {
			decimal result = 0M;
			for(var i = 0; i < args.Length; i++) {
				result += args[i];
			}
			return result;
		}
	}

	[Export]
	public class Subtract : IOperation {
		public decimal Execute(params decimal[] args) {
			decimal result = 0M;
			if (args.Length > 0)
			{
				result = args[0];
				if (args.Length > 1) {
					for(var i = 1; i < args.Length; i++) {
						result = result - args[i];
					}
				}
			}
			return result;
		}
	}

	public class Calculator
	{
		public Calculator ()
		{
			CompositionHelper.ComposeParts (this);
		}

		[Import]
		private Add _add;
		public decimal Add(params decimal[] args) 
		{
			return _add.Execute (args);
		}

		[Import]
		private Subtract _subtract;
		public decimal Subtract(params decimal[] args)
		{
			return _subtract.Execute (args);
		}
	}

	public static class CompositionHelper 
	{
		public static void ComposeParts(object compositionTarget) {
			var catalog = new AssemblyCatalog (Assembly.GetExecutingAssembly ());
			var container = new CompositionContainer (catalog);
			container.SatisfyImportsOnce(compositionTarget);
		}
	}
}
```

It appears we are adding more and more code! That's true, for now. But, notice the `[Export]` attributes on the 
`Add` and `Subtract` classes and the `[Import]` attributes on top of the private fields above the `Add` and 
`Subtract` methods in the `Calculator` class. These attribute pairs are what enable the 
`CompositionHelper.ComposeParts` method to inject the `Calculator` class with instances of those classes.

Yet, we still have not achieved a true decoupling, have we? The `Calculator` class still has hard dependencies upon the 
`Add` and `Subtract` concrete implementations! Let's change that now.

# Truer decoupling with named imports

This time, we'll modify the private class fields to be of type `IOperation`, the interface that both `Add` and `Subtract` 
implement. This takes us one step closer to true decoupling -- something we'll keep working toward in successive iterations!

```csharp
using NUnit.Framework;
using System;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Reflection;

namespace Modularity.MEFCalculatorNamedExports
{
	[TestFixture()]
	public class MEFCalculatorNamedExportsTests
	{
		private Calculator _subject = new Calculator();
		
		[Test()]
		public void add_sums_three_numbers()
		{
			var result = _subject.Add (5.5M, 6M, 7M);
			
			Assert.AreEqual (18.5, result);
		}
		
		[Test()]
		public void subtract_removes_two_numbers_from_first()
		{
			var result = _subject.Subtract (10M, 5M, 3M);
			
			Assert.AreEqual (2M, result);
		}
	}

	public interface IOperation
	{
		decimal Execute(params decimal[] args);
	}

	[Export("Add", typeof(IOperation))]
	public class Add : IOperation {
		public decimal Execute(params decimal[] args) {
			decimal result = 0M;
			for(var i = 0; i < args.Length; i++) {
				result += args[i];
			}
			return result;
		}
	}

	[Export("Subtract", typeof(IOperation))]
	public class Subtract : IOperation {
		public decimal Execute(params decimal[] args) {
			decimal result = 0M;
			if (args.Length > 0)
			{
				result = args[0];
				if (args.Length > 1) {
					for(var i = 1; i < args.Length; i++) {
						result = result - args[i];
					}
				}
			}
			return result;
		}
	}

	public class Calculator
	{
		public Calculator ()
		{
			CompositionHelper.ComposeParts (this);
		}

		[Import("Add")]
		private IOperation _add;
		public decimal Add(params decimal[] args) 
		{
			return _add.Execute (args);
		}

		[Import("Subtract")]
		private IOperation _subtract;
		public decimal Subtract(params decimal[] args)
		{
			return _subtract.Execute (args);
		}
	}

	public static class CompositionHelper 
	{
		public static void ComposeParts(object compositionTarget) {
			var catalog = new AssemblyCatalog (Assembly.GetExecutingAssembly ());
			var container = new CompositionContainer (catalog);
			container.SatisfyImportsOnce(compositionTarget);
		}
	}
}
```

# Segregating the modules into separate deployment packages -- DLLs

In successive refactorings, we've achieved the following so far:

* Moved the logic from one big class into smaller, discrete classes, each responsible for calculating one specific 
mathematical operation.
* Generalized the concept of the calculations into the `IOperation` interface, with its one method signature that takes 
an array of decimals, and returns a decimal result.
* Decoupled the `Calculator` from the specific `Add` and `Subtract` implementations, making it depend only upon the 
`IOperation` interface.
* Used the Managed Extensibility Framework (MEF) with its `[Import]` and `[Export]` attributes to dynamically 
compose the `Calculator` instance at run-time, injecting it with the concrete `Add` and `Subtract` implementations.

Yet, we are just playing around with concepts if we never broach the subject of **physical separation of classes** from each 
other, and ultimately of their deployment. Let's get started!

Before showing the code, let's look at the folder structure:

```text
+---Calculator
|       Calculator.cs
|          
+---Interfaces
|       IOperation.cs      
|               
+---MEFCalculator.Tests
|       MEFCalculatorTests.cs
|           
+---Modules
|   +---Add
|   |       Add.cs
|   |      
|   \---Subtract
|           Subtract.cs
|                   
\---Modules.Deploy
        Add.dll
        Interfaces.dll
        Subtract.dll
```

The `Calculator` itself is in its own project, under the Calculator folder. The interface for the operations is in its own 
assembly, and then we have a `Modules` folder that contains separate projects for `Add` and `Subjract`. We also have a 
folder named `Modules.Deploy`, which is where the `Add` and `Subtract` DLLs get copied to after building. In our examples 
thus far, we've used MEF's `AssemblyCatalog`, but we're about to use the `DirectoryCatalog`, which lets us pull in 
DLLs from a folder into the composition process. That's how we can compose the `Calculator` instance with types culled from 
multiple assemblies with ease -- and just a couple of lines of code.















