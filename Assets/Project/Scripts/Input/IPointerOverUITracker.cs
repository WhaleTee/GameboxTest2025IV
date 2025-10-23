using System;

namespace WhaleTee.Input {
  public interface IPointerOverUITracker<in UI> : IDisposable {
    void Track(UI ui);
    void Untrack(UI ui);
    bool IsTracked(UI ui);
    bool IsPointerOverUI();
  }
}