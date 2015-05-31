using UnityEngine;
using System.Collections;

public class SoundAutoKill : MonoBehaviour {

	AudioSource audioSource;
	public bool randomizePitch = true;
	public bool pitchLadder = false;

	// Use this for initialization
	void Start () {
	audioSource = this.GetComponent <AudioSource> ();
	
	if(randomizePitch)
		audioSource.pitch = Random.Range (.5f , 1);
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(!audioSource.isPlaying)
		{
			this.Destroy (gameObject);
		}
	}
}
