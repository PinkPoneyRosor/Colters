using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HowToNavigation : MonoBehaviour {

	public Image page1;
	public Image page2;
	public Image page3;
	public Image page4;
	public Image page5;
	public Image page6;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Next ()
	{
		if (page1.enabled)
		{
			page1.enabled = false;
			page2.enabled = true;
		}
		else if (page2.enabled)
		{
			page2.enabled = false;
			page3.enabled = true;
		}
		else if (page3.enabled)
		{
			page3.enabled = false;
			page4.enabled = true;
		}
		else if (page4.enabled)
		{
			page4.enabled = false;
			page5.enabled = true;
		}
		else if (page5.enabled)
		{
			page5.enabled = false;
			page6.enabled = true;
		}
		else if (page6.enabled)
		{
			page6.enabled = false;
			page1.enabled = true;
		}
	}

	public void Previous ()
	{
		if (page6.enabled)
		{
			page6.enabled = false;
			page5.enabled = true;
		}
		else if (page5.enabled)
		{
			page5.enabled = false;
			page4.enabled = true;
		}
		else if (page4.enabled)
		{
			page4.enabled = false;
			page3.enabled = true;
		}
		else if (page3.enabled)
		{
			page3.enabled = false;
			page2.enabled = true;
		}
		else if (page2.enabled)
		{
			page2.enabled = false;
			page1.enabled = true;
		}
		else if (page1.enabled)
		{
			page1.enabled = false;
			page6.enabled = true;
		}
	}
}
