using MessagePipe;
using Reflex.Core;

namespace WhaleTee.MessagePipe.Reflex {
  public sealed class MessagePipeContainerDecorator {
    private const string MESSAGE_PIPE_CONTAINER_NAME = "Massage Pipe Container";

    private readonly MessagePipeOptions options;
    private readonly ContainerBuilder containerBuilder;

    public MessagePipeContainerDecorator(ContainerBuilder containerBuilder) {
      this.containerBuilder = containerBuilder;
      var messagePipeContainerBuilder = new ContainerBuilder();
      messagePipeContainerBuilder.SetName(MESSAGE_PIPE_CONTAINER_NAME);
      options = messagePipeContainerBuilder.RegisterMessagePipe();
      foreach (var binding in messagePipeContainerBuilder.Bindings) containerBuilder.Bindings.Add(binding);
      containerBuilder.AddSingleton(_ => messagePipeContainerBuilder.Build().AsServiceProvider());
      containerBuilder.OnContainerBuilt += container => GlobalMessagePipe.SetProvider(container.AsServiceProvider());
    }

    public ContainerBuilder RegisterMessageBroker<TMessage>() where TMessage : IEventMessage =>
    containerBuilder.RegisterMessageBroker<TMessage>(options);

    public ContainerBuilder RegisterMessageBroker<TKey, TMessage>() where TKey : IEventKey where TMessage : IEventMessage =>
    containerBuilder.RegisterMessageBroker<TKey, TMessage>(options);

    public ContainerBuilder RegisterRequestHandler<TRequest, TResponse, THandler>()
    where TRequest : IEventMessage where TResponse : IEventMessage where THandler : IRequestHandler =>
    containerBuilder.RegisterRequestHandler<TRequest, TResponse, THandler>(options);

    public ContainerBuilder RegisterAsyncRequestHandler<TRequest, TResponse, THandler>()
    where TRequest : IEventMessage where TResponse : IEventMessage where THandler : IAsyncRequestHandler =>
    containerBuilder.RegisterAsyncRequestHandler<TRequest, TResponse, THandler>(options);
  }
}