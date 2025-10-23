using System.Collections.Generic;
using ZLinq;

public class Inventory {
  private readonly Item[] inventory = new Item[10];
  private readonly bool[] emptyInventoryCells = new bool[10];
  private readonly Dictionary<Item, int> items = new Dictionary<Item, int>();
  private int lastIndex = 0;

  public Inventory() {
    for (var i = 0; i < emptyInventoryCells.Length; i++) {
      emptyInventoryCells[i] = true;
    }
  }

  private int NextIndex(int index) {
    var nextIndex = index + 1;
    if (nextIndex >= inventory.Length) nextIndex = 0;
    return nextIndex;
  }

  private int PreviousIndex(int index) {
    var nextIndex = index - 1;
    if (nextIndex < 0) nextIndex = inventory.Length - 1;
    return nextIndex;
  }

  private int NextNonEmptyIndex(int index) {
    var nextIndex = NextIndex(index);

    while (nextIndex != index) {
      if (!emptyInventoryCells[nextIndex]) return nextIndex;
      nextIndex = NextIndex(nextIndex);
    }

    return -1;
  }

  private int PreviousNonEmptyIndex(int index) {
    var previousIndex = PreviousIndex(index);

    while (previousIndex != index) {
      if (!emptyInventoryCells[previousIndex]) return previousIndex;
      previousIndex = PreviousIndex(previousIndex);
    }

    return -1;
  }

  private int FirstEmptyCell() {
    for (var i = 0; i < emptyInventoryCells.Length; i++) {
      if (emptyInventoryCells[i]) return i;
    }

    return -1;
  }

  public void AddItem(Item item, int count = 1) {
    items.Add(item, count);

    if (!inventory.AsValueEnumerable().Contains(item)) {
      if (lastIndex < 10 || inventory[0] == null) {
        lastIndex = NextIndex(lastIndex);
        var index = lastIndex;
        if (!emptyInventoryCells[index]) index = FirstEmptyCell();
        inventory[index] = item;
        emptyInventoryCells[index] = false;
      }
    }
  }

  public void RemoveItem(Item item, int count = 1) {
    var value = items.GetValueOrDefault(item);
    value -= count;

    if (value > 0) items[item] = value;
    else {
      items.Remove(item);

      for (var i = 0; i < inventory.Length; i++) {
        if (inventory[i] == item) emptyInventoryCells[i] = true;
      }
    }
  }

  public Item GetItem(int index) {
    if (index < 0 || index >= inventory.Length) return null;

    return !emptyInventoryCells[index] ? inventory[index] : null;
  }

  public Item GetNextItem(int index) {
    if (index < 0 || index >= inventory.Length) return null;

    var nextNonEmptyIndex = NextNonEmptyIndex(index);

    return nextNonEmptyIndex != -1 ? inventory[nextNonEmptyIndex] : null;
  }

  public Item GetPreviousItem(int index) {
    if (index < 0 || index >= inventory.Length) return null;

    var previousNonEmptyIndex = PreviousNonEmptyIndex(index);

    return previousNonEmptyIndex != -1 ? inventory[previousNonEmptyIndex] : null;
  }

  public int GetCount(Item item) => items[item];

  public int IndexOf(Item item) {
    for (var i = 0; i < inventory.Length; i++) {
      if (inventory[i] == item) return i;
    }

    return -1;
  }
}