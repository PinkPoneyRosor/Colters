using UnityEngine;
using System.Collections;

public class SpawnPrefab2 : MonoBehaviour {

	public Component[] allTrapRocks;
	public GameObject TrapV3Prefab;
	public Transform spawnPosition;
	public GameObject spawnedTrap;

	public float speed = 5;
	public bool falling = false;


	// Use this for initialization
	void Start () {
		spawnedTrap = Instantiate(TrapV3Prefab, transform.localPosition, transform.localRotation) as GameObject;
		allTrapRocks = spawnedTrap.GetComponents<Transform>();
	}


	void OnTriggerEnter(Collider c)
	{
		if (c.gameObject.tag == "ThrowableRock" && spawnedTrap != null)
		{
			falling = true;
			StartCoroutine ("DoSomethingElse");
		}
	}



	
	// Update is called once per frame
	void Update () {
		if(falling == true){
			foreach (Transform child in allTrapRocks)
			{
				child.transform.Translate(Vector3.down * speed * Time.deltaTime);
			}
		
	}
}

	IEnumerator DoSomethingElse ()
	{
		yield return new WaitForSeconds(3f);	
		Destroy(spawnedTrap.gameObject);
		spawnedTrap = Instantiate(TrapV3Prefab, transform.localPosition, transform.localRotation) as GameObject;
		allTrapRocks = spawnedTrap.GetComponentsInChildren <Transform>();
		falling = false;
	}

}
