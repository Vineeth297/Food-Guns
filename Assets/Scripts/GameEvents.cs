using UnityEngine;
using System;

public class GameEvents : MonoBehaviour
{
    public static GameEvents Ge;

	public Action<HandGunController, GameObject> onAmmoFound;
	private void Awake()
	{
		if(!Ge) Ge = this;
		else Destroy(gameObject);
	}

	public void InvokeOnAmmoFound(HandGunController hand,GameObject ammo) => onAmmoFound?.Invoke(hand,ammo);
}
