using UnityEngine;
using System;
using UnityEngine.Serialization;

public class GameEvents : MonoBehaviour
{
    public static GameEvents Ge;

	public Action<HandGunController, GameObject,GameObject> ammoFound;

	public Action aimModeSwitch;

	public Action startFeeding;
	
	public Action pickupReaction;

	private void Awake()
	{
		if(!Ge) Ge = this;
		else Destroy(gameObject);
	}

	public void InvokeOnAmmoFound(HandGunController hand,GameObject ammo,GameObject mag) => ammoFound?.Invoke(hand,ammo,mag);
	public void InvokeOnAimModeSwitch() => aimModeSwitch?.Invoke();
	public void InvokeOnStartFeeding() => startFeeding?.Invoke();
	public void InvokePickUpReaction() => pickupReaction?.Invoke();

}
