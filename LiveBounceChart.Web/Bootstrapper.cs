using System;
using System.Web.Mvc;
using Microsoft.Practices.Unity;
using Unity.Mvc4;

namespace LiveBounceChart.Web
{
    public static class Bootstrapper
    {
        public static IUnityContainer Initialize(Action<IUnityContainer> registerTypes)
        {
            var container = new UnityContainer();

            registerTypes(container);

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));

            return container;
        }
    }
}