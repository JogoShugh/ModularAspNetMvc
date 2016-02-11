# ModularAspNetMvc

This is a work in progress of a modular architecture for ASP.NET MVC, utilizing the awesome MvcCodeRouting NuGet package and MEF, the Managed Extensibility Framework from `System.ComponentModel.Composition`.

# High level summary

The application is comprised of a few major concepts:

## Core.Infrastructure 

Contains the stable, non-volatile, components that all other modules take a reference too. It contains Boot, Composition, Eventing, Security, and baseline Ui classes

## Modules

Contains three sample modules: Authentication, StandardReport, UserProfile. More about the structure of a module below.

# Modules.Deploy

The binaries for modules get placed into this folder

# Core.Modules.Deploy

Simple file copy utility that enumerates the contents of the `Modules.Deploy` folder and copies the binaries into the
web app's bin folder for runtime execution.

# Core.Ui.Mvc

Contains the application shell which bootstraps the module loading process. After modules are loaded, then there are actual
MVC routes ready to respond to requests, such as the default of `Authentication/Login`.

# How do modules get loaded into the web app?

Each module contains all code that it needs. That is to say, each module is a **vertical slice**, and is fully
*shippable* in the agile sense. Architectually, it is fully *separate* from the main application.

The main application's job is to compose and execute **modules**, not to be a monolithic beast that programmers must
"fork and modify", thus impacting the lives of all other module authors and users when they merge the code.

Here's how ASP.NET loads a module:

* Before running the web site, we run `Core.Modules.Deploy.exe`. This copies all the modules' DLLs into the `Core.Ui.Mvc\bin` folder.
* Next, in `Global`, we have this code:

```csharp
public void RegisterRoutes(RouteCollection routes)        
{            
    routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

    // NOTE: Take this out because MvcCodeRouting is doing our work now...
    //routes.MapRoute(
    //    "Default", // Route name
    //    "{controller}/{action}/{id}", // URL with parameters
    //    new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
    //);

    ViewEngines.Engines.EnableCodeRouting();

    new BootStrapper().Boot();
}
```

What's happening here is like your standard MVC template, but instead of relying on the default route, we delegate 
to the very awesome MvcCodeRouting NuGet package. This will take care of adding routes into the main route table 
for us. We'll see how in a bit.

Here's the next bit:

```csharp
public class BootStrapper
{
    public void Boot()
    {
        var moduleLoader = new ModuleLoader();
        moduleLoader.LoadAllModules();

        var messageHandlerWiring = new MessageHandlerWiring();
        messageHandlerWiring.WireMessageHandlers(MessageProcessor.Instance);

        var eventSubscriberWiring = new EventSubscriberWiring();
        eventSubscriberWiring.WireEventListeners(EventAggregator.Instance);
    }
}
```

Let's look only at the ModuleLoader for now. Here's what it does:

```csharp
public class ModuleLoader
{
    public void LoadAllModules()
    {
        new PartsList<IModule>(module => module.Initialize(this));
    }

    public void MapCodeRoutes(string baseRoute, Type rootControllerType)
    {
        var routes = RouteTable.Routes;

        routes.MapCodeRoutes(
            baseRoute: baseRoute,
            rootController: rootControllerType,
            settings: new CodeRoutingSettings
                          {
                              UseImplicitIdToken = true,
                              EnableEmbeddedViews = true
                          });
    }
}
```

That's not a lot of code, but it actually does everything we need to get our routes added, and our views dynamically
pulled out of the assemblies contained in the modules!

The `PartsList` generic is a convenience on top of MEF, which basically tells MEF to scan the `bin` folder for all
instances of IModule, instantiate it, and then call its `Initialize` method, passing in the `ModuleLoader` instance itself.

Further down, we have the `MapCodeRoutes` method. This utilizes from extension methods that MvcCodeRouting provides. And, it
basically is telling MvcCodeRouting to setup default routes to use an implicit Id, like `route/{id}`, and that our views are 
actually embedded inside of module assemblies, not located in physical folders in a monolithic "Web" project (grrrrrrr!)

## The Authentication module's Module.cs file

What's in an instance of IModule? Not much:

```csharp
[Export(typeof(IModule))]
public class Module : DefaultModule<AuthenticationController>
{
    protected override string GetBaseControllerRoute()
    {
        return "Authentication";
    }
}
```

Basically, this one just tells the framework that its route name is "Authentication". Perhaps we could get away without 
this at all if we used a convention that said route names are the same as the module name.

The base class, `DefaultModule<AuthenticationController>` is telling the framework that the default controller will 
be the `AuthenticationController`, and the rest of the class does this:

```csharp
namespace Core.Infrastructure.Ui.Mvc.Modularity
{
    [Export(typeof(IModule))]
    public abstract class DefaultModule<TBaseControllerType> : IModule
    {
        protected abstract string GetBaseControllerRoute();

        public void Initialize(ModuleLoader moduleLoader)
        {
            var baseControllerRoute = GetBaseControllerRoute();
            moduleLoader.MapCodeRoutes(baseControllerRoute, typeof(TBaseControllerType));
        }
    }
}
```

So, you see that the `Initialize` call we saw in the `ModuleLoader` gets called, and then the route is fetched, and
the routes get mapped.

## AuthenticationController implementation

There's not much! Just a redirect:

```csharp
namespace Core.Modules.Authentication.Ui.Mvc.Controllers
{
    public class AuthenticationController : Controller
    {
        //
        // GET: /Authentication/

        public ActionResult Index()
        {
            return Redirect("Login");
        }

    }
}
```

Ok, so here's LoginController:

```csharp
namespace Core.Modules.Authentication.Ui.Mvc.Controllers.Login
{
    public class LoginController : DefaultController
    {
        public ActionResult Index()
        {
            var model = new LoginSubmitViewModel();
            model.CommandActions.Add("Login", "Submit");
            model.CommandActions.Add("Cancel", "Index");

            return View(model);
        }

        [HttpPost]
        [PostAction]
        public ActionResult Submit(LoginSubmitViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var command = new LoginSubmit
                                  {
                                      Password = viewModel.Password,
                                      UserName = viewModel.UserName
                                  };
                var response = ProcessCommand<TypedMessageResponse<string>>(command);

                return Content(response.Data);
            }

            return Content("TODO: handle errors properly here.");
        }
    }
}
```

Looks like a lot is going on here,  but with very little code. That's true.

First, an HTTP GET will invoke the `Index` method, which news up a `LoginSubmitViewModel` and adds a couple of 
actions to it. That class looks like this:

```csharp
namespace Core.Modules.Authentication.Ui.Mvc.ViewModels
{
    public class LoginSubmitViewModel
    {
        public LoginSubmitViewModel()
        {
            CommandActions = new CommandActionCollection();
        }

        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }

        [ScaffoldColumn(false)]
        public CommandActionCollection CommandActions { get; set; }
    }
}
```

Notice we use some simple data annotations and also specify that our `CommandActions` collection is not to be scaffolded.

## The Login View

Now, this part is pretty simple:

```razor
@using System.Web.Mvc.Html
@model Core.Modules.Authentication.Ui.Mvc.ViewModels.LoginSubmitViewModel
@{
    Layout = "~/Views/Shared/_Layout.cshtml";
    ViewBag.Title = "Login";
}

@using (Ajax.BeginForm("Index", "Authentication/Login", new AjaxOptions 
{ InsertionMode = InsertionMode.Replace, HttpMethod = "Post", UpdateTargetId = "Results"}, new { id = "LoginForm" }))
{
@Html.ValidationSummary("The following errors were found:", new { id = "ValidationSummary" })
<fieldset>
    <legend>Login</legend>
    @Html.EditorForModel()
    @Html.EditorFor(m => Model.CommandActions)
</fieldset>
}

<div id="Results"></div>
```

There's actually a lot of stuff we can say about this, but to summarize:

1. We are utilizing MVC's `EditorForModel`, and `EditorFor`, which will attempt to find "render templates" for each
property on the model. When it does this, it will find that we have an `EditorTemplates\Object.cshtml` and 
`EditorTemplates\CommandActionCollection.cshtml` file. I don't want to go too much into these, because I would prefer 
to refactor this to take place on the client. But, suffice it to say, these render the HTML for the view 
based on simple templates and meta-data.
2. And, we use AjAX to submit the form.

## LoginController.Submit POST operation

This looked pretty interesting:

```csharp
[HttpPost]
[PostAction]
public ActionResult Submit(LoginSubmitViewModel viewModel)
{
    if (ModelState.IsValid)
    {
        var command = new LoginSubmit
                          {
                              Password = viewModel.Password,
                              UserName = viewModel.UserName
                          };
        var response = ProcessCommand<TypedMessageResponse<string>>(command);

        return Content(response.Data);
    }

    return Content("TODO: handle errors properly here.");
}
```

Here we are creating an instance of a `Command`, the `LoginSubmit` class. This is just a simple data class that 
bundles our parameters to send into the to application. We invoke the ProcessCommand method (which could use 
some refactoring), and then return the response data.

## TODO: explain the next parts

# How is a module constructed, anyway?

A module in this app consists of one or more physical DLLs, and, thus far, can contain a number of "standard" classes. Some classes
have special implications because they inherit base classes or implement interfaces from the `Core.Infrastructure` project, and 
by doing so get loaded into the main application automatically during the bootstrap process.

Here's an annotated directory tree example:

```text
UserProfile
|       +---Domain -- contains the Domain classes for the UserProfile module:
|       |   +---Listeners -- contains Domain Event listeners that respond to events raised in other parts of the app
|       |   +---Models -- contains the model classes for this module
|       |   +---Repositories -- for repositories (unused so far here)
|       |   \---Services -- for services, or 'handlers'
|       +---Domain.Tests -- tests for the domain layer
|       +---Interfaces -- an interface-only assembly, such that mocks can be built against this, not the implementation
|       |   \---Repositories -- placeholder for repository interfaces, which are typically mocked in tests
|       +---Messages -- contains the external-facing "contract" classes:
|       |   +---Commands -- commands represent the stimuli coming into the system from the outside. They represent, well, Commands to DO something.
|       |   +---Events -- events are named in the past-tense and represent changes that *have already happened*
|       |   +---Queries -- queries are questions asked of the module, but they never result in changes. They are idempotent
|       |   \---ViewModels -- view models are basically thin value objects consumed by Ui layer classes. They represent projects out of the domain, but specific and slimmed down to support a Ui scenario
|       \---Ui.Mvc
|           +---Controllers -- contains standard ASP.NET MVC controllers. Yes, they are defined in here, not in some global, monolithic, system-wide, "Web" project. **You've all been there, seen that, squinted in pain; won't watch on NetFlix, not even on streaming**.
|           +---Models -- contains MVC models, if any. In our case, none so far.
|           \---Views -- contains the CSHTML Razor views. Again, they are in the MODULE, not a global, brittle catch-all. They get embedded as Resources in the DLL, and MvcCodeRouting pulls them out with it's custom view engine.
```

I don't have time yet to fully annonate this, but here's the tree view of the entire solution so far:

It should start to give you the feel that each part of the app is named clearly, and each folder and file signal the 
intent of what they do.

```text
|   Core.sln
|   Core.vsmdi
|   Local.testsettings
|   TraceAndTestImpact.testsettings
|   
+---.nuget
|       NuGet.Config
|       NuGet.exe
|       NuGet.targets
|       
+---Core.Infrastructure
|   |   App.config
|   |   Core.Infrastructure.csproj
|   |   packages.config
|   |   
|   +---Boot
|   |       BootStrapper.cs
|   |       
|   +---Composition
|   |       ComposedList.cs
|   |       PartsAssembler.cs
|   |       PartsList.cs
|   |       SimpleComposer.cs
|   |       
|   +---Eventing
|   |       EventAggregator.cs
|   |       EventSubscriberWiring.cs
|   |       IEventSubscriber.cs
|   |       IRegisterEventSubscribers.cs
|   |       
|   +---Messaging
|   |       DefaultCommandHandler.cs
|   |       DefaultCommandHandlerWithTypedResponse.cs
|   |       DefaultMessageResponse.cs
|   |       ICommand.cs
|   |       IHandleMessage.cs
|   |       ILogMessages.cs
|   |       IMessage.cs
|   |       IMessageResponse.cs
|   |       IQuery.cs
|   |       IRegisterHandlers.cs
|   |       MessageHandlerWiring.cs
|   |       MessageProcessor.cs
|   |       TypedMessageResponse.cs
|   |       
|   +---Properties
|   |       AssemblyInfo.cs
|   |       
|   +---Repositories
|   +---Security
|   |       CoreFormsAuthentication.cs
|   |       CorePrincipal.cs
|   |       CorePrincipalFactory.cs
|   |       ICorePrincipal.cs
|   |       
|   \---Ui
|       \---Mvc
|           +---Controllers
|           |       DefaultController.cs
|           |       PostActionAttribute.cs
|           |       
|           +---Menu
|           |       MainMenu.cs
|           |       MenuItemAction.cs
|           |       
|           +---Modularity
|           |       DefaultModule.cs
|           |       IModule.cs
|           |       ModuleLoader.cs
|           |       
|           \---ViewModels
|                   CommandAction.cs
|                   CommandActionCollection.cs
|                   ICommandAction.cs
|                   MenuItemWiring.cs
|                   
+---Core.Modules.Deploy
|   |   App.config
|   |   Core.Modules.Deploy.csproj
|   |   Core.Modules.Deploy.exe
|   |   Program.cs
|   |   
|   \---Properties
|           AssemblyInfo.cs
|           
+---Core.Testing
|   |   BaseTestScript.cs
|   |   Core.Testing.csproj
|   |   SerializedStringAssert.cs
|   |   
|   \---Properties
|           AssemblyInfo.cs
|           
+---Core.Ui.Mvc
|   |   Core.Ui.Mvc.csproj
|   |   Core.Ui.Mvc.csproj.user
|   |   Global.asax
|   |   Global.asax.cs
|   |   MessageLogger.cs
|   |   packages.config
|   |   Web.config
|   |   Web.Debug.config
|   |   Web.Release.config
|   |   
|   +---Content
|   |   |   Site.css
|   |   |   
|   |   \---themes
|   |       \---base
|   |           |   jquery.ui.all.css
|   |           |   
|   |           \---images
|   |                   ui-bg_flat_0_aaaaaa_40x100.png
|   |                   
|   +---Properties
|   |       AssemblyInfo.cs
|   |       
|   +---Scripts
|   |       jquery-1.5.1.min.js
|   |       jquery-ui-1.8.11.min.js
|   |       jquery.unobtrusive-ajax.js
|   |       modernizr-1.7.min.js
|   |       
|   \---Views
|       |   Web.config
|       |   _ViewStart.cshtml
|       |   
|       \---Shared
|           |   Error.cshtml
|           |   _Layout.cshtml
|           |   
|           \---EditorTemplates
|                   CommandActionCollection.cshtml
|                   Object.cshtml
|                   
+---Modules
|   +---Login
|   |   +---Domain
|   |   |   |   App.config
|   |   |   |   Domain.csproj
|   |   |   |   packages.config
|   |   |   |   RegisterEventSubscribers.cs
|   |   |   |   RegisterHandlers.cs
|   |   |   |   
|   |   |   +---Listeners
|   |   |   |       UserProfileUpdatedSubscriber.cs
|   |   |   |       
|   |   |   +---Models
|   |   |   |       UserCredential.cs
|   |   |   |       
|   |   |   +---Properties
|   |   |   |       AssemblyInfo.cs
|   |   |   |       
|   |   |   +---Repositories
|   |   |   |       AuthenticationDbContext.cs
|   |   |   |       IUserCredentialRepository.cs
|   |   |   |       UserCredentialRepository.cs
|   |   |   |       
|   |   |   \---Services
|   |   |           LoginSubmitHandler.cs
|   |   |           
|   |   +---Interfaces
|   |   |   |   Interfaces.csproj
|   |   |   |   
|   |   |   \---Properties
|   |   |           AssemblyInfo.cs
|   |   |           
|   |   +---Messages
|   |   |   |   Messages.csproj
|   |   |   |   
|   |   |   +---Commands
|   |   |   |       LoginSubmit.cs
|   |   |   |       
|   |   |   +---Events
|   |   |   |       LoginFailed.cs
|   |   |   |       
|   |   |   \---Properties
|   |   |           AssemblyInfo.cs
|   |   |           
|   |   \---Ui.Mvc
|   |       |   Authentication.csproj
|   |       |   Authentication.csproj.user
|   |       |   Module.cs
|   |       |   packages.config
|   |       |   Ui.Mvc.csproj
|   |       |   Ui.Mvc.csproj.user
|   |       |   web.config
|   |       |   
|   |       +---Controllers
|   |       |   |   AuthenticationController.cs
|   |       |   |   
|   |       |   \---Login
|   |       |           LoginController.cs
|   |       |           
|   |       +---Properties
|   |       |       AssemblyInfo.cs
|   |       |       
|   |       +---ViewModels
|   |       |       LoginSubmitViewModel.cs
|   |       |       
|   |       \---Views
|   |           |   Index.cshtml
|   |           |   
|   |           \---Login
|   |                   Index.cshtml
|   |                   
|   \---UserProfile
|       |   UserProfile.Module.sln
|       |   
|       +---.nuget
|       |       NuGet.Config
|       |       NuGet.exe
|       |       NuGet.targets
|       |       
|       +---Domain
|       |   |   Domain.csproj
|       |   |   Domain.csproj.user
|       |   |   RegisterEventSubscribers.cs
|       |   |   RegisterHandlers.cs
|       |   |   
|       |   +---Listeners
|       |   |       LoginFailedSubscriber.cs
|       |   |       
|       |   +---Models
|       |   |       User.cs
|       |   |       
|       |   +---Properties
|       |   |       AssemblyInfo.cs
|       |   |       
|       |   +---Repositories
|       |   \---Services
|       |           HandleUserProfileEdit.cs
|       |           HandleUserProfileUpdate.cs
|       |           
|       +---Domain.Tests
|       |   |   ConvertToInitializier.cs
|       |   |   Domain.Tests.csproj
|       |   |   EditTests.cs
|       |   |   packages.config
|       |   |   UserProfileTest.cs
|       |   |   
|       |   \---Properties
|       |           AssemblyInfo.cs
|       |           
|       +---Interfaces
|       |   |   Interfaces.csproj
|       |   |   
|       |   +---Properties
|       |   |       AssemblyInfo.cs
|       |   |       
|       |   \---Repositories
|       |           IUserRepository.cs
|       |           
|       +---Messages
|       |   |   Messages.csproj
|       |   |   
|       |   +---Commands
|       |   |       UserProfileUpdate.cs
|       |   |       
|       |   +---Events
|       |   |       UserProfileUpdated.cs
|       |   |       
|       |   +---Properties
|       |   |       AssemblyInfo.cs
|       |   |       
|       |   +---Queries
|       |   |       UserProfileEdit.cs
|       |   |       
|       |   \---ViewModels
|       |                   
|       \---Ui.Mvc
|           |   Module.cs
|           |   packages.config
|           |   Ui.Mvc.csproj
|           |   Ui.Mvc.csproj.user
|           |   web.config
|           |   
|           +---Controllers
|           |       UserProfileController.cs
|           |       
|           +---Models
|           +---Properties
|           |       AssemblyInfo.cs
|           |       
|           \---Views
+---Modules.Deploy
|   +---Authentication
|   |       ...
|   \---UserProfile
|           ...
```


