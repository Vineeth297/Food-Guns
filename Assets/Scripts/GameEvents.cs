using UnityEngine;
using System;
using UnityEngine.Serialization;

public class GameEvents : MonoBehaviour
{
    public static GameEvents Ge;

	public Action<HandGunController, GameObject,GameObject> ammoFound;
	public Action<HandGunController, GameObject,GameObject> leftAmmoFound;
	public Action<HandGunController, GameObject,GameObject> rightAmmoFound;

	public Action aimModeSwitch;
	private void Awake()
	{
		if(!Ge) Ge = this;
		else Destroy(gameObject);
	}

	public void InvokeOnAmmoFound(HandGunController hand,GameObject ammo,GameObject mag) => ammoFound?.Invoke(hand,ammo,mag);
	public void InvokeOnLeftAmmoFound(HandGunController hand,GameObject ammo,GameObject mag) => leftAmmoFound?.Invoke(hand,ammo,mag);
	public void InvokeOnRightAmmoFound(HandGunController hand,GameObject ammo,GameObject mag) => rightAmmoFound?.Invoke(hand,ammo,mag);

	public void InvokeOnAimModeSwitch() => aimModeSwitch?.Invoke();
}
