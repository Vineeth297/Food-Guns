using UnityEngine;
using System;
using Unity.VisualScripting;

public class GameEvents : MonoBehaviour
{
    public static GameEvents Ge;

	public Action<HandGunController, GameObject,GameObject> ammoFound;

	public Action aimModeSwitch;

	public Action startFeeding;

	public Action stopCamera;

	public Action obstacleHeadEatingBurger;

	public Action startShooting;
	public Action stopShooting;

	public Action obstacleHeadAimSwitch;

	public Action continueFollowing;

	private void Awake()
	{
		if(!Ge) Ge = this;
		else Destroy(gameObject);
	}

	public void InvokeOnAmmoFound(HandGunController hand,GameObject ammo,GameObject mag) => ammoFound?.Invoke(hand,ammo,mag);
	public void InvokeOnAimModeSwitch() => aimModeSwitch?.Invoke();
	public void InvokeOnStartFeeding() => startFeeding?.Invoke();
	public void InvokeStopCamera() => stopCamera?.Invoke();
	public void InvokeStartShooting() => startShooting?.Invoke();
	public void InvokeStopShooting() => stopShooting?.Invoke();
	public void InvokeObstacleHeadEatingBurger() => obstacleHeadEatingBurger?.Invoke();
	public void InvokeObstacleHeadAimSwitch() => obstacleHeadAimSwitch?.Invoke();
	public void InvokeOnContinueFollowing() => continueFollowing?.Invoke();
}
