using System;
using Reflex.Core;
using UnityEngine.Scripting;

namespace WhaleTee.MessagePipe.Reflex {
  [Preserve]
  public sealed class ServiceProviderContainerAdapter : IServiceProvider {
    private readonly Container container;

    [Preserve]
    public ServiceProviderContainerAdapter(Container container) => this.container = container;

    public object GetService(Type serviceType) => container.Resolve(serviceType);
  }
}