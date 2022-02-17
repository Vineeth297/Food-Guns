using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AnimationEvent : ObstacleHead
{
	public Animator animator;
	public void OpenMouth()
	{
		animator.speed = 0;
		//animator.speed = 0;
	}
	public void CloseMouth()
	{
		animator.speed = 1;
	}
}
