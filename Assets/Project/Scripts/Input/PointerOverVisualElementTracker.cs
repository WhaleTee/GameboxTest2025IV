using System.Collections.Generic;
using R3;
using Reflex.Attributes;
using UnityEngine.UIElements;
using WhaleTee.Reactive.Input;
using ZLinq;
using Screen = UnityEngine.Device.Screen;

namespace WhaleTee.Input {
  public sealed class PointerOverVisualElementTracker : IPointerOverUITracker<VisualElement> {
    [Inject] private readonly UserInput userInput;
    private readonly HashSet<VisualElement> trackingElements = new HashSet<VisualElement>();
    private VisualElement pointerOverElement;
    private DisposableBag subscriptions;

    public PointerOverVisualElementTracker(UserInput userInput) {
      this.userInput = userInput;
      userInput.PointerPosition.Subscribe(_ => pointerOverElement = null).AddTo(ref subscriptions);
    }

    private static bool IsElementEnabled(VisualElement element) => element.visible && element.resolvedStyle.display == DisplayStyle.Flex;

    private bool IsElementContainsPointer(VisualElement element) {
      if (userInput == null) return false;
      if (element == null) return false;
      if (pointerOverElement != null) return true;

      // we need to get y-inverted pointer position here
      // UI Toolkit coordinates origin is a top-left, but for the Screen is a bottom-left
      var pointerPosition = userInput.GetPointerPositionInvertY(Screen.height);
      
      if (!element.worldBound.Contains(RuntimePanelUtils.ScreenToPanel(element.panel, pointerPosition))) return false;

      pointerOverElement = element;
      return true;
    }

    public void Track(VisualElement element) => trackingElements.Add(element);

    public void Untrack(VisualElement element) => trackingElements.Remove(element);

    public bool IsTracked(VisualElement element) => trackingElements.Contains(element) && IsElementEnabled(element);

    public bool IsPointerOverUI() => trackingElements.AsValueEnumerable().Where(IsElementEnabled).Any(IsElementContainsPointer);

    public void Dispose() => subscriptions.Dispose();
  }
}