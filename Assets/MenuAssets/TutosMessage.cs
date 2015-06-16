using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TutosMessage : MonoBehaviour {

	public Image tuto1;
	public Image tuto2;
	public Image tuto3;
	public Image tuto4;
	public Image tuto5;
	public Image tuto6;

	public float timeDisplay = 5f;

	private float localDeltaTime;

	// Use this for initialization
	void Awake () 
	{
	}
	
	// Update is called once per frame
	void Update () 
	{
		localDeltaTime = (Time.timeScale == 0) ? 1 : Time.deltaTime / Time.timeScale;
	}

	public IEnumerator Display (int picNumberToDisplay)
	{
		Image picToDisplay;

		picToDisplay = tuto1;

		if (picNumberToDisplay == 1)
			picToDisplay = tuto1;
		else if (picNumberToDisplay == 2)
			picToDisplay = tuto2;
		else if (picNumberToDisplay == 3)
			picToDisplay = tuto3;
		else if (picNumberToDisplay == 4)
			picToDisplay = tuto4;
		else if (picNumberToDisplay == 5)
			picToDisplay = tuto5;
		else if (picNumberToDisplay == 6)
			picToDisplay = tuto6;

		picToDisplay.enabled = true;

		yield return new WaitForSeconds (timeDisplay);

		picToDisplay.enabled = false;
	}

	public void Clear ()
	{
		tuto1.enabled = false;
		tuto2.enabled = false;
		tuto3.enabled = false;
		tuto4.enabled = false;
		tuto5.enabled = false;
		tuto6.enabled = false;
	}
}
