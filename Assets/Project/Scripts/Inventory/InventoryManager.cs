using MessagePipe;
using R3;
using Reflex.Attributes;
using UnityEngine;
using WhaleTee.Extensions;
using WhaleTee.MessagePipe;
using WhaleTee.Reactive.Input;

public class InventoryManager : MonoBehaviour {
  [SerializeField]
  private LayerMask inventoryItemLayer;

  [Inject]
  private UserInput userInput;

  [Inject]
  private IPublisher<ActiveItemSelectedEvent, ActiveItemSelectedMessage> activeItemPublisher;

  [Inject]
  private IPublisher<AddItemEvent, AddItemMessage> addItemPublisher;

  [Inject]
  private ISubscriber<SelectItemEvent, SelectItemMessage> selectItemSubscriber;

  private readonly Inventory inventory = new Inventory();

  private CollectableItem selectedItem;
  private int activeItemIndex;
  private bool selectedItemInteractable;

  private void Awake() {
    userInput.KeyboardNum.Where(value => value != -1)
             .Subscribe(value => {
                          var item = inventory.GetItem(value);

                          if (item.OrNull() == null) activeItemIndex = -1;
                          else activeItemIndex = value;

                          activeItemPublisher.Publish(new ActiveItemSelectedEvent(), new ActiveItemSelectedMessage(inventory.GetOwner(item), activeItemIndex));
                        }
             )
             .AddTo(this);

    userInput.Next.Where(value => value)
             .Subscribe(_ => {
                          var item = inventory.GetNextItem(activeItemIndex);
                          if (item != null) activeItemIndex = inventory.IndexOf(item);
                          activeItemPublisher.Publish(new ActiveItemSelectedEvent(), new ActiveItemSelectedMessage(inventory.GetOwner(item), activeItemIndex));
                        }
             )
             .AddTo(this);

    userInput.Previous.Where(value => value)
             .Subscribe(_ => {
                          var item = inventory.GetPreviousItem(activeItemIndex);
                          if (item != null) activeItemIndex = inventory.IndexOf(item);
                          activeItemPublisher.Publish(new ActiveItemSelectedEvent(), new ActiveItemSelectedMessage(inventory.GetOwner(item), activeItemIndex));
                        }
             )
             .AddTo(this);

    userInput.Interact.Where(value => value && selectedItemInteractable)
             .Subscribe(_ => {
                          inventory.AddItem(selectedItem.Item);

                          selectedItem.gameObject.SetActive(false);

                          selectedItem.gameObject.SetLayersRecursively((int)Mathf.Log(inventoryItemLayer, 2f));

                          activeItemIndex = inventory.IndexOf(selectedItem.Item);
                          addItemPublisher.Publish(new AddItemEvent(), new AddItemMessage(selectedItem.Item, activeItemIndex, 1));
                          selectedItem = null;
                          selectedItemInteractable = false;
                        }
             )
             .AddTo(this);

    selectItemSubscriber.Subscribe(
      new SelectItemEvent(),
      message => {
        selectedItem = message.item.OrNull() != null ? message.item.GetComponent<CollectableItem>() : null;
        selectedItemInteractable = selectedItem != null;
      }
    );
  }
}