using UnityEngine;
using System.Collections;

/*CE SCRIPT sert à controler le personnage principal, que ce soit dans le monde, ou bien à travers
 * son animateur. Il gère également les divers inputs du pad et/ou du clavier ainsi que le mode
 * dans lequel se trouve la caméra principale. */

public class CharacterControllerLogic : MonoBehaviour {

	bool canGetHit = true;

	public bool isActive = true; //LA VARIABLE ULTIME qui fait quand elle est false, la foncion update MEEEEURT dans d'horribles souffrances. 
	//Alors attention à ce que tu fait avec cette variable, garçon ! Tu ne voudrais rien regretter.

	//Var qui régit le mode dans lequel est le joueur (Body ou Soul)
	[HideInInspector]
	public enum PlayerMode { Body, Soul, BodyGhost };
	[HideInInspector]
	public PlayerMode Current = PlayerMode.Body;

	//Renseignement des divers gameObjects et scripts avec lesquels ce script communique
	public GameObject Soul;
	[HideInInspector]
	public Transform follow; //Cette variable permet de communiquer à la caméra ce qu'elle doit suivre.
	[SerializeField]
	Animator animator;
	[SerializeField]
	CharacterController controller;
	Transform SpawnedSoul;
	public GameObject shieldRock;
	GameObject spawnedShieldRock;

	//Diverses var pour gérer les mouvements et les animations/l'animateur
	float speed = 3f;
	float animSpeed = 0.0f;
	Vector3 direction = Vector3.zero;
	float floatDir = 0f;
	float horizontal = 0.0f;
	float vertical = 0.0f;
	float lookH =0;
	public float aimSpeed = 5;
	Vector3 moveDirection = Vector3.zero;
	public float maxSpeed = 5;
	Vector3 faceDirection = Vector3.zero;
	Vector3 tempMoveDir;

	//...Et variables pour le saut

	float gravity = 20;
	[SerializeField]
	float jumpSpeed = 8;
	[HideInInspector]
	public bool isAiming = false;

	//Var gérant l'attaque principale de Phalene (Alpha)
	Transform fist;
	Animator fistAnimator;
	bool isAttacking = false;

	//Var de déplacements et de comportement du mode ghost
	float ghostSpeed = 3; // La vitesse à laquelle se déplace le joueur en mode Ghost.
	[HideInInspector]
	public GameObject currentTargetedGhost = null;
	float ghostSpeedRotation = 5;
	bool rushToGhost = false;
	[HideInInspector]
	public int currentGhostNumber = 0; //Le numéro du ghost actuellement visé.
	bool finishedGhostMode = false;
	bool mustRotate = false;
	[HideInInspector]
	public bool arrivedCam = false; //Le mode ghost doit parfois attendre que la caméra soit arrivé à un endroit précis avant de continuer ses actions

	//Diverses variables et variables de communication entre gameobjects/scripts
	public float localDeltaTime;
	float damping = 1;
	GameObject TPSCam;
	ThirdPersonCamera sendToCam;
	SoulBar soulBarScript;
	
	// Ce qui suit gère la barre du mode Soul
	Texture progressBarEmpty;
	Texture progressBarFull;
	GameObject actionGhost;

	/*bool déterminant les actions qu'est en train d'effectuer le joueur. Permet de vérifier que certaines actions incompatibles ne puissent
	 * pas s'effectuer en meme temps*/
	bool shield = false; //Le bouclier de pierre est-il actif ?
	bool wasInTheAirLastFrame = false;

	//Var de statistiques du perso
	public float healthPoints = 10;
	public float minimumHeightDanger = 16;

	bool noMoreGhosts = false;
	bool finishedFist = false;

	//Toutes les variables faisant références à des objets ou scripts extérieurs sont renseignées ici.
	void Start () 
	{
		//Renomme le gameObject rattaché en "Beta" (A remplacer par Phalene dans la version finale)
		this.name = "Beta";
		TPSCam = Camera.main.gameObject;
		sendToCam = TPSCam.GetComponent<ThirdPersonCamera>();
		animator = GetComponent<Animator>();
		controller = GetComponent<CharacterController>();
		follow = this.transform;
		//On renseigne le gameObject et l'animator de l'animation du poing (ALPHA)
		fist = GameObject.Find ("Fist").transform;
		fistAnimator = fist.GetComponent<Animator>();
		soulBarScript = this.GetComponent<SoulBar>();
		//Set du poids des couches d'animations

			if(animator.layerCount >= 2)
				{

				animator.SetLayerWeight (1,1);

				}
	}

	void Update () {
		if(isActive){
		//Is the player still alive ?
		if(healthPoints<=0)
			Die();

		#region Trigger controls
		//Bouclier et visée précise
		if(Input.GetAxisRaw("Triggers") <= -.1f && Current == PlayerMode.Body){
			isAiming = true;
		}
		else{
			isAiming = false;
		}
		if(Input.GetAxisRaw("Triggers") >= .1f && Current == PlayerMode.Body && controller.isGrounded){
			shield = true;
			spawnShield();
		}
		else if(Input.GetAxisRaw("Triggers") <= 0){
			destroyShield();
			shield = false;
		}
		#endregion

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
		stickToWorldSpace(transform, sendToCam.transform, ref direction, ref floatDir, ref speed, false);
		#endregion

		#region aim mode behaviour
		if(!isAiming){
		//Cette section donne la direction à suivre à Phalene lorsque le joueur la dirige.
		Quaternion target = Quaternion.Euler(0, floatDir, 0);
		tempMoveDir = target * Vector3.forward * speed;
		tempMoveDir = transform.TransformDirection (tempMoveDir * maxSpeed); 
		moveDirection.x = tempMoveDir.x;
		moveDirection.z = tempMoveDir.z;
		} 

		if (isAiming){
			tempMoveDir = (Vector3.forward * vertical) + (Vector3.right * horizontal) * speed;
			tempMoveDir = transform.TransformDirection (tempMoveDir * maxSpeed);
			moveDirection.x = tempMoveDir.x;
			moveDirection.z = tempMoveDir.z;
		}
		#endregion

		#region animator instructions
		//Ce qui suit n'est effectué que si le joueur est en mode Body et qu'il est à terre
		if (animator && Current == PlayerMode.Body && controller.isGrounded)
		{
			arrivedCam = false;
			//On renseigne à l'animator la vitesse de déplacement actuelle de Phalene
			if(!shield)
			animator.SetFloat ("Speed", animSpeed);
			else
				animator.SetFloat ("Speed", 0);
		
			//Appuyer sur Jump déclenche l'animation et fait sauter le charController.
			if(Input.GetButton("Jump")){
				animator.SetBool ("Jump", true);
				moveDirection.y = jumpSpeed;
			//La variable d'animation de saut est désactivée si le joueur n'appuie plus dessus
			}else{
				animator.SetBool("Jump", false);
			}
		}
		#endregion

		#region buttons and mode changing
		//Le bouton d'action permet, en mode body, de frapper.
		if(Input.GetButtonDown ("Action") && Current == PlayerMode.Body){
			Hit(false);
		}

		//Si le joueur est en mode soul et qu'il cancel, tout les ghosts sont détruits, puis on revient en mode Body.
		if(Current == PlayerMode.Soul && Input.GetButtonDown ("Cancel")){
			GameObject[] allGhosts = GameObject.FindGameObjectsWithTag("Action Ghost");
			foreach (GameObject ghost in allGhosts){
				Destroy(ghost.gameObject);
			}
			exitGhostMode (sendToCam);
			GameObject.FindWithTag("TPS Soul").SendMessage ("DestroyMe");
		}

		//En appuyant sur le bouton de switch de mode...
		if(Input.GetKeyDown(KeyCode.E) || Input.GetButtonDown ("SwitchMode")){
				//Si on est en mode Body, on spawne l'ame, et la caméra la prend pour cible.
				if (Current == PlayerMode.Body && soulBarScript.barDisplay>=.99f){

					Current = PlayerMode.Soul;
					SpawnSoul();
					Debug.Log ("Now in Soul Mode");

				//Sinon, et si aucun ghost n'existe, on retourne en mode Body, et la caméra suit de nouveau le corps de Phalene
				}else if (Current == PlayerMode.Soul && GameObject.FindGameObjectWithTag("Action Ghost") == null){
				//Quitter le mode "Soul"
				GameObject.FindWithTag("TPS Soul").SendMessage ("DestroyMe");
				Current = PlayerMode.Body;
					Debug.Log ("Now in Body Mode");


				//Cependant, si un ghost existe déjà, on adopte le mode de suivi de ghost.
			}else if (Current == PlayerMode.Soul && GameObject.FindGameObjectWithTag("Action Ghost") != null){
				GameObject.FindWithTag("TPS Soul").SendMessage ("DestroyMe");
				sendToCam.ghostHitPos = transform.position;
				Current = PlayerMode.BodyGhost;
				BodyGhostBehaviourInit(sendToCam, ref currentTargetedGhost);
				Debug.Log ("Body script detected a ghost existing !");
			}
		}
		#endregion

		//Gestion du mode de la caméra en fonction du mode du joueur...
		#region send instructions to camera
		if (Current == PlayerMode.Body){
			sendToCam.camTarget = GameObject.FindWithTag ("Player").transform;
			sendToCam.CurrentCamMode = ThirdPersonCamera.CamMode.Body;
		}else if (Current == PlayerMode.Soul){
			sendToCam.camTarget = GameObject.FindWithTag ("TPS Soul").transform;
			sendToCam.CurrentCamMode = ThirdPersonCamera.CamMode.Soul;
		}else if (Current == PlayerMode.BodyGhost){
			sendToCam.CurrentCamMode = ThirdPersonCamera.CamMode.BodyGhost;
		}
		#endregion

		#region Follow Ghosts mode
		//Gère l'orientation du joueur en direction du prochain Ghost si on est en mode Follow Ghost
		if (Current == PlayerMode.BodyGhost && currentTargetedGhost != null && mustRotate && isAttacking==false && arrivedCam){
			Quaternion lookAtRotation;
			Quaternion doneRotation;
				lookAtRotation = Quaternion.LookRotation(currentTargetedGhost.transform.position - transform.position);
				// Will assume you mean to divide by damping meanings it will take damping seconds to face target assuming it doesn't move
				doneRotation = Quaternion.Slerp(transform.rotation, lookAtRotation, (localDeltaTime/damping)*ghostSpeedRotation);
				doneRotation = Quaternion.Euler(new Vector3(0,doneRotation.eulerAngles.y,0));
				transform.rotation = doneRotation;
			float angle = 10;
			Vector3 targetPoint = currentTargetedGhost.transform.position;
				targetPoint.y = transform.position.y;
			if ( Vector3.Angle(targetPoint - transform.position, transform.forward ) < angle && currentTargetedGhost != null) {
					Debug.Log ("Finished Rotation !");
					mustRotate = false;
				StartCoroutine(rushToGhostFunc());
				}
		}

			//Tant que la variable rushToGhost est vraie, Phalene se dirige vers le prochain Ghost
		if (rushToGhost && currentTargetedGhost !=null && arrivedCam){
			Vector3 targetedGhostPosition = currentTargetedGhost.transform.position;
			targetedGhostPosition.y-=1;
			transform.position = Vector3.Lerp (transform.position, targetedGhostPosition, (localDeltaTime*ghostSpeed/damping));
		}
		
			AnimatorStateInfo fistState = fistAnimator.GetCurrentAnimatorStateInfo(0);
			//Si la variable finishedGhostMode est vraie, on sort du mode Ghost (Dès qu'il n'y a plus aucun ghost, donc).
		if (finishedGhostMode){
				noMoreGhosts = false;
				finishedFist = false;
				exitGhostMode (sendToCam);
		} 
		#endregion

		#region fall damages
		if(!controller.isGrounded){
			wasInTheAirLastFrame = true;
		}

		if(controller.isGrounded && wasInTheAirLastFrame){
			wasInTheAirLastFrame = false;
			if (moveDirection.y<=-minimumHeightDanger){
				healthPoints -=1*(-moveDirection.y/10);
			}
		}
		#endregion

		#region apply movements & gravity
			//Cette section finale permet d'appliquer les déplacements et la gravité en mode Body
		

			if(!controller.isGrounded && Current == PlayerMode.Body)
		moveDirection.y -= gravity * Time.deltaTime;
			else if (Current!=PlayerMode.Body)
				moveDirection.y=0;

			if (Current == PlayerMode.Body){
			if(!shield)
		controller.Move(moveDirection *  Time.deltaTime);

			if(!isAiming && !shield){
		faceDirection = transform.position + moveDirection;
		faceDirection.y = transform.position.y;

			transform.LookAt (faceDirection);
			}else if(isAiming){
				transform.Rotate (Vector3.up, lookH * aimSpeed * localDeltaTime);
			}
		}
		#endregion

		}else{
			//On fait en sorte que l'animateur stoppe l'animation de course si jamais le joueur n'est pas actif.
			animator.SetFloat ("Speed", 0);
			if(Input.anyKeyDown){
				Debug.Log ("Reset Level...");
				Application.LoadLevel("MountainMap");
			}
		}



		}

	void OnControllerColliderHit(ControllerColliderHit hit) {
		if(hit.transform.CompareTag("Enemy Attack") || hit.transform.CompareTag("Trap")){
		if(canGetHit){
		healthPoints-=1;
		canGetHit = false;
		StartCoroutine(gotHitCoolDown());
		}
		}
	}

	IEnumerator gotHitCoolDown(){
		yield return new WaitForSeconds(50f*localDeltaTime);
		canGetHit = true;
		Debug.Log ("Can get hit again");
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

	//Cette fonction permet de spawner l'ame de Phalene
	void SpawnSoul(){
		//Décalage du point de spawn de l'ame par rapport à Phalene...
		Vector3 soulSpawnOffset = new Vector3(0,2,-2);
		Vector3 soulSpawnPoint = follow.transform.position + soulSpawnOffset;

		SpawnedSoul = Instantiate(Soul, soulSpawnPoint , follow.transform.rotation) as Transform;
	}

	//Cette fonction est appelée à la fin de l'animation de saut en marche
	void stopJump(){
		animator.SetBool ("Jump", false);
	}

	//Cette fonction initialise tout les paramètres du mode de suivi de Ghost.
	public void BodyGhostBehaviourInit(ThirdPersonCamera sendToCam, ref GameObject Ghost){

		Debug.Log ("Now in Follow Ghost Mode!");
		Ghost = GameObject.Find ("spawnedGhost_"+currentGhostNumber);
		mustRotate = true;

	}


		//Cette fonction est appelée chaque fois qu'un Ghost est touché par Phalene
	void touchedGhost (){
		sendToCam.ghostHitPos = transform.position;
			//On appelle la fonction qui déclenche l'attaque, en précisant qu'elle a été appelé par la collision entre Phalene et un ghost.
		Hit(true);
			//On ajoute +1 au chiffre désignant le ghost visé...
		currentGhostNumber+=1;
			//On stoppe les mouvements de rush
		rushToGhost = false;
			//Si le prochain ghost existe, on le vise et on active les mouvements de réorientation de Phalene
		if (GameObject.Find("spawnedGhost_"+currentGhostNumber) != null){
			Debug.Log ("Targeting spawned Ghost N°"+currentGhostNumber);
			currentTargetedGhost = GameObject.Find ("spawnedGhost_"+currentGhostNumber);
			mustRotate = true;

			//Sinon, on demande au mode BodyGhost de se terminer...
		}else{
			Debug.Log ("No more ghosts!");
			noMoreGhosts = true;
			currentGhostNumber = 0;
		}
	}

	//Quitter le ghost mode
	void exitGhostMode (ThirdPersonCamera sendToCam){
		finishedGhostMode = false;
		Debug.Log ("Exiting ghost mode...");
		rushToGhost = false;
		Current = PlayerMode.Body;
	}

	//Attaque de base
	void Hit(bool calledByGhost){
		if(Current == PlayerMode.BodyGhost)
			fistAnimator.speed = 100;
		else
			fistAnimator.speed = 1;

		fistAnimator.SetBool ("Punch", true);
		fist.SendMessage("Attack",true);
		isAttacking = true;
	}
	
	//Termine l'animation d'attaque
	void finishedAttack(bool calledByGhost){
		isAttacking = false;
		if(calledByGhost && noMoreGhosts){
			finishedGhostMode = true;
		}
	}

	#region jump animations
	//Gère les booléennes selon les animations lues par l'animator.
	public bool IsInJump()
	{
		return (IsInIdleJump() || IsInLocomotionJump());
	}
	
	public bool IsInIdleJump()
	{
		return animator.GetCurrentAnimatorStateInfo(0).IsName("Base Layer.IdleJump");
	}
	
	public bool IsInLocomotionJump()
	{
		return animator.GetCurrentAnimatorStateInfo(0).IsName("Base Layer.LocomotionJump");
	} 
	#endregion

	//Permet d'ajouter un petit temps d'attente avant que Phalene ne rushe sur le prochain Ghost
	IEnumerator rushToGhostFunc(){
		yield return new WaitForSeconds(.01f);
		rushToGhost = true;
	}
	
	void spawnShield(){
		//Vector3 myForward = transform.forward;
		if(GameObject.Find("rockShield")==null){
		spawnedShieldRock = Instantiate (shieldRock, transform.position + transform.forward*2, transform.rotation) as GameObject;
		spawnedShieldRock.gameObject.name="rockShield";
		}
	}

	void destroyShield(){
		if(spawnedShieldRock!=null)
			Destroy(spawnedShieldRock.gameObject);
	}

	void Die(){
		isActive = false;
	}

}//END OF SCRIPT
