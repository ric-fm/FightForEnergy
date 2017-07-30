/*
* Author: Ricardo Franco Martín
*/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class TurretMachine : Machine
{
	public Enemy target;

	public Transform Cannon;
	public Transform shootPoint;

	bool canShoot = true;

	public float shootSpeed;
	public float turnSpeed;
	public float coolDownInterval;

	public GameObject missileTemplate;

	Vector3 shootDirection;
	float shootAngle;

	protected override void Start()
	{
		base.Start();

		StartCoroutine(CoolDown());
	}

	void Update()
	{
		Aim();

		if (canShoot && target != null && CannonReady())
		{
			Shoot();
		}
	}

	void Aim()
	{
		if (target != null)
		{
			shootDirection = (transform.position - target.transform.position).normalized;

			shootAngle = -Mathf.Atan2(shootDirection.x, -shootDirection.z) * Mathf.Rad2Deg;

			Cannon.transform.localRotation = Quaternion.Lerp(Cannon.transform.localRotation, Quaternion.Euler(-90.0f, 0.0f, shootAngle), turnSpeed * Time.deltaTime);
		}

	}

	bool CannonReady()
	{
		return true;
	}

	void Shoot()
	{
		GameObject missileGO = Instantiate(missileTemplate, shootPoint.position, Quaternion.Euler(0.0f, shootAngle, 0.0f));

		Missile missile = missileGO.GetComponent<Missile>();

		missile.Shoot(shootDirection, shootSpeed);
		StartCoroutine(CoolDown());
	}

	IEnumerator CoolDown()
	{
		canShoot = false;

		yield return new WaitForSeconds(coolDownInterval);

		canShoot = true;
	}
}
