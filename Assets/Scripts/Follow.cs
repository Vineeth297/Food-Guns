using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Follow : MonoBehaviour
{
    public Vector3 followOffset;
	public Transform charToFollow;
	public float damping;

	public bool startFollowing;

	private void LateUpdate()
	{
	    if(startFollowing)
			FollowTheGuy();
	}

	private void FollowTheGuy()
	{
		if (charToFollow != null)
		{
			Vector3 smoothPos = Vector3.Lerp(transform.position, charToFollow.position + followOffset,
				Time.deltaTime * damping);
			transform.position = smoothPos;
			transform.eulerAngles = charToFollow.eulerAngles;
		}
	}
}
