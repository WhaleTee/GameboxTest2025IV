using System;
using System.Threading;
using MessagePipe;
using R3;
using Reflex.Attributes;
using UnityEngine;
using WhaleTee.Extensions;
using WhaleTee.MessagePipe;
using WhaleTee.Reactive.Input;

[RequireComponent(typeof(Rigidbody))]
public class CharacterController : MonoBehaviour {
  [Header("Camera")]
  [SerializeField]
  private CameraFollowSettings cameraFollowSettings;

  [SerializeField]
  private CameraRotationSettings cameraRotationSettings;

  private CameraFollow cameraFollow;
  private CameraRotation cameraRotation;

  [Header("Movement")]
  [SerializeField]
  private GameObject orientation;

  [SerializeField]
  private float moveSpeed;

  [SerializeField]
  private float groundDamping;

  [SerializeField]
  private float airMoveSpeed;

  [SerializeField]
  private float airDamping;

  [SerializeField]
  private float jumpHeight;

  private const float SPEED_MULTIPLIER = 10;

  [Header("Ground")]
  [SerializeField]
  private float raycastDistance;

  [SerializeField]
  private LayerMask groundMask;

  [SerializeField]
  private float slopeMaxAngle;

  [SerializeField]
  private float slopePressForce;

  [Header("Items")]
  [SerializeField]
  private GameObject itemHolder;
  
  [SerializeField]
  private float throwAngle;
  
  [SerializeField]
  private float throwForce;
  
  [SerializeField]
  private GameObject grenadePrefab;
  
  [Inject]
  private UserInput userInput;

  [Inject]
  private ISubscriber<ActiveItemSelectedEvent, ActiveItemSelectedMessage> activeItemSelectedSubscriber;

  private Rigidbody mainRigidbody;
  private bool grounded;
  private bool onSlope;
  private Vector3 slopeNormal;
  private bool jumping;
  private GameObject activeItem;
  private ObjectThrow thrower;

  private void Awake() {
    SetupRigidbody();

    cameraFollow = new CameraFollow(cameraFollowSettings);
    cameraRotation = new CameraRotation(cameraRotationSettings, userInput);
    thrower = new ObjectThrow();

    activeItemSelectedSubscriber.Subscribe(new ActiveItemSelectedEvent(),
                                           message => {
                                             if (message.item == null) return;
                                             var itemGameObject = ((CollectableItem)message.item).gameObject;
                                             if (itemGameObject.OrNull() != null && itemGameObject != activeItem) {
                                               if (activeItem) activeItem.SetActive(false);
                                               activeItem = itemGameObject;
                                               activeItem.transform.SetParent(itemHolder.transform);
                                               activeItem.transform.localPosition = Vector3.zero;
                                               activeItem.transform.localRotation = Quaternion.identity;
                                               activeItem.SetActive(true);
                                               activeItem.GetComponent<WeaponShoot>().enabled = true;
                                             }
                                           });

    userInput.Throw.Where(value => value).Subscribe(_ => thrower.ThrowObject(itemHolder.transform, throwAngle, throwForce, grenadePrefab)).AddTo(this);

  }

  private void SetupRigidbody() {
    mainRigidbody = GetComponent<Rigidbody>();
    mainRigidbody.freezeRotation = true;
  }

  private void Update() {
    if (userInput.Jump.Value && grounded) Jump();
    ApplySpeedControl(grounded ? moveSpeed : airMoveSpeed);
    ApplyDamping();
  }

  private void LateUpdate() {
    cameraRotation.Update();
    cameraFollow.Update();
    UpdateObjectHolder();
  }

  private void FixedUpdate() {
    CheckGround();
    MovePlayer(userInput.Move.Value, grounded ? moveSpeed : airMoveSpeed);
    if (jumping) jumping = false;
  }

  private void UpdateObjectHolder() => itemHolder.transform.rotation = cameraRotationSettings.CameraTransform.rotation;

  private void ApplyDamping() => mainRigidbody.linearDamping = grounded ? groundDamping : airDamping;

  private void ApplySpeedControl(float speed) {
    var currentVelocity = mainRigidbody.linearVelocity;

    if (onSlope && !jumping) {
      if (currentVelocity.magnitude > slopeMaxAngle) mainRigidbody.linearVelocity = currentVelocity.normalized * speed;
    } else {
      var movementVelocity = currentVelocity.With(y: 0);
      if (movementVelocity.magnitude > speed) mainRigidbody.linearVelocity = (movementVelocity.normalized * speed).With(y: currentVelocity.y);
    }
  }

  private void MovePlayer(Vector2 direction, float speed) {
    var movement = orientation.transform.right * direction.x + orientation.transform.forward * direction.y;

    if (onSlope) {
      movement = Vector3.ProjectOnPlane(movement.normalized, slopeNormal);
      if (mainRigidbody.linearVelocity is { y: > 0 or < 0 }) mainRigidbody.AddForce(Vector3.down * slopePressForce, ForceMode.Force);
    }

    mainRigidbody.AddForce(movement.normalized * (speed * SPEED_MULTIPLIER), ForceMode.Force);
  }

  private void Jump() {
    mainRigidbody.linearVelocity = mainRigidbody.linearVelocity.With(y: 0);
    mainRigidbody.AddForce(transform.up * Mathf.Sqrt(2 * jumpHeight + mainRigidbody.linearDamping * Physics.gravity.magnitude), ForceMode.Impulse);
    grounded = false;
    jumping = true;
  }

  private void CheckGround() {
    var position = transform.position;

    grounded = Physics.Raycast(
      position,
      Vector3.down,
      out var _,
      raycastDistance,
      groundMask
    );

    Physics.Raycast(
      position,
      Vector3.down,
      out var result,
      raycastDistance * 1.5f,
      groundMask
    );

    var angle = Vector3.Angle(result.normal, Vector3.up);
    onSlope = angle != 0 && angle <= slopeMaxAngle;
    slopeNormal = result.normal;
    mainRigidbody.useGravity = !onSlope;
  }
}