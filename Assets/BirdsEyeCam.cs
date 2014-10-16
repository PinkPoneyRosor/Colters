using UnityEngine;
using System.Collections;

public class BirdsEyeCam : MonoBehaviour {
	
	Transform soul;
	Transform player;
	float yaw;
	float roll;

	public bool followBody = false;

	// Use this for initialization
	void Start () {
		soul = GameObject.Find ("Soul").transform;
		player = GameObject.Find ("Player").transform;
	}
	
	// Update is called once per frame
	void LateUpdate () {

		if (!followBody) {
					soul = GameObject.Find ("Soul").transform;
					this.transform.position = soul.position + new Vector3 (0, 10, 0) + Vector3.forward * -5 + Vector3.right * 2;
					this.transform.LookAt (soul.transform.position);
					yaw = transform.eulerAngles.y;
				} else {
					this.transform.position = player.position + new Vector3 (0, 3, 0) + Vector3.forward * -3 + Vector3.right * 1;
					this.transform.LookAt (player.transform.position);
				}
		}
}
