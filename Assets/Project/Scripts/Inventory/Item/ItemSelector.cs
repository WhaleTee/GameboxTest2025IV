using MessagePipe;
using Reflex.Attributes;
using UnityEngine;
using WhaleTee.MessagePipe;

public class ItemSelector : MonoBehaviour {
  [SerializeField]
  private LayerMask itemLayer;

  [SerializeField]
  private HUD hud;

  [Inject]
  private IPublisher<SelectItemEvent, SelectItemMessage> itemSelectPublisher;

  private CollectableItem item;

  private void FixedUpdate() {
    if (Physics.Raycast(transform.position, transform.forward, out var hit, Mathf.Infinity, itemLayer)) {
      var collectableItem = hit.collider.GetComponent<CollectableItem>();
      if (collectableItem == item) return;

      item = collectableItem;
      hud.ActivateInteract();
      itemSelectPublisher.Publish(new SelectItemEvent(), new SelectItemMessage(collectableItem.gameObject));
    } else {
      if (item) {
        itemSelectPublisher.Publish(new SelectItemEvent(), new SelectItemMessage(null));

        item = null;
      }

      hud.DeactivateInteract();
    }
  }
}