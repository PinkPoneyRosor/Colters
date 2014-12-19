using UnityEngine;
using System.Collections;

public class PlayerCommon : MonoBehaviour 
{	
	Vector3 tempMoveDir;
	Vector3 moveDirection = Vector3.zero;
	Vector3 faceDirection = Vector3.zero;
	ThirdPersonCamera mainCameraScript;
	Vector3 direction = Vector3.zero;
	float floatDir = 0f;
	float currentSpeed = 3f;

	void Start () 
	{
		mainCameraScript = Camera.main.GetComponent<ThirdPersonCamera> ();
	}

	public void DefaultMoves(GameObject player, CharacterController controller, float maxSpeed, float heightOfJump, float gravity)
	{
		#region default Controls
		#region setting essential variables each frame
		//This method will translate axis input into world coordinates, according to the camera's point of view.
		stickToWorldSpace(player.transform, mainCameraScript.transform, ref direction, ref floatDir, ref currentSpeed, false);
		#endregion
		
		Quaternion target = Quaternion.Euler(0, floatDir, 0);
		tempMoveDir = target * Vector3.forward * currentSpeed;
		tempMoveDir = transform.TransformDirection (tempMoveDir * maxSpeed); 
		moveDirection.x = tempMoveDir.x;
		moveDirection.z = tempMoveDir.z;
		
		if (Input.GetButton ("Jump") && controller.isGrounded)
			moveDirection.y = heightOfJump;
		
		#region apply movements & gravity
		//This final section will apply movements and gravity.
		if(!controller.isGrounded)
			moveDirection.y -= gravity * Time.deltaTime;
		
		controller.Move(moveDirection * Time.deltaTime);
		
		faceDirection = transform.position + moveDirection;
		faceDirection.y = transform.position.y;
		
		transform.LookAt (faceDirection);
		#endregion
		#endregion
	}

	//This is where the magic happens, this method translate the left stick coordinates into world space coordinates, according to the camera's point of view!
	void stickToWorldSpace(Transform root, Transform camera, ref Vector3 directionOut, ref float floatDirOut, ref float speedOut, bool outForAnim)
	{
		//We take the model's direction, the stick's direction, then we put in the square magnitude.
		Vector3 rootDirection = root.forward;
		Vector3 stickDirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
		speedOut = stickDirection.sqrMagnitude;
		
		//Getting the camera's current rotation.
		Vector3 CameraDirection = camera.forward;
		CameraDirection.y = 0.0f;
		Quaternion referentialShift = Quaternion.FromToRotation (Vector3.forward, CameraDirection);
		
		//Conversion de l'input du joystick/clavier en coordonnées World.
		Vector3 moveDirection = referentialShift * stickDirection;
		Vector3 axisSign = Vector3.Cross(moveDirection, rootDirection);
		
		#region debug draw rays
		//These lines allow us to see how the vectors are handled for debug.
		
		/*Debug.DrawRay (new Vector3(root.position.x, root.position.y + 2f, root.position.z), moveDirection, Color.green);
		Debug.DrawRay (new Vector3(root.position.x, root.position.y + 2f, root.position.z), axisSign, Color.red);
		Debug.DrawRay (new Vector3(root.position.x, root.position.y + 2f, root.position.z), rootDirection, Color.magenta);
		Debug.DrawRay (new Vector3(root.position.x, root.position.y + 2f, root.position.z), stickDirection, Color.blue);*/
		#endregion
		
		//Give the angle between the model's direction and the direction we give to it.
		float angleRootToMove = Vector3.Angle (rootDirection, moveDirection) * (axisSign.y >= 0 ? -1f :1f);
		
		//DirectionOut will be useful to give the direction where the character have to go to the animator.
		floatDirOut = angleRootToMove;
	}
}
