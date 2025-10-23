namespace WhaleTee.MessagePipe {
  public interface IEventMessage { }

  public struct ActiveItemSelectedMessage : IEventMessage {
    public readonly Item item;
    public readonly int index;
    
    public ActiveItemSelectedMessage(Item item, int index) {
      this.item = item;
      this.index = index;
    }
  }

  public struct SelectItemMessage : IEventMessage {
    public readonly Item item;

    public SelectItemMessage(Item item) => this.item = item;
  }

  public struct AddItemMessage : IEventMessage {
    public readonly Item item;
    public readonly int index;

    public AddItemMessage(Item item, int index) {
      this.item = item;
      this.index = index;
    }
  }
}