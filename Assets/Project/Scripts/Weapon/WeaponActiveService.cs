using UnityEngine;

public class WeaponActiveService {
  private GameObject activeItem;
  
  public GameObject GetActiveItem() => activeItem;

  public void SetActiveItem(WeaponShoot weapon, Transform itemHolder) {
    var gameObject = weapon.gameObject;
    
    if (gameObject == null || gameObject == activeItem) return;

    if (activeItem) activeItem.SetActive(false);
    activeItem = gameObject;
    activeItem.transform.SetParent(itemHolder.transform);
    activeItem.transform.localPosition = Vector3.zero;
    activeItem.transform.localRotation = Quaternion.identity;
    activeItem.SetActive(true);
    weapon.enabled = true;
  }
}