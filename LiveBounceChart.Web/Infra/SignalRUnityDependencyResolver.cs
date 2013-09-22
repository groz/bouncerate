using System;
using System.Collections.Generic;
using Microsoft.Practices.Unity;

namespace LiveBounceChart.Web.Infra
{
    public class SignalRUnityDependencyResolver : Microsoft.AspNet.SignalR.DefaultDependencyResolver
    {
        private readonly IUnityContainer _container;

        public SignalRUnityDependencyResolver(IUnityContainer container)
        {
            _container = container;
        }

        public override object GetService(Type serviceType)
        {
            if (_container.IsRegistered(serviceType))
            {
                return _container.Resolve(serviceType);
            }
            return base.GetService(serviceType);
        }

        public override IEnumerable<object> GetServices(Type serviceType)
        {
            if (_container.IsRegistered(serviceType))
            {
                return _container.ResolveAll(serviceType);
            }

            return base.GetServices(serviceType);
        }
    }
}