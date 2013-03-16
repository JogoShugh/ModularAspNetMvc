# ModularAspNetMvc

This is a work in progress of a modular architecture for ASP.NET MVC, utilizing the awesome MvcCodeRouting NuGet package.

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

# What is a Module?

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


