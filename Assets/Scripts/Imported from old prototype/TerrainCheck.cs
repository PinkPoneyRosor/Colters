using UnityEngine;
using System.Collections;

/*Ce script permet de faire toutes les vérifications de textures du terrain.
 * Sous certaines conditions, il peut également placer des rochers de plate-forme
 */

public class TerrainCheck : MonoBehaviour {

	public int surfaceIndex = 0;
	public int rockSpawnableSurface = 0;
	public GameObject spawnableRock;
	public int maxSpawnableRocks = 3;
	GameObject spawnedRock;

	Camera mainCamera;
	
	public Vector3 spawnOffset = new Vector3(0,.5f,0);

	Terrain terrain;
	TerrainData terrainData;
	Vector3 terrainPos;

	int rockNumber = 0;
	int rockNumberToDestroy = 0;
	int spawnedRockAmount = 0;


	GameObject player;
	CharacterController playerScript;
	bool coolingDown = false;
	
	void Start()
	{

		terrain = Terrain.activeTerrain;
		terrainData = terrain.terrainData;
		terrainPos = terrain.transform.position;

		mainCamera = Camera.main;

		//A REMPLACER PAR PHALENE DANS LA VERSION FINALE
		player = GameObject.FindWithTag ("Player");
		playerScript = player.GetComponent<CharacterController>();
	}
	
	
	void Update()
	{
		if(coolingDown){
		Vector3 p = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width/2, Screen.height/2, mainCamera.nearClipPlane));

		if(Input.GetButtonDown ("RockSpawn")){
				StartCoroutine(coolDown());
			RaycastHit hit = new RaycastHit();
			Ray ray = new Ray(p, mainCamera.transform.forward);

			if (terrain.collider.Raycast(ray, out hit, Mathf.Infinity)) {
				surfaceIndex = GetMainTexture( hit.point );
			}

			if(surfaceIndex == rockSpawnableSurface){
				float terrainHeight = Terrain.activeTerrain.SampleHeight(transform.position);
				Vector3 rockSpawnPoint = new Vector3(transform.position.x, terrainHeight,transform.position.z);
				

				Vector3 slope = Vector3.zero;
				if (terrain.collider.Raycast(ray, out hit, Mathf.Infinity)) 
					{
					slope = hit.normal;
					Debug.Log ("Slope1="+slope);
					rockSpawnPoint = hit.point;
					rockSpawnPoint -=spawnOffset;
					spawnedRock = Instantiate(spawnableRock, rockSpawnPoint , Quaternion.identity) as GameObject;
					Debug.Log ("Slope2="+slope);
					spawnedRock.transform.rotation = Quaternion.FromToRotation(spawnedRock.transform.up, slope) * spawnedRock.transform.rotation;
					spawnedRock.gameObject.name = "spawnedRock_"+rockNumber;
					spawnedRockAmount++;

						if(spawnedRockAmount>maxSpawnableRocks){
							GameObject firstSpawnedRock = GameObject.Find ("spawnedRock_"+rockNumberToDestroy);
							Destroy (firstSpawnedRock.gameObject);
							rockNumberToDestroy++;
							spawnedRockAmount--;
						}
						rockNumber++;
				}
}}}}
	
	
	// - just for GUI demonstration -

	
	// ----
	
	
	float[] GetTextureMix( Vector3 worldPos )
	{
		// retourne un array qui contiendra les données du mix de textures
		// du terrain principal sur la coordonnées du monde donnée.
		
		// Le nombre de valeurs retournées dans l'array
		// sera équivalent au nombre de textures rattachés au terrain.
		
		// On calcule la case du terrain correspondant au coordonnées du monde données
		float mapXf = ((worldPos.x - terrainPos.x) / terrainData.size.x) * terrainData.alphamapWidth ;
		float mapZf = ((worldPos.z - terrainPos.z) / terrainData.size.z) * terrainData.alphamapHeight ;
		int mapX = (int)mapXf;
		int mapZ = (int)mapZf;
		
		//on obtient les données splat/alpha de cette case
		float[,,] splatmapData = terrainData.GetAlphamaps( mapX, mapZ, 1, 1 );
		
		//et on reconvertit ça dans un array 1D
		float[] cellMix = new float[ splatmapData.GetUpperBound(2) + 1 ];
		
		for ( int n = 0; n < cellMix.Length; n ++ )
		{
			cellMix[n] = splatmapData[ 0, 0, n ];
		}
		
		return cellMix;
	}
	
	
	int GetMainTexture( Vector3 worldPos )
	{
		// on donne l'index de la texture detectée
		// sur le terrain principal, et sur la coordonnée donnée
		float[] mix = GetTextureMix( worldPos );
		
		float maxMix = 0;
		int maxIndex = 0;
		
		// on loope chacune des textures jusqu'à trouver la dominante.
		for ( int n = 0; n < mix.Length; n ++ )
		{
			if ( mix[n] > maxMix )
			{
				maxIndex = n;
				maxMix = mix[n];
			}
		}
		
		return maxIndex;
	}

	IEnumerator coolDown(){
		coolingDown = true;
		Debug.Log ("Coolin' down, man");
		yield return new WaitForSeconds(.5f);
		coolingDown = false;
	}
	

}// END OF SCRIPT
