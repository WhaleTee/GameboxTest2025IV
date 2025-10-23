namespace WhaleTee.MessagePipe {
  public interface IEventKey { }

  public struct ActiveItemSelectedEvent : IEventKey { }
  public struct SelectItemEvent : IEventKey { }
  public struct AddItemEvent : IEventKey { }
}