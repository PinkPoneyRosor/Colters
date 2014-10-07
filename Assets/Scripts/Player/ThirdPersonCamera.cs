using UnityEngine;
using System.Collections;

public class ThirdPersonCamera : MonoBehaviour {


	[SerializeField]
	public float smooth;	//Multiplicateur de smoothing de mouvement
	private Vector3 offset = new Vector3(0f, 1.3f, 0f); //décalage de la cam
	[HideInInspector]
	public Vector3 aimOffset = Vector3.zero;
	[HideInInspector]
	public float lookHSpeed = 150;

	//Renseignement des assets avec lesquels le script communique, ainsi que leurs position, etc...
	[HideInInspector]
	public Transform camTarget; //Ce que la caméra suit
	private Vector3 targetPosition; //Position de la cible                                                         
	GameObject currentTargetedGhost;
	[HideInInspector]
	public int currentGhostNumber = 0;
	Transform mainChar;
	PlayerController characterScript;
	[HideInInspector]
	public Vector3 ghostHitPos = Vector3.zero;

	//Var controlant la position et les mouvements de la cam
	private Vector3 lookDir; //Direction dans laquelle est en train de regarder la cam
	[HideInInspector]
	public float damping = 6.0f;	//to control the rotation 
	private Transform _myTransform;
	float lookH = 0;
	public Quaternion rotationOffset = Quaternion.identity;
	public Quaternion soulRotationOffset;
	[HideInInspector]
	public Quaternion aimRotationOffset = Quaternion.identity;
	float aimSpeed;
	
	//Divers
	float localDeltaTime;
	[HideInInspector]
	public enum CamMode { Body, Soul, BodyGhost }; //Définit le mode actuel dans lequel se trouve la cam
	[HideInInspector]
	public CamMode CurrentCamMode = CamMode.Body;
	bool aimCamMode = false;
	Transform aimCamTarget;
	
	public float distance = 10.0f;

	public float xSpeed = 250.0f;
	public float ySpeed = 120.0f;
	
	public float yMinLimit = -20f;
	public float yMaxLimit = 80f;
	public float xAimMinLimit = -80f;
	public float xAimMaxLimit = 80;
	
	float x = 0.0f;
	float y = 50f;

	bool justExitAimMode = true;

	bool justExitBaseCamMode = false;

	public bool invertedVerticalAxis;

	public Texture2D CrosshairTexture;
	Rect positionch;

	public LayerMask CompensateLayer;


	void Awake() {
		_myTransform = transform;
	}

	// Use this for initialization
	void Start () {
		//La caméra regarde le joueur au départ
		camTarget = GameObject.FindWithTag ("Player").transform;
		mainChar = GameObject.FindWithTag ("Player").transform;
		characterScript = mainChar.GetComponent<PlayerController>();

		float x = 0.0f;
		float y = 50f;
	}
	
	// Update is called once per frame
	void Update () {
	}

	//LateUpdate est appelée à chaque frame APRES toutes les fonctions Update
	void LateUpdate()
	{
		localDeltaTime = (Time.timeScale == 0) ? 1 : Time.deltaTime / Time.timeScale;
		Vector3 characterOffset = Vector3.zero;

		if(camTarget!=null)
		characterOffset = camTarget.localPosition + offset;

		//Lorsque la caméra est en mode Body...
		if (CurrentCamMode == CamMode.Body){
		
			currentGhostNumber = 0;

			//On calcule la direction venant de la caméra et allant vers le joueur, on neutralise l'axe Y, et on normalise pour pouvoir utiliser la magnitude.
			lookDir = characterOffset - transform.position;
			lookDir.y = 0;
			lookDir.Normalize ();

			lookH = Input.GetAxis ("LookH");
			transform.RotateAround (camTarget.localPosition, Vector3.up, lookH * lookHSpeed * localDeltaTime);

				x += Input.GetAxis("LookH") * xSpeed * 0.02f;
				if (!invertedVerticalAxis){
				y -= Input.GetAxis("LookV") * ySpeed * 0.02f;
				}else{
					y += Input.GetAxis("LookV") * ySpeed * 0.02f;
				}
				
				y = ClampAngle(y, yMinLimit, yMaxLimit);


				Quaternion rotation = Quaternion.Euler(y, x, 0f);


				Vector3 position = rotation * new Vector3(0.0f, 0.0f, -distance) + camTarget.position;
				targetPosition = position;
		}

		//Dans tout les cas, transition fluide entre la position actuelle de la caméra et sa position cible
			CompensateForWalls (characterOffset, ref targetPosition);
			transform.position = Vector3.Lerp (transform.position, targetPosition, localDeltaTime * smooth);
		
		//On s'assure que la caméra regarde toujours là où il faut
		if(camTarget!=null){
		Quaternion rotation = Quaternion.LookRotation(camTarget.position - _myTransform.position);

			if(CurrentCamMode==CamMode.Body)
			rotation *= rotationOffset;
			else if (CurrentCamMode==CamMode.Soul)
			rotation *= soulRotationOffset;

		_myTransform.rotation = Quaternion.Slerp(_myTransform.rotation, rotation, localDeltaTime * damping);
		}
	}

	//Est appelé à chaque fois que Phalene atteint un ghost, incrémente la variable locale qui gère le numéro du ghost actuellement visé.
	void addGhostNumber(){
		currentGhostNumber++;
	}

	static float ClampAngle (float angle,float min,float max) {
		if (angle < -360)
			angle += 360;
		if (angle > 360)
			angle -= 360;
		return Mathf.Clamp (angle, min, max);
	}

	//Dans cette func, un raycast part de l'objet vers là où la caméra doit se positionner. S'il y a un obstacle entre les deux, on décale le point ou elle se place.
	private void CompensateForWalls (Vector3 fromObject, ref Vector3 toTarget)
	{
		RaycastHit wallHit = new RaycastHit ();
		Debug.DrawLine (fromObject, toTarget);
		if (Physics.Linecast(fromObject, toTarget, out wallHit, CompensateLayer))
		{
			Vector3 hitWallNormal = wallHit.normal.normalized;
			toTarget = new Vector3(wallHit.point.x + 1f * hitWallNormal.x, wallHit.point.y + 1f * hitWallNormal.y, wallHit.point.z + 1f * hitWallNormal.z);
		}
	}

	void OnGUI()
	{
		if(aimCamMode == true)
		{
			GUI.DrawTexture(positionch, CrosshairTexture);
		}
	}

}
