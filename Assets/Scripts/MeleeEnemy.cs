/*
* Author: Ricardo Franco Martín
*/


using RAIN.Core;
using RAIN.Memory;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MeleeEnemy : Enemy
{
	public int damage = 1;

	GameObject currentTarget;

	public AudioClip PlayerHitSound;
	public AudioClip MachineHitSound;

	protected override void Start()
	{
		base.Start();
	}

	public void Hit()
	{
		//GameObject target = GetTarget();

		Debug.Log("Hit on " + currentTarget.name);

		if (currentTarget != null)
		{
			
			StealEnergy(currentTarget);
		}
	}

	void StealEnergy(GameObject target)
	{
		Energy energy = target.GetComponent<Energy>();

		if(energy.CanSteal)
		{
			if (target.tag == "Player")
			{
				SoundManager.Instance.PlaySingleAtLocation(PlayerHitSound, currentTarget.transform.position);
			}
			else
			{
				SoundManager.Instance.PlaySingleAtLocation(MachineHitSound, currentTarget.transform.position);
			}

			energy.Steal(damage, 1);
		}
	}

	private void LateUpdate()
	{
		currentTarget = GetTarget();
		if (currentTarget != null)
		{
			transform.LookAt(currentTarget.transform);
		}
	}

}
