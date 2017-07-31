/*
* Author: Ricardo Franco Martín
*/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CasterEnemy : Enemy
{

	public Transform shootPoint;

	public GameObject bulletTemplate;

	public float shootSpeed;

	protected override void Start()
	{
		base.Start();
	}

	public void Shoot()
	{
		GameObject target = GetTarget();

		if (target != null)
		{
			Debug.Log("shoot to " + target.name);

			Vector3 shootDirection = (shootPoint.position - target.transform.position).normalized;

			shootDirection = (transform.position - target.transform.position).normalized;

			float shootAngle = -Mathf.Atan2(shootDirection.x, -shootDirection.z) * Mathf.Rad2Deg;

			GameObject bulletGO = GameObject.Instantiate(bulletTemplate, shootPoint.position, Quaternion.Euler(0.0f, shootAngle, 0.0f));

			EnemyBullet bullet = bulletGO.GetComponent<EnemyBullet>();

			bullet.Shoot(shootDirection, shootSpeed);
		}
	}
}
