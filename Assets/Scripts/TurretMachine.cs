/*
* Author: Ricardo Franco Martín
*/


using RAIN.Core;
using RAIN.Entities;
using RAIN.Memory;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class TurretMachine : Machine
{
	public GameObject target;

	public Transform Cannon;
	public Transform shootPoint;

	bool canShoot = true;

	public float shootSpeed;
	public float turnSpeed;
	public float coolDownInterval;

	public GameObject missileTemplate;

	Vector3 shootDirection;
	float shootAngle;

	public AIRig ai;

	bool isRunning;

	Animator anim;

	public AudioClip ShootSound;

	protected override void Awake()
	{
		base.Awake();

		anim = GetComponent<Animator>();
	}

	protected override void Start()
	{
		base.Start();
	}

	void Update()
	{
		if (isRunning)
		{

			Aim();

			target = GetTarget();

			if (canShoot && target != null && CannonReady())
			{
				Shoot();
			}
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
		GameObject missileGO = Instantiate(missileTemplate, shootPoint.position, Quaternion.Euler(0.0f, 0.0f, shootAngle));

		missileGO.transform.forward = -shootDirection;

		Missile missile = missileGO.GetComponent<Missile>();

		missile.Shoot(-shootDirection, shootSpeed);

		SoundManager.Instance.PlaySingleAtLocation(ShootSound, transform.position);

		StartCoroutine(CoolDown());
	}

	IEnumerator CoolDown()
	{
		canShoot = false;

		yield return new WaitForSeconds(coolDownInterval);

		canShoot = true;
	}

	public GameObject GetTarget()
	{
		AI ai2 = ai.AI;

		RAINMemory memory = ai2.WorkingMemory;

		object obj = memory.GetItem("target");

		if (obj != null)
		{
			return (GameObject)obj;
		}

		return null;
	}


	public void On()
	{
		isRunning = true;
		AddEntity();

		anim.SetBool("IsOn", true);

		StartCoroutine(CoolDown());

	}

	public void Off()
	{
		isRunning = false;
		RemoveEntity();

		anim.SetBool("IsOn", true);
		StopAllCoroutines();
	}

	public override void CheckOn()
	{
		if (!energy.HasEnergy && isRunning)
		{
			Off();
		}
		else if (energy.HasEnergy && !isRunning)
		{
			On();
		}
	}

	protected override void OnEnergyChanged(Energy energy)
	{
		base.OnEnergyChanged(energy);

		CheckOn();
	}
}
