using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AudioVolume : MonoBehaviour {

	public Text	text;
	public Slider slider;

	public void AudioText () {     // Affiche la valeur du slider

		text.text = slider.value.ToString ();

	}
	

}
