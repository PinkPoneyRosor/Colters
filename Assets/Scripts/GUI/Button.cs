using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Button : MonoBehaviour {


	public void ClickMenu ()  //Affiche la fonction dans le OnClick (event Button Script)
	{
		Application.LoadLevel(0);
	}
	public void ClickPlay () 
		{
			Application.LoadLevel(1);
		}
	public void ClickHowToPlay ()
		{
			Application.LoadLevel(2);
		}
	public void ClickOption ()
		{
			Application.LoadLevel(3);
		}
	public void ClickCredit ()
		{
			Application.LoadLevel(4);
		}
	public void ClickLeave()
		{
			Application.Quit();
		}
	
}
