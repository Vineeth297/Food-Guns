using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HandGunController : MonoBehaviour
{
	public List<GameObject> myAmmo;

	private void Start()
	{
		myAmmo = new List<GameObject>();
	}
}
