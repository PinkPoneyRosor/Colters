using UnityEngine;
using System.Collections;

/*This script can check what composes the point the player is looking at on a terrain
 * Under certain conditions, it can spawn a rock
 */

public class TerrainCheck : MonoBehaviour {

	//Set up & inspector vars
	public int surfaceIndex = 0;
	public int rockSpawnableSurface = 0;
	public int maxSpawnableRocks = 3;
	public Vector3 spawnOffset = new Vector3(0,.5f,0);

	//External scripts and objects
	GameObject spawnedRock;
	Camera mainCamera;
	GameObject player;
	CharacterController playerScript;
	public GameObject spawnableRock;

	//Terrain vars
	Terrain terrain;
	TerrainData terrainData;
	Vector3 terrainPos;

	//Misc. Vars
	int rockNumber = 0;
	int rockNumberToDestroy = 0;
	int spawnedRockAmount = 0;
	bool coolingDown = false;
	
void Start()
{
	terrain = Terrain.activeTerrain;
	terrainData = terrain.terrainData;
	terrainPos = terrain.transform.position;

	mainCamera = Camera.main;
	
	player = GameObject.FindWithTag ("Player");
	playerScript = player.GetComponent<CharacterController>();
}
	
	
void Update()
{
	if(!coolingDown)
	{
		Vector3 p = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width/2, Screen.height/2, mainCamera.nearClipPlane));

		if(Input.GetButtonDown ("RockSpawn"))
		{
			StartCoroutine(coolDown());
			RaycastHit hit = new RaycastHit();
			Ray ray = new Ray(p, mainCamera.transform.forward);

			if (terrain.collider.Raycast(ray, out hit, Mathf.Infinity)) 
				surfaceIndex = GetMainTexture( hit.point );

			if(surfaceIndex == rockSpawnableSurface)
			{
				float terrainHeight = Terrain.activeTerrain.SampleHeight(transform.position);
				Vector3 rockSpawnPoint = new Vector3(transform.position.x, terrainHeight,transform.position.z);
				

				Vector3 slope = Vector3.zero;
				if (terrain.collider.Raycast(ray, out hit, Mathf.Infinity)) 
				{
					slope = hit.normal;
					rockSpawnPoint = hit.point;
					rockSpawnPoint -=spawnOffset;
					spawnedRock = Instantiate(spawnableRock, rockSpawnPoint , Quaternion.identity) as GameObject;
					spawnedRock.transform.rotation = Quaternion.FromToRotation(spawnedRock.transform.up, slope) * spawnedRock.transform.rotation;
					spawnedRock.gameObject.name = "spawnedRock_"+rockNumber;
					spawnedRockAmount++;

					if(spawnedRockAmount>maxSpawnableRocks)
					{
						GameObject firstSpawnedRock = GameObject.Find ("spawnedRock_"+rockNumberToDestroy);
						Destroy (firstSpawnedRock.gameObject);
						rockNumberToDestroy++;
						spawnedRockAmount--;
					}
						rockNumber++;
				}
			}
		}
	}
}
	
float[] GetTextureMix( Vector3 worldPos )
{
	// Return an array which will contain datas from the textures mix
	// of the main terrain, based on world coordinates.
	
	// The amount of returned values will be
	// equal to the number of textures attached to the terrain.

	//Let's calculate which point of the terrain corresponds to the given world coordinates
	float mapXf = ((worldPos.x - terrainPos.x) / terrainData.size.x) * terrainData.alphamapWidth ;
	float mapZf = ((worldPos.z - terrainPos.z) / terrainData.size.z) * terrainData.alphamapHeight ;
	int mapX = (int)mapXf;
	int mapZ = (int)mapZf;

	//Then we get the splat/alpha of this point
	float[,,] splatmapData = terrainData.GetAlphamaps( mapX, mapZ, 1, 1 );
	
	//Let's put it in a unidimensional array.
	float[] cellMix = new float[ splatmapData.GetUpperBound(2) + 1 ];
	
	for ( int n = 0; n < cellMix.Length; n ++ )
	{
		cellMix[n] = splatmapData[ 0, 0, n ];
	}
	
	return cellMix;
}
	
	
int GetMainTexture( Vector3 worldPos )
{
	// Let's get the index of the detected texture
	float[] mix = GetTextureMix( worldPos );
	
	float maxMix = 0;
	int maxIndex = 0;
	
	//Let's look at each texture and find the dominant.
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
	yield return new WaitForSeconds(.5f);
	coolingDown = false;
}
	

}
// END OF SCRIPT
