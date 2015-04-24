using UnityEngine;
using System.Collections;

public class MenuManager : MonoBehaviour {

	public Menu CurrentMenu;

	public void Start()
	{
		ShowMenu (CurrentMenu);
	}

	public void ShowMenu(Menu menu)
	{

		if (CurrentMenu != null)
			CurrentMenu.IsOpen = false;

		CurrentMenu = menu;
		CurrentMenu.IsOpen = true;
	
	}

	public void WebFacebook()
	{

		Application.OpenURL ("https://fr-fr.facebook.com/");

	}
		public void WebGooglePlus()
		{
			
			Application.OpenURL ("https://plus.google.com/");
			
		}
			public void Webtweet()
			{
				
				Application.OpenURL ("https://twitter.com/");
				
			}
				public void WebTrailler()
				{
					
					Application.OpenURL ("https://www.youtube.com/watch?v=IB7Cg6BAdo0");
					
				}
					public void WebAries()
					{
						
						Application.OpenURL ("http://www.ecolearies.fr/");
						
					}



	
}