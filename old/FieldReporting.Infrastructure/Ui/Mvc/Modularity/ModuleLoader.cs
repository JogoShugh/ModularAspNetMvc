using System;
using System.Collections.Generic;
using System.Web.Routing;
using FieldReporting.Infrastructure.Composition;
using MvcCodeRouting;

namespace FieldReporting.Infrastructure.Ui.Mvc.Modularity
{
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
}
