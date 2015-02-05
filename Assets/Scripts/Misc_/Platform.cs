using UnityEngine;
using System.Collections;

public class Platform : MonoBehaviour {

	public float speed = 2;
	
	// Update is called once per frame
	void Update () {
		transform.Translate(Vector3.right * speed * Time.deltaTime);
	}
}
