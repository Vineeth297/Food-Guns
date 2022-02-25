using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class SellingMachine : MonoBehaviour
{
	[SerializeField] private GameObject parent;
	[SerializeField] private GameObject delivery;

	[SerializeField] private TMP_Text comboText;
	[SerializeField] private ComboText comboComponent;
	
	private float _initFontSize;
	
	private void Start()
	{
		comboText.enabled = false;
		_initFontSize = comboText.fontSize;
	}
	
	private void OnTriggerEnter(Collider other)
	{
		if (!other.CompareTag("Solids") && !other.CompareTag("Liquids")) return;
		
		other.GetComponent<Collider>().enabled = false;
		var collectible = other.GetComponent<Collectible>();
		if (other.CompareTag("Solids"))
			collectible.leftAmmoIndex = 0;
			
		SoundManager.Singleton.PlaySound(SoundManager.Singleton.moneySound);
		collectible.enabled = false;
		collectible.handGunController.myAmmo.Remove(other.gameObject);

		StartCoroutine(Refollow(other.gameObject, collectible));
		
		/*if (other.CompareTag("Solids"))
		{
			var theSecondLeftFollower = collectible.handGunController.myAmmo[0];
			theSecondLeftFollower.GetComponent<Collectible>().ammoToFollow = collectible.handGunController.leftMagPos.transform;
		}
		if (other.CompareTag("Liquids"))
		{
			var theSecondRightFollower= collectible.handGunController.myAmmo[0];
			theSecondRightFollower.GetComponent<Collectible>().ammoToFollow = collectible.handGunController.rightMagPos.transform;
		}*/
			
		comboComponent.ComboSelling(comboText,_initFontSize);
			
		other.transform.parent = parent.transform;
		other.transform.DOMove(delivery.transform.position, 2f).OnComplete(() =>
		{
			other.gameObject.SetActive(false);
		});
	}

	private IEnumerator Refollow(GameObject secondFollower,Collectible collectible)
	{
		if (secondFollower.CompareTag("Solids"))
		{
			var theSecondLeftFollower = collectible.handGunController.myAmmo[0];
			theSecondLeftFollower.GetComponent<Collectible>().ammoToFollow = collectible.handGunController.leftMagPos.transform;
		}
		if (secondFollower.CompareTag("Liquids"))
		{
			var theSecondRightFollower= collectible.handGunController.myAmmo[0];
			theSecondRightFollower.GetComponent<Collectible>().ammoToFollow = collectible.handGunController.rightMagPos.transform;
		}
		
		yield return new WaitForSeconds(0.5f);
	}
}
