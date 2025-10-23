using System.Collections.Generic;
using R3;
using Reflex.Attributes;
using UnityEngine;
using UnityEngine.EventSystems;
using WhaleTee.Reactive.Input;
using WhaleTee.Extensions;
using ZLinq;

namespace WhaleTee.Input {
  public sealed class PointerOverGameObjectTracker : IPointerOverUITracker<GameObject> {
    [Inject] private readonly UserInput userInput;
    private readonly HashSet<GameObject> trackingGameObjects;
    private readonly PointerEventData pointerEventData;
    private readonly List<RaycastResult> raycastResults;
    private GameObject pointerOverGameObject;
    private DisposableBag subscriptions;

    private static EventSystem EventSystem => EventSystem.current;

    public PointerOverGameObjectTracker(UserInput userInput) {
      this.userInput = userInput;
      trackingGameObjects = new HashSet<GameObject>();
      pointerEventData = new PointerEventData(EventSystem);
      raycastResults = new List<RaycastResult>();
      userInput.PointerPosition.Subscribe(_ => pointerOverGameObject = null).AddTo(ref subscriptions);
    }

    private static bool IsGameObjectEnabled(GameObject gameObject) => gameObject.activeSelf && gameObject.activeInHierarchy;

    private bool IsPointerOverGameObject() {
      if (userInput == null) return false;
      if (pointerOverGameObject.OrNull() != null) return true;

      pointerEventData.position = userInput.PointerPosition.Value;

      raycastResults.Clear();
      EventSystem.RaycastAll(pointerEventData, raycastResults);

      pointerOverGameObject = raycastResults.AsValueEnumerable()
                                            .Select(result => result.gameObject)
                                            .Where(IsGameObjectEnabled)
                                            .FirstOrDefault(trackingGameObjects.Contains);

      return pointerOverGameObject.OrNull() != null;
    }

    public void Track(GameObject ui) => trackingGameObjects.Add(ui);

    public void Untrack(GameObject ui) => trackingGameObjects.Remove(ui);

    public bool IsTracked(GameObject ui) => trackingGameObjects.Contains(ui);

    public bool IsPointerOverUI() => IsPointerOverGameObject();

    public void Dispose() => subscriptions.Dispose();
  }
}