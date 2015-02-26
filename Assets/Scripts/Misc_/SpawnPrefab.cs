using UnityEngine;
using System.Collections;

class SpawnPrefab : MonoBehaviour 
{
	public Component[] allTrapRocks;
	public GameObject TrapV2Prefab = null;
	public Transform spawnPosition = null;
	public GameObject spawnedTrap;
	public float speed= 5;
	
	void Start ()
	{
		spawnedTrap = Instantiate(TrapV2Prefab, transform.localPosition, transform.localRotation) as GameObject;
		allTrapRocks = spawnedTrap.GetComponentsInChildren <Rigidbody>();
	}

	void OnTriggerEnter(Collider c)
	{
		if (c.gameObject.tag == "ThrowableRock" && spawnedTrap != null)
		{
			
			foreach (Rigidbody child in allTrapRocks)
			{
				child.rigidbody.constraints = RigidbodyConstraints.FreezeRotation;

				child.transform.Translate(Vector3.down * speed * Time.deltaTime);
			}
			
			StartCoroutine ("DoSomethingElse");
		}
	}

	IEnumerator DoSomethingElse ()
	{
		yield return new WaitForSeconds(5f);	
		Destroy(spawnedTrap.gameObject);
		spawnedTrap = Instantiate(TrapV2Prefab, transform.localPosition, transform.localRotation) as GameObject;
		allTrapRocks = spawnedTrap.GetComponentsInChildren <Rigidbody>();
	}
}