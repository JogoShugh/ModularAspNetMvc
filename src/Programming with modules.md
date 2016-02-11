# What is a module?

Before we get into conceptual detail on topics like Test-Driven Development, Behavior-Driven Development, 
or any of the ideas from Domain-Driven Design and the latest hotness, Command and Query Responsibility Segregation 
(CQRS), we need some lessons on modularity and its evil, vile doppleganger monolithity (yes, I made that up). 

Monolithity masquerades as a convenient, fast way to build applications that purports to make people happy by
reducing the amount of time it takes to get something, anything, "shipped". This usually comes at the expense of
manageability and extensibility. Those prices are too high to pay for important systems.

To start, here's a great definition of a [modular programming](http://en.wikipedia.org/wiki/Modular_programming) 
comes from Wikipedia:

> Modular programming (also called "top-down design" and "stepwise refinement") is a software design technique
> that emphasizes separating the functionality of a program into independent, interchangeable modules,
> such that each contains everything necessary to execute only one aspect of the desired functionality.
> Conceptually, modules represent a separation of concerns, and improve maintainability by enforcing logical
> boundaries between components. Modules are typically incorporated into the program through interfaces.
> A module interface expresses the elements that are provided and required by the module.
> The elements defined in the interface are detectable by other modules. The implementation contains the
> working code that corresponds to the elements declared in the interface.

This is a wonderful working definition of modular programming! Although there is a lot to it, we will break it 
down by using example code very soon. But, first let's see the definition of modularity's evil twin, the 
[monolith](http://en.wikipedia.org/wiki/Monolithic_system):

TODO: link: https://creately.com/app/?tempID=h165rwt81#

> A software system is called "monolithic" if it has a monolithic architecture, in which functionally 
> distinguishable aspects (for example data input and output, data processing, error handling, and the user 
> interface), are not architecturally separate components but are all interwoven.

Here's a typical depiction of what a monolithic application looks like at the package level. Typically, each of the 
horizontal packages depicted here would be compiled into a DLL, resulting in 3 separate DLLs. Some people will say 
that this is "good design", and is based on the layers pattern. 

TODO: add image
![MonolithicApplication.png]

## Exercise 01: Monolithic Calculator TODO fix this

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

## Separating functionality of a program into independent, interchangeable modules


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

Here are the files to create for this solution:

## Calculator.cs

```csharp
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Reflection;

namespace MEFCalculator
{
	public class Calculator
	{
		public Calculator()
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
			var catalog = new DirectoryCatalog(
				@"C:\Projects\github\ModularAspNetMvc\Chapters\Modularity\MEFCalculator.SeparateFiles\Modules.Deploy");
			var container = new CompositionContainer(catalog);
			container.SatisfyImportsOnce(compositionTarget);
		}
	}
}

```

## IOperation.cs

```csharp
namespace MEFCalculator
{
	public interface IOperation
	{
		decimal Execute(params decimal[] args);
	}
}
```

## Modules\Add\Add.cs

```csharp
using System.ComponentModel.Composition;

namespace MEFCalculator
{
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
}
```
## Modules\Subtract\Subtract.cs

```csharp
using System.ComponentModel.Composition;

namespace MEFCalculator
{
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
}
```

## MEFCalculatorTests.MEFCalculatorTest.cs

```csharp
using NUnit.Framework;
using MEFCalculator;

namespace MEFCalculatorTests
{
	[TestFixture()]
	public class MEFCalculatorSeparateFilesTests
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
}
```

As you can see, all we have done in this iteration is to shufle the code around into separate files and into separate 
physical projects. So, what's an immediate benefit for this approach? The first one that comes to my mind is agile development, 
with its heavy focus on delivering business value rapidly and with lots of stakeholder feedback through iterative cycles. The 
reason for this is that you can now develop the math operations in isolation from each other. Thus, you can have different 
developers working on each, if you need to do that, without stepping on each other's work at all.

Of course, you'd be right to note that we still must add a new wrapper method to the  `Calculator` class every time we add 
a new operation. This is true, but only because we are relying on C#'s static features such that you can have strong code 
completion in an IDE. Essentially, the `Calculator` class is a thin facade on top of our very independent math operation 
classes. Let's do a new refactoring that, while removing our strongly-typed facade methods, will afford us the ability to 
add additional implementations of the `IOperation` interface into the `Modules.Deploy` folder, and then invoke them, without 
needing to modify the `Calculator` class at all.

# Using [ImportMany] to pull in multiple exports

I'll only show code for clasess that change from the previous iteration.

## Modules\Add\Add.cs

```csharp
using System.ComponentModel.Composition;

namespace MEFCalculator
{
	[Export(typeof(IOperation))]
	public class Add : IOperation {
		public decimal Execute(params decimal[] args) {
			decimal result = 0M;
			for(var i = 0; i < args.Length; i++) {
				result += args[i];
			}
			return result;
		}
	}
}
```

## Modules\Subtract\Subtract.cs

```csharp
using System.ComponentModel.Composition;

namespace MEFCalculator
{
	[Export(typeof(IOperation))]
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
}
```
## Calculator.Dynamic\Calculator.cs

First, note we have an `[ImportMany]` attribute on top of the `List<IOperation> _operations` member variable. This tells 
MEF to pull in **all** instances of `IOperation` in its run-time catalog. Then, wee've add a method named 
`Execute` to the `Calculator` class itself, which takes a string name for the operation, and 
the array of decimal arguments. Using LINQ, we look it up by name in our `_operations` list, and then invoke it! That's all 
it takes. Given this, can you see how we could add new DLLs for operations like divid, multiply, power, etc?

```csharp
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

namespace MEFCalculator
{
	public class Calculator
	{
		public Calculator()
		{
			CompositionHelper.ComposeParts(this);
		}
		
		[ImportMany]
		private List<IOperation> _operations;

		public decimal Execute(string operationName, params decimal[] args) {
			IOperation operation = _operations.FirstOrDefault
				(x => x.GetType().Name.ToLower().Equals(operationName.ToLower(), StringComparison.OrdinalIgnoreCase));
			if (operation != null) {
				return operation.Execute (args);
			}
			return 0M;
		}			
	}
	
	public static class CompositionHelper 
	{
		public static void ComposeParts(object compositionTarget) {
			var catalog = new DirectoryCatalog(
				@"C:\Projects\github\ModularAspNetMvc\Chapters\Modularity\MEFCalculator.DynamicInvoke\Modules.Deploy");
			var container = new CompositionContainer(catalog);
			container.SatisfyImportsOnce(compositionTarget);
		}
	}
}
```

# Adding a console interface for our calculator

Now that we've achieved a number of important modularity goals, let's put a more interactive user interface on top of our 
calculator. This is a very rudimentary and simple interface, but one that will carry us into two more example user interfaces 
that will make more sense (mobile web, and desktop).

## Add ExecuteScript method to Calculator

It's certainly debatable whether we should but the following method directly on the `Calculator` class or do it elsewhere, but 
for illustration purposes, we'll do it on the `Calculator`. But, first, the new test case in our test class:

This test specifies that we can pass a string, separated by new-line characters and containing one command per line, into 
the calcuator and it will produce an array of results.

```csharp
		[Test]
		public void runs_script_with_multiple_operations()
		{
			const string mathScript =
@"Add 6 7 8
Subtract 20 15 1
Add 1 2 4
";
			var results = _subject.ExecuteScript(mathScript).ToList();
			
			Assert.AreEqual(21, results[0]);
			Assert.AreEqual(4, results[1]);
			Assert.AreEqual(7, results[2]);
		}
	}
}
```

Now, add a new C# console application project named `MathConsole` with this code in a file called `MathConsole.cs` 
(or the Main.cs file):

```csharp
using System;
using MEFCalculator;
using System.Linq;
using System.Collections.Generic;

namespace MathConsole
{
	public static class MathConsole
	{
		public static void Main(string[] args)
		{
			var calc = new Calculator();
			var line = string.Empty;

			WriteMessage();
			while ((line = Console.ReadLine()) != "exit") {
				var result = calc.ExecuteScript(line).FirstOrDefault();
				if (result != Decimal.MinValue) {
					Console.WriteLine("Result = " + result);
				}
				WriteMessage();
			}
		}

		private static void WriteMessage() {
			Console.WriteLine("Type a math expression in the form of: Add 1 2 3 or Subtract 20 15 1");
		}
	}
}

```

This class is deliberately simple, and not very robust. That's fine for now. Set the `MathConsole` as the solution's 
start-up project, and run it. You can now type in `Add 5 6 7` or `Subtract 20 9 2`, and so forth.

# Relocating test cases to their appropriate module directory

TODO

# Simulating dependencies with stub  or mock objects for the `Calculator` test

TODO -- consider AutoMock, FakeItEasy. Also, prefer creating the instances by hand as simple implementations of IOperation. 
This will force a refactoring of Calculator, in that it currently has a hard-coded DirectoryCatalog.

# Reusing the Calculator in a simple web service

In ASP.NET we can create a very simple web service, implemented within an IHttpModule-implementing class, by using this code:

```csharp
using System;
using System.Web;
using MEFCalculator;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MathRESTService
{
	public class MathHttpModule : IHttpModule
	{
		private static Calculator _calculator = new Calculator();
		
		public void Init(HttpApplication app) {
			app.BeginRequest += (object sender, EventArgs e) => {
				var query = app.Request.Url.Query;
				if (string.IsNullOrWhiteSpace(query)) {
					app.Response.Write("Please specify a query");
					app.Response.End();
				}
				else {
					query = query.Replace(',', ' ');
					query = query.Replace(';', '\n');
					query = query.Substring(1);

					var results = _calculator.ExecuteScript(query);
					var buffer = new StringBuilder();
					foreach(var result in results) {
						buffer.AppendLine(result.ToString());
					}

					app.Response.Write(buffer.ToString());
					app.Response.End();
				}
			};
		}

		public void Dispose() {
		}
	}
}
```

There are a couple of things to notice in this:

* We replace `'` with a space, this makes it easier to pass the parameters in the URL query string, since spaces get translated
to ugly `%20` escaped sequences
* We replace `;` with a new line, since our `Calculator.ExecuteScript` method expects inputs to be separated by new lines

Because of this, we can have the service process multiple calculations in a single request, which we build up over a loop that 
writes to a `StringBuilder` instance.

# Create a jQuery Mobile based mobile web app for the calculator

Now that we have a functioning web service that lets us calculate results, how about we put a better looking web interface on 
top of it? We'll use the popular, open source jQuery Mobile library to do this. We're only going to use a very small subset of 
jQuery Mobile's features at this time, but we will get much more sophisticated later on in the book with it.

jQuery Mobile is quite easy to get started with. But, before we use it, it would be helpful to us if we could query our web 
service for a list of all the operations it supports, so that we don't have to have a static user interface, but rather can 
dynamically build it based upon however many operations exist. 

## Adding metadata to our calculator and web service

First, let's modify our unit test class for the calculator itself:

```csharp
[Test]
public void returns_operations_list() 
{
	var expectedOperations = new List<string>();
	expectedOperations.Add("Add");
	expectedOperations.Add("Subtract");

	var actualOperations = _subject.GetOperations();

	CollectionAssert.AreEquivalent(expectedOperations, actualOperations);
}
```

Now, implement the `GetOperations` method on the `Calculator:

```csharp
public IList<string> GetOperations() {
	return _operations.Select(x => x.GetType().Name).ToList();
}
```

Finally, modify the web service class's `Init` method to look like this:

```csharp
public void Init(HttpApplication app) {
	app.BeginRequest += (object sender, EventArgs e) => {
		var query = app.Request.Url.Query;
		if (string.IsNullOrWhiteSpace(query)) {
			var buffer = new StringBuilder();
			var operations = _calculator.GetOperations();
			foreach (var operationName in operations) {
				buffer.AppendLine(operationName);
			}
			app.Response.Write(buffer.ToString());
			app.Response.End();
		}
		else {
			query = query.Replace(',', ' ');
			query = query.Replace(';', '\n');
			query = query.Substring(1);

			var results = _calculator.ExecuteScript(query);
			var buffer = new StringBuilder();
			foreach(var result in results) {
				buffer.AppendLine(result.ToString());
			}

			app.Response.Write(buffer.ToString());
			app.Response.End();
		}
	};
}
```
Now, when your browse the web server without passing any parameters on the query string, you get a simple new-line 
delimited list of the named operations. Pretty simple.

Open the `MathHttpModule` class again, and modify the `Init` method so that it now looks like this:


# Outline for remainder

* Iteration: Dynamically add and invoke operations from a command-line script interface, interactively
* Iteration: Move test cases for each operation into the solution folder for that operation, not the Calculator class
* Iteration: Show piping of operations with a left->right precedence
* Iteration: Create a web-facade on top of the operations so that they can be invoked as a "web service", and retain 
capability for operations to be dynamically deployed into the bin folder as separate DLLs. 
* Iteration: Create a desktop, WPF app on top of the operations, supporting "plugins"
* Iteration: Show how to add plugins using IronPythonMEF




