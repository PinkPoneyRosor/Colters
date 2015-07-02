using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CreditsDefil : MonoBehaviour {

	public Image Credits;
	public bool go = false;

	private float localDeltaTime;
	private Vector2 startPos;

	// Use this for initialization
	void Start () 
	{
		//Credits.rectTransform.position = new Vector2 (Screen.width / 2, -Screen.height - 20);
		startPos = Credits.rectTransform.position;
	}
	
	// Update is called once per frame
	void Update () 
	{
		localDeltaTime = (Time.timeScale == 0) ? 1 : Time.deltaTime / Time.timeScale;

		if (go)
		{
			Credits.rectTransform.position += Vector3.up * 150 * localDeltaTime;
			Debug.Log ("Gogolol");
		}

		if (Credits.rectTransform.position.y > Screen.height + Credits.rectTransform.rect.height * 1.5f)
		{
			go = false;
			Application.LoadLevel(0);
		}
	}

	public void GoGoGo ()
	{
		go = true;
	}
}
