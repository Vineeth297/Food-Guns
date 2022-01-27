using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class GameEvents : MonoBehaviour
{
    public static GameEvents Ge;

	public event Action OnCollection;
	private void Awake()
	{
		Ge = this;
	}

	public void InvokeOnCollection() => OnCollection?.Invoke();
}
