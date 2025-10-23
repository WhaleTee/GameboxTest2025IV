using MessagePipe;
using Reflex.Attributes;
using UnityEngine;
using UnityEngine.UIElements;
using WhaleTee.MessagePipe;

public class HUD : MonoBehaviour {
  [SerializeField] private UIDocument UIDocument;
  [SerializeField] private Sprite activeItemSprite;
  [SerializeField] private Sprite inactiveItemSprite;

  [Inject]
  private ISubscriber<AddItemEvent, AddItemMessage> addItemSubscriber;

  [Inject]
  private ISubscriber<ActiveItemSelectedEvent, ActiveItemSelectedMessage> selectActiveItemSubscriber;
  
  private VisualElement root;
  private VisualElement interact;
  private VisualElement inventory;
  private VisualElement[] inventoryCells = new VisualElement[10];
  
  private void Awake() {
    root = UIDocument.rootVisualElement;
    interact = root.Q("Interact");
    inventory = root.Q("Inventory");
    inventoryCells[0] = inventory.Q("Container");

    for (var i = 1; i < inventoryCells.Length; i++) {
      var itemContainer = new VisualElement();
      var item = new VisualElement();
      var name = new Label();
      item.AddToClassList("item");
      item.name = "Item";
      name.AddToClassList("item-name");
      name.name = "Name";
      itemContainer.AddToClassList("item-container");
      itemContainer.Add(item);
      itemContainer.Add(name);
      inventory.Add(itemContainer);
      inventoryCells[i] = itemContainer;
    }

    addItemSubscriber.Subscribe(
      new AddItemEvent(),
      message => {
        inventoryCells[message.index].Q("Item").style.backgroundImage = new StyleBackground(message.item.Sprite);
        inventoryCells[message.index].Q<Label>("Name").text = message.item.Name;
        inventoryCells[message.index].style.display = DisplayStyle.Flex;
      }
    );

    selectActiveItemSubscriber.Subscribe(new ActiveItemSelectedEvent(),
                                         message => {
                                           for (var i = 0; i < inventoryCells.Length; i++)
                                           {
                                             if (i != message.index) inventoryCells[i].style.backgroundImage = new StyleBackground(inactiveItemSprite);
                                           }

                                           inventoryCells[message.index].style.backgroundImage = new StyleBackground(activeItemSprite);
                                         });
  }
  
  public void ActivateInteract() => interact.style.display = DisplayStyle.Flex;
  public void DeactivateInteract() => interact.style.display = DisplayStyle.None;
}