using UnityEngine;

public class SoundManager : MonoBehaviour
{
	public static SoundManager Singleton;

	public AudioSource miscSounds;
	
	public AudioClip gunEquipSound;
	public AudioClip pickupSound;
	public AudioClip shootSound;
	public AudioClip chewingSound;
	public AudioClip finalMunchingSound;
	public AudioClip levelCompleteSound;
	public AudioClip gulpingSound;
	public AudioClip moneySound;
	public AudioClip hmmSound;
	private void Awake()
	{
	#region Singleton
		if (!Singleton)
			Singleton = this;
		else
			Destroy(gameObject);
	#endregion
	}

	public void PlaySound(AudioClip clip)
	{
		miscSounds.PlayOneShot(clip);
	}

}
