using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	float horizontal = 0.0f;
	float vertical = 0.0f;
	float lookH =0;
    float localDeltaTime;
	float animSpeed = 0.0f;
	Vector3 moveDirection = Vector3.zero;
	Vector3 faceDirection = Vector3.zero;

	ThirdPersonCamera mainCameraScript;

	Vector3 direction = Vector3.zero;
	float floatDir = 0f;
	float speed = 3f;

	CharacterController controller;
	float gravity = 20;
	Vector3 tempMoveDir;
	public float maxSpeed = 5;

	// Use this for initialization
	void Start () {
		controller = GetComponent<CharacterController>();
		mainCameraScript = Camera.main.GetComponent<ThirdPersonCamera> ();
	}
	
	// Update is called once per frame
	void Update () {

		#region Get Axises
		//Recuperation des axes des input (Clavier, stick...)
		horizontal = Input.GetAxis ("Horizontal");
		vertical = Input.GetAxis ("Vertical");
		lookH = Input.GetAxis ("LookH");
		#endregion

		#region setting essential variables each frame
		//localDeltaTime permet au script de ne pas etre influencé par le changement de TimeScale.
		localDeltaTime = (Time.timeScale == 0) ? 1 : Time.deltaTime / Time.timeScale;
		
		//Variable determinant la vitesse de déplacement du personnage pour le blending d'animation
		animSpeed = new Vector2(horizontal,vertical).sqrMagnitude;
		
		//Appel de la fonction transformant les coordonnées données par le stick en coordonnées spatiales
		stickToWorldSpace(transform, mainCameraScript.transform, ref direction, ref floatDir, ref speed, false);
		#endregion

		Quaternion target = Quaternion.Euler(0, floatDir, 0);
		tempMoveDir = target * Vector3.forward * speed;
		tempMoveDir = transform.TransformDirection (tempMoveDir * maxSpeed); 
		moveDirection.x = tempMoveDir.x;
		moveDirection.z = tempMoveDir.z;

		#region apply movements & gravity
		//Cette section finale permet d'appliquer les déplacements et la gravité en mode Body
				
		if(!controller.isGrounded)
			moveDirection.y -= gravity * Time.deltaTime;

			controller.Move(moveDirection * Time.deltaTime);
			
			faceDirection = transform.position + moveDirection;
			faceDirection.y = transform.position.y;
				
			transform.LookAt (faceDirection);
		#endregion
	}

	//Cette fonction permet de traduire les coordonnées du stick en coordonnées spatiales (liées au monde)
	public void stickToWorldSpace(Transform root, Transform camera, ref Vector3 directionOut, ref float floatDirOut, ref float speedOut, bool outForAnim)
	{
		//On prend la direction du model, la direction du stick, et on renseigne la magnitude au carré (Ceci, dans un soucis d'optimisation)
		Vector3 rootDirection = root.forward;
		Vector3 stickDirection = new Vector3(horizontal, 0, vertical);
		speedOut = stickDirection.sqrMagnitude;
		
		//Obtention de la rotation de la caméra
		Vector3 CameraDirection = camera.forward;
		CameraDirection.y = 0.0f;
		Quaternion referentialShift = Quaternion.FromToRotation (Vector3.forward, CameraDirection);
		
		//Conversion de l'input du joystick/clavier en coordonnées World.
		Vector3 moveDirection = referentialShift * stickDirection;
		Vector3 axisSign = Vector3.Cross(moveDirection, rootDirection);
		
		#region debug draw rays
		//Ces lignes permettent de visualiser la façon dont sont gérés les vecteurs dans la fonction StickToWorldSpace (Debug)
		
		/*Debug.DrawRay (new Vector3(root.position.x, root.position.y + 2f, root.position.z), moveDirection, Color.green);
		//Debug.DrawRay (new Vector3(root.position.x, root.position.y + 2f, root.position.z), axisSign, Color.red);
		Debug.DrawRay (new Vector3(root.position.x, root.position.y + 2f, root.position.z), rootDirection, Color.magenta);
		//Debug.DrawRay (new Vector3(root.position.x, root.position.y + 2f, root.position.z), stickDirection, Color.blue);*/
		#endregion
		
		//Donne l'angle entre la direction du model et la direction du mouvement qu'on lui donne.
		float angleRootToMove = Vector3.Angle (rootDirection, moveDirection) * (axisSign.y >= 0 ? -1f :1f);
		
		//DirectionOut servira à donner la direction dans laquelle le personnage se dirige à l'animateur
		floatDirOut = angleRootToMove;
	}
}
