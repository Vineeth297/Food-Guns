using DG.Tweening;
using UnityEngine;

public class BogusCode : MonoBehaviour
{
	public static BogusCode bogu;

	private void Awake() => bogu = this;

	public Transform cameraEndPoint, leftHandEndPoint, rightHandEndPoint;
	public Transform camera, leftHand, rightHand;
	public Transform playerFinalTransform;

	private void OnTriggerEnter(Collider other)
	{
		//disable all movement control on player
		//keep moving
		PlayerControl.single.xSpeed = 0f;

		//move left hand to desired posx
		//move right hand to desired posx
		/*
		DOTween.KillAll(false, leftHand);
		DOTween.KillAll(false, rightHand);
		DOTween.KillAll(false, leftHand.parent);
		DOTween.KillAll(false, rightHand.parent);
		
		leftHand.DOMove(leftHandEndPoint.position, 0.5f);
		rightHand.DOMove(rightHandEndPoint.position, 0.5f);
		*/

		PlayerControl.single.transform.DOMove(playerFinalTransform.position, 0.5f);

		//dont make camera child in the other script
		//tween to this pos
		//camera.transform.DOMove(cameraEndPoint.position, 0.5f);
	}
}
