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
			
		collectible.enabled = false;
		collectible.handGunController.myAmmo.Remove(other.gameObject);
			
		comboComponent.ComboSelling(comboText,_initFontSize);
			
		other.transform.parent = parent.transform;
		other.transform.DOMove(delivery.transform.position, 2f).OnComplete(() =>
		{
			other.gameObject.SetActive(false);
		});
	}
}
