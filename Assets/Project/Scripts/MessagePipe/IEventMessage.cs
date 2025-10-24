using UnityEngine;

namespace WhaleTee.MessagePipe {
  public interface IEventMessage { }

  public struct ActiveItemSelectedMessage : IEventMessage {
    public readonly GameObject item;
    public readonly int index;
    
    public ActiveItemSelectedMessage(GameObject item, int index) {
      this.item = item;
      this.index = index;
    }
  }

  public struct SelectItemMessage : IEventMessage {
    public readonly GameObject item;

    public SelectItemMessage(GameObject item) => this.item = item;
  }

  public struct AddItemMessage : IEventMessage {
    public readonly Item item;
    public readonly int index;
    public readonly int count;

    public AddItemMessage(Item item, int index, int count) {
      this.item = item;
      this.index = index;
      this.count = count;
    }
  }
}