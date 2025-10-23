using System;
using MessagePipe;
using Reflex.Core;

namespace WhaleTee.MessagePipe.Reflex {
  public static class ContainerBuilderExtensions {
    public static MessagePipeOptions RegisterMessagePipe(this ContainerBuilder builder) => RegisterMessagePipe(builder, _ => { });

    public static MessagePipeOptions RegisterMessagePipe(this ContainerBuilder builder, Action<MessagePipeOptions> configure) {
      MessagePipeOptions messagePipeOptions = null;
      var proxy = new ServiceCollectionContainerBuilderAdapter(builder);

      proxy.AddMessagePipe(options => {
          configure(options);
          messagePipeOptions = options;
        }
      );

      return messagePipeOptions;
    }

    public static ContainerBuilder RegisterMessageBroker<TMessage>(
      this ContainerBuilder builder,
      MessagePipeOptions options
    ) where TMessage : IEventMessage {
      var lifetime = options.InstanceLifetime.AsLifetime();
      var services = new ServiceCollectionContainerBuilderAdapter(builder);

      // keyless Pub/Sub
      services.Add(typeof(MessageBrokerCore<TMessage>), lifetime);
      services.Add(typeof(IPublisher<TMessage>), typeof(MessageBroker<TMessage>), lifetime);
      services.Add(typeof(ISubscriber<TMessage>), typeof(MessageBroker<TMessage>), lifetime);

      // keyless Pub/Sub async
      services.Add(typeof(AsyncMessageBrokerCore<TMessage>), lifetime);
      services.Add(typeof(IAsyncPublisher<TMessage>), typeof(AsyncMessageBroker<TMessage>), lifetime);
      services.Add(typeof(IAsyncSubscriber<TMessage>), typeof(AsyncMessageBroker<TMessage>), lifetime);

      // keyless buffered Pub/Sub
      services.Add(typeof(BufferedMessageBrokerCore<TMessage>), lifetime);
      services.Add(typeof(IBufferedPublisher<TMessage>), typeof(BufferedMessageBroker<TMessage>), lifetime);
      services.Add(typeof(IBufferedSubscriber<TMessage>), typeof(BufferedMessageBroker<TMessage>), lifetime);

      // keyless buffered Pub/Sub async
      services.Add(typeof(BufferedAsyncMessageBrokerCore<TMessage>), lifetime);
      services.Add(typeof(IBufferedAsyncPublisher<TMessage>), typeof(BufferedAsyncMessageBroker<TMessage>), lifetime);
      services.Add(typeof(IBufferedAsyncSubscriber<TMessage>), typeof(BufferedAsyncMessageBroker<TMessage>), lifetime);

      return builder;
    }

    public static ContainerBuilder RegisterMessageBroker<TKey, TMessage>(
      this ContainerBuilder builder,
      MessagePipeOptions options
    ) where TKey : IEventKey where TMessage : IEventMessage {
      var lifetime = options.InstanceLifetime.AsLifetime();
      var services = new ServiceCollectionContainerBuilderAdapter(builder);

      // keyed Pub/Sub
      services.Add(typeof(MessageBrokerCore<TKey, TMessage>), lifetime);
      services.Add(typeof(IPublisher<TKey, TMessage>), typeof(MessageBroker<TKey, TMessage>), lifetime);
      services.Add(typeof(ISubscriber<TKey, TMessage>), typeof(MessageBroker<TKey, TMessage>), lifetime);

      // keyed Pub/Sub async
      services.Add(typeof(AsyncMessageBrokerCore<TKey, TMessage>), lifetime);
      services.Add(typeof(IAsyncPublisher<TKey, TMessage>), typeof(AsyncMessageBroker<TKey, TMessage>), lifetime);
      services.Add(typeof(IAsyncSubscriber<TKey, TMessage>), typeof(AsyncMessageBroker<TKey, TMessage>), lifetime);

      return builder;
    }

    public static ContainerBuilder RegisterRequestHandler<TRequest, TResponse, THandler>(
      this ContainerBuilder builder,
      MessagePipeOptions options
    ) where TRequest : IEventMessage where TResponse : IEventMessage where THandler : IRequestHandler {
      var lifetime = options.RequestHandlerLifetime.AsLifetime();
      var services = new ServiceCollectionContainerBuilderAdapter(builder);

      services.Add(typeof(IRequestHandlerCore<TRequest, TResponse>), typeof(THandler), lifetime);

      if (!builder.HasBinding(typeof(IRequestHandler<TRequest, TResponse>))) {
        services.Add(typeof(IRequestHandler<TRequest, TResponse>), typeof(RequestHandler<TRequest, TResponse>), lifetime);
        services.Add(typeof(IRequestAllHandler<TRequest, TResponse>), typeof(RequestAllHandler<TRequest, TResponse>), lifetime);
      }

      return builder;
    }

    public static ContainerBuilder RegisterAsyncRequestHandler<TRequest, TResponse, THandler>(
      this ContainerBuilder builder, MessagePipeOptions options
    ) where TRequest : IEventMessage where TResponse : IEventMessage where THandler : IAsyncRequestHandler {
      var lifetime = options.RequestHandlerLifetime.AsLifetime();
      var services = new ServiceCollectionContainerBuilderAdapter(builder);

      services.Add(typeof(IAsyncRequestHandlerCore<TRequest, TResponse>), typeof(THandler), lifetime);

      if (!builder.HasBinding(typeof(IAsyncRequestHandler<TRequest, TResponse>))) {
        services.Add(typeof(IAsyncRequestHandler<TRequest, TResponse>), typeof(AsyncRequestHandler<TRequest, TResponse>), lifetime);
        services.Add(typeof(IAsyncRequestAllHandler<TRequest, TResponse>), typeof(AsyncRequestAllHandler<TRequest, TResponse>), lifetime);
      }

      AsyncRequestHandlerRegistory.Add(typeof(TRequest), typeof(TResponse), typeof(THandler));
      return builder;
    }

    public static ContainerBuilder RegisterMessageHandlerFilter<T>(this ContainerBuilder builder) where T : class, IMessageHandlerFilter {
      var type = typeof(T);
      if (!builder.HasBinding(type)) builder.AddTransient(type);
      return builder;
    }

    public static ContainerBuilder RegisterAsyncMessageHandlerFilter<T>(this ContainerBuilder builder) where T : class, IAsyncMessageHandlerFilter {
      var type = typeof(T);
      if (!builder.HasBinding(type)) builder.AddTransient(type);
      return builder;
    }

    public static ContainerBuilder RegisterRequestHandlerFilter<T>(this ContainerBuilder builder) where T : class, IRequestHandlerFilter {
      var type = typeof(T);
      if (!builder.HasBinding(type)) builder.AddTransient(type);
      return builder;
    }

    public static ContainerBuilder RegisterAsyncRequestHandlerFilter<T>(this ContainerBuilder builder) where T : class, IAsyncRequestHandlerFilter {
      var type = typeof(T);
      if (!builder.HasBinding(type)) builder.AddTransient(type);
      return builder;
    }

    public static IServiceCollection AsServiceCollection(this ContainerBuilder builder) => new ServiceCollectionContainerBuilderAdapter(builder);

    public static IMessagePipeBuilder ToMessagePipeBuilder(this ContainerBuilder builder) => new MessagePipeBuilder(builder.AsServiceCollection());
  }
}