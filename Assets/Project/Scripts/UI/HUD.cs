using MessagePipe;
using Reflex.Attributes;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;
using WhaleTee.MessagePipe;

public class HUD : MonoBehaviour {
  [SerializeField]
  private UIDocument uiDocument;

  [SerializeField]
  private Sprite activeItemSprite;

  [SerializeField]
  private Sprite inactiveItemSprite;

  [Inject]
  private ISubscriber<AddItemEvent, AddItemMessage> addItemSubscriber;

  [Inject]
  private ISubscriber<ActiveItemSelectedEvent, ActiveItemSelectedMessage> selectActiveItemSubscriber;

  private readonly VisualElement[] inventoryCells = new VisualElement[10];
  private VisualElement root;
  private VisualElement interact;
  private VisualElement inventory;

  private void Awake() {
    root = uiDocument.rootVisualElement;
    interact = root.Q("Interact");
    inventory = root.Q("Inventory");
    inventoryCells[0] = inventory.Q("Container");

    for (var i = 1; i < inventoryCells.Length; i++) {
      var itemContainer = new VisualElement();
      var item = new VisualElement();
      var itemData = new VisualElement();
      var itemName = new Label();
      var itemCount = new Label();
      item.AddToClassList("item");
      item.name = "Item";
      itemName.AddToClassList("item-name");
      itemName.name = "Name";
      itemCount.AddToClassList("item-name");
      itemCount.name = "Count";
      itemData.Add(itemName);
      itemData.Add(itemCount);
      itemData.style.flexDirection = FlexDirection.Row;
      itemContainer.AddToClassList("item-container");
      itemContainer.Add(item);
      itemContainer.Add(itemData);
      inventory.Add(itemContainer);
      inventoryCells[i] = itemContainer;
    }

    addItemSubscriber.Subscribe(
      new AddItemEvent(),
      message => {
        inventoryCells[message.index].Q("Item").style.backgroundImage = new StyleBackground(message.item.Sprite);
        inventoryCells[message.index].Q<Label>("Name").text = message.item.Name;
        var count = inventoryCells[message.index].Q<Label>("Count");
        count.text = int.TryParse(count.text, out var result) ? (result + message.count).ToString() : message.count.ToString();
        inventoryCells[message.index].style.display = DisplayStyle.Flex;
      }
    );

    selectActiveItemSubscriber.Subscribe(
      new ActiveItemSelectedEvent(),
      message => {
        if (message.index == -1) return;
        for (var i = 0; i < inventoryCells.Length; i++) {
          if (i != message.index) inventoryCells[i].style.backgroundImage = new StyleBackground(inactiveItemSprite);
        }

        inventoryCells[message.index].style.backgroundImage = new StyleBackground(activeItemSprite);
      }
    );
  }

  public void ActivateInteract() => interact.style.display = DisplayStyle.Flex;
  public void DeactivateInteract() => interact.style.display = DisplayStyle.None;
}