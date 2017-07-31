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

	GameObject currentTarget;

	protected override void Start()
	{
		base.Start();
	}

	public void Shoot()
	{
		//GameObject target = GetTarget();

		if (currentTarget != null)
		{
			Debug.Log("shoot to " + currentTarget.name);

			Vector3 shootDirection = (shootPoint.position - currentTarget.transform.position).normalized;

			shootDirection = (transform.position - currentTarget.transform.position).normalized;

			float shootAngle = -Mathf.Atan2(shootDirection.x, -shootDirection.z) * Mathf.Rad2Deg;

			GameObject bulletGO = GameObject.Instantiate(bulletTemplate, shootPoint.position, Quaternion.Euler(0.0f, shootAngle, 0.0f));

			EnemyBullet bullet = bulletGO.GetComponent<EnemyBullet>();

			bullet.Shoot(shootDirection, shootSpeed);
		}
	}

	private void LateUpdate()
	{
		currentTarget = GetTarget();
		if (currentTarget != null)
		{
			transform.LookAt(currentTarget.transform);
		}
		else
		{
			Debug.Log("no target");
		}
	}
}
