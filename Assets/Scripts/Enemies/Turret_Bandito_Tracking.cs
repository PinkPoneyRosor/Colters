using UnityEngine;
using System.Collections;

public class Turret_Bandito_Tracking : Turret_GlobalBehaviour {	

	public override void FireProjectile()
	{
		nextFireTime = Time.time+reloadTime;
		nextMoveTime = Time.time+firePauseTime;
		CalculateAimError();
		
		GameObject spawnedArrow;
		
		spawnedArrow = Instantiate (myProjectile, muzzle.position, transform.rotation) as GameObject;
		
		Projectile_Tracking projectileScript = spawnedArrow.GetComponent <Projectile_Tracking>();
		projectileScript.masterTurret = this.gameObject;
	}

}
