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

	protected override void Start()
	{
		base.Start();
	}

	public void Hit()
	{
		GameObject target = GetTarget();

		Debug.Log("Hit on " + target.name);
		StealEnergy(target);
	}

	void StealEnergy(GameObject target)
	{
		Energy energy = target.GetComponent<Energy>();

		energy.Steal(damage, 1);
	}

}
