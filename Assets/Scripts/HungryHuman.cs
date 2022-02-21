using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class HungryHuman : MonoBehaviour
{
	public Animator animator;
	
	private static readonly int OpenMouth = Animator.StringToHash("OpenMouth");
	private static readonly int CloseMouth = Animator.StringToHash("CloseMouth");

	private void OnEnable()
	{
		GameEvents.Ge.startFeeding += StartFeeding;
	}

	private void OnDisable()
	{
		GameEvents.Ge.startFeeding -= StartFeeding;
	}

	private void StartFeeding()
	{
		transform.DORotate(Vector3.zero, 2f).OnComplete(() =>
		{
			animator.SetTrigger(OpenMouth);
			GameEvents.Ge.InvokeStartShooting();
		});
	}

	public void Eat()
	{
		animator.SetTrigger(CloseMouth);
	}
}
