using System;
using Reflex.Core;

namespace WhaleTee.MessagePipe.Reflex {
  public static class ContainerExtensions {
    public static IServiceProvider AsServiceProvider(this Container container) => new ServiceProviderContainerAdapter(container);
  }
}