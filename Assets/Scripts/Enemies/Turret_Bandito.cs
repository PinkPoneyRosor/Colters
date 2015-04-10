using UnityEngine;
using System.Collections;

public class Turret_Bandito : Turret_GlobalBehaviour {


	public override void FireProjectile()
	{
		nextFireTime = Time.time+reloadTime;
		nextMoveTime = Time.time+firePauseTime;
		CalculateAimError();
		
		GameObject spawnedArrow;
		
		spawnedArrow = Instantiate (myProjectile, muzzle.position, transform.rotation) as GameObject;
		
		Projectile_Bandit projectileScript = spawnedArrow.GetComponent <Projectile_Bandit>();
		projectileScript.masterTurret = this.gameObject;
	}
}
