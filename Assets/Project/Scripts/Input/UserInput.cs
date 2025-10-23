using System;
using R3;
using UnityEngine;
using UnityEngine.InputSystem;
using WhaleTee.EqualityComparers;
using WhaleTee.Input;
using WhaleTee.Extensions;

namespace WhaleTee.Reactive.Input {
  public sealed class UserInput : IDisposable {
    private const float EQUITY_TOLERANCE = 0.1f;
    private readonly InputActions inputActions;
    private DisposableBag subscriptions;

    private static Camera MainCamera => Camera.main;

    public ReactiveProperty<bool> LeftClick { get; } = new ReactiveProperty<bool>();
    public ReactiveProperty<bool> RightClick { get; } = new ReactiveProperty<bool>();
    public ReactiveProperty<Vector2> PointerPosition { get; } = new ReactiveProperty<Vector2>(Vector2.zero, new Vector2EqualityComparer(EQUITY_TOLERANCE));
    public ReactiveProperty<Vector2> ScrollWheel { get; } = new ReactiveProperty<Vector2>(Vector2.zero, new Vector2EqualityComparer(EQUITY_TOLERANCE));
    public ReactiveProperty<Vector2> Move { get; } = new ReactiveProperty<Vector2>(Vector2.zero, new Vector2EqualityComparer(EQUITY_TOLERANCE));
    public ReactiveProperty<Vector2> Look { get; } = new ReactiveProperty<Vector2>(Vector2.zero, new Vector2EqualityComparer(EQUITY_TOLERANCE)); 
    public ReactiveProperty<bool> Jump { get; } = new ReactiveProperty<bool>();
    public ReactiveProperty<int> KeyboardNum { get; } = new ReactiveProperty<int>(-1);
    public ReactiveProperty<bool> Interact { get; } = new ReactiveProperty<bool>();
    public ReactiveProperty<bool> Next { get; } = new ReactiveProperty<bool>();
    public ReactiveProperty<bool> Previous { get; } = new ReactiveProperty<bool>();
    public ReactiveProperty<bool> Shoot { get; } = new ReactiveProperty<bool>();
    public ReactiveProperty<bool> Throw { get; } = new ReactiveProperty<bool>();

    public UserInput() {
      inputActions = new InputActions();
      inputActions.Enable();
      UpdateMouseProperties();
    }

    private static Vector3 ScreenToWorldPoint(Camera camera, Vector3 point) => camera.ScreenToWorldPoint(point);

    private void UpdateMouseProperties() {
      Observable.EveryUpdate(UnityFrameProvider.EarlyUpdate)
                .Subscribe(_ => {
                             LeftClick.Value = inputActions.UI.Click.IsPressed();
                             RightClick.Value = inputActions.UI.RightClick.IsPressed();
                             Jump.Value = inputActions.Player.Jump.IsPressed() && inputActions.Player.Jump.WasPressedThisFrame();
                             Move.Value = inputActions.Player.Move.IsPressed() ? inputActions.Player.Move.ReadValue<Vector2>() : Vector2.zero;
                             KeyboardNum.Value = inputActions.Player.NumKeys.WasPressedThisFrame() 
                                                 ? (int)inputActions.Player.NumKeys.ReadValue<float>() 
                                                 : -1;
                             Interact.Value = inputActions.Player.Interact.IsPressed() && inputActions.Player.Interact.WasPressedThisFrame();
                             Next.Value = inputActions.Player.Next.IsPressed() && inputActions.Player.Next.WasPressedThisFrame();
                             Previous.Value = inputActions.Player.Previous.IsPressed() && inputActions.Player.Previous.WasPressedThisFrame();
                             Shoot.Value = inputActions.Player.Attack.IsPressed();
                             Throw.Value = inputActions.Player.Throw.IsPressed() && inputActions.Player.Throw.WasPressedThisFrame();
                           }
                )
                .AddTo(ref subscriptions);

      Observable.FromEvent<InputAction.CallbackContext>(
                  handler => inputActions.UI.Point.performed += handler,
                  handler => inputActions.UI.Point.performed -= handler
                )
                .Subscribe(ctx => PointerPosition.Value = ctx.ReadValue<Vector2>())
                .AddTo(ref subscriptions);

      Observable.FromEvent<InputAction.CallbackContext>(
                  handler => inputActions.UI.ScrollWheel.performed += handler,
                  handler => inputActions.UI.ScrollWheel.performed -= handler
                )
                .Subscribe(ctx => ScrollWheel.Value += ctx.ReadValue<Vector2>())
                .AddTo(ref subscriptions);

      Observable.FromEvent<InputAction.CallbackContext>(
                  handler => inputActions.Player.Look.performed += handler,
                  handler => inputActions.Player.Look.performed -= handler
                )
                .Subscribe(ctx =>  Look.Value = ctx.ReadValue<Vector2>())
                .AddTo(ref subscriptions);

      Observable.EveryUpdate(UnityFrameProvider.PostLateUpdate)
                .Subscribe(_ => {
                             ScrollWheel.Value = Vector2.zero;
                             Look.Value = Vector2.zero;
                           }
                )
                .AddTo(ref subscriptions);
    }

    public static void WrapCursorPosition(Vector2 position) => Mouse.current.WarpCursorPosition(position);

    public Vector3 GetPointerPositionWorld() => ScreenToWorldPoint(MainCamera, PointerPosition.Value.With(z: MainCamera.transform.position.z));

    public Vector3 GetPointerPositionWorldSaveZ(Vector3 point) =>
    ScreenToWorldPoint(MainCamera, PointerPosition.Value.With(z: MainCamera.transform.InverseTransformPoint(point).z));

    public Vector2 GetPointerPositionInvertY(float value) {
      var pointerPosition = PointerPosition.Value;
      pointerPosition.y = value - pointerPosition.y;
      return pointerPosition;
    }

    public void SetCursorVisible(bool visible, Vector2 atPoint) {
      Cursor.lockState = !visible ? CursorLockMode.Locked : CursorLockMode.None;
      Cursor.visible = visible;
      WrapCursorPosition(atPoint);
    }

    public void Dispose() {
      subscriptions.Dispose();
      inputActions?.Disable();
      inputActions?.Dispose();
    }
  }
}