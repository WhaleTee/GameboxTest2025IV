using Reflex.Core;
using UnityEngine;
using WhaleTee.Factory;
using WhaleTee.MessagePipe;
using WhaleTee.MessagePipe.Reflex;
using WhaleTee.ObjectPooling;
using WhaleTee.Reactive.Input;

namespace WhaleTee.GameboxTest2025IV.DI {
  public class ProjectInstaller : MonoBehaviour, IInstaller {
    private const string ROOT_CONTAINER_NAME = "Root Container";

    private Container rootContainer;

    private static void InstallMessagePipe(ContainerBuilder rootContainerBuilder) {
      var pipeContainerDecorator = new MessagePipeContainerDecorator(rootContainerBuilder);

      pipeContainerDecorator.RegisterMessageBroker<ActiveItemSelectedEvent, ActiveItemSelectedMessage>();
      pipeContainerDecorator.RegisterMessageBroker<SelectItemEvent, SelectItemMessage>();
      pipeContainerDecorator.RegisterMessageBroker<AddItemEvent, AddItemMessage>();
    }

    private void InstallServices(ContainerBuilder containerBuilder) {
      containerBuilder.OnContainerBuilt += container => {
                                             rootContainer = container;
                                             rootContainer.Resolve<ObjectPoolManager>().Initialize();
                                           };

      containerBuilder.AddSingleton(typeof(UserInput));
      containerBuilder.AddSingleton(typeof(GameObjectFactory), typeof(DeactivatedGameObjectFactory));
      containerBuilder.AddSingleton(typeof(ObjectPoolManager));
      containerBuilder.AddSingleton(typeof(ObjectThrowService));
      containerBuilder.AddSingleton(typeof(WeaponActiveService));
    }

    public void InstallBindings(ContainerBuilder containerBuilder) {
      containerBuilder.SetName(ROOT_CONTAINER_NAME);
      InstallMessagePipe(containerBuilder);
      InstallServices(containerBuilder);
    }
  }
}