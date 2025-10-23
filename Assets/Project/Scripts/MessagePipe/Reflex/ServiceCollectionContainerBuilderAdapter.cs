using System;
using MessagePipe;
using Reflex.Core;
using Reflex.Enums;
using WhaleTee.Extensions;

namespace WhaleTee.MessagePipe.Reflex {
  internal class ServiceCollectionContainerBuilderAdapter : IServiceCollection {
    private readonly ContainerBuilder containerBuilder;

    public ServiceCollectionContainerBuilderAdapter(ContainerBuilder containerBuilder) => this.containerBuilder = containerBuilder;

    public void AddTransient(Type type) => containerBuilder.AddTransient(type);

    public void TryAddTransient(Type type) {
      if (!containerBuilder.HasBinding(type)) containerBuilder.AddTransient(type);
    }

    public void AddSingleton<T>(T instance) => containerBuilder.AddSingleton(instance, typeof(T));

    public void AddSingleton(Type type) => containerBuilder.AddSingleton(type);

    public void Add(Type type, Lifetime lifetime, params Type[] implementations) {
      switch (lifetime) {
        case Lifetime.Scoped:
          if (implementations.IsNullOrEmpty()) containerBuilder.AddScoped(type);
          else containerBuilder.AddScoped(type, implementations);

          break;
        case Lifetime.Singleton:
          if (implementations.IsNullOrEmpty()) containerBuilder.AddSingleton(type);
          else containerBuilder.AddSingleton(type, implementations);

          break;
        case Lifetime.Transient:
          if (implementations.IsNullOrEmpty()) containerBuilder.AddTransient(type);
          else containerBuilder.AddTransient(type, implementations);

          break;
      }
    }

    public void Add(Type serviceType, Type implementationType, Lifetime lifetime) => Add(implementationType, lifetime, serviceType);

    public void Add(Type serviceType, InstanceLifetime lifetime) => Add(serviceType, lifetime.AsLifetime());

    public void Add(Type serviceType, Type implementationType, InstanceLifetime lifetime) =>
    Add(implementationType, lifetime.AsLifetime(), serviceType);
  }
}