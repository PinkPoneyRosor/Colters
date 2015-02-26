using UnityEngine;
using System.Collections;

public class Floater : MonoBehaviour {
	public float waterLevel, floatHeight;
	public Vector3 buoyancyCentreOffset;
	public float bounceDamp;
	public LayerMask waterLayer;
	
	
	private Vector3 flowDirection = Vector3.zero;
	private bool inWater = false;
	private GameObject hitWater;
	private WaterVolume hitWaterScript;
	
	

	void FixedUpdate () {
		if(inWater)
		{
			//Let's get the surface's height
			RaycastHit hit;
			if (Physics.Raycast(transform.position + transform.up * 3, -transform.up, out hit, 10, waterLayer))
			{
				waterLevel = hit.point.y;
				
				if(hit.normal != Vector3.up)
				flowDirection = hit.normal;
				else
				flowDirection = hitWaterScript.streamDirection;
				
				Debug.Log (flowDirection.normalized);
			}	
		
			Vector3 actionPoint = transform.position + transform.TransformDirection(buoyancyCentreOffset);
			
			if(flowDirection.y != 0)
				actionPoint -= Vector3.up * -1f;
			
			float forceFactor = 1f - ((actionPoint.y - waterLevel) / floatHeight);
			
			if (forceFactor > 0f) 
			{
				Vector3 uplift = -Physics.gravity * (forceFactor - rigidbody.velocity.y * bounceDamp);
	
				
				rigidbody.AddForceAtPosition(uplift, actionPoint);
			}
			
			if(flowDirection != Vector3.zero)
			{
				rigidbody.AddForce(flowDirection.normalized * hitWaterScript.waterStreamSpeed);
				//Making sure speed won't be too fast
				rigidbody.velocity = rigidbody.velocity.normalized * hitWaterScript.waterStreamSpeed;
			}
		}
		
		
		
	}
	
	void OnTriggerEnter(Collider hit)
	{
		if (hit.CompareTag("Water"))
		{
			inWater = true;
			hitWater = hit.gameObject;
			hitWaterScript = hitWater.GetComponent <WaterVolume>();
		}
	}
}
