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
	protected override void Start()
	{
		base.Start();
	}

	public void Hit()
	{
		GameObject target = GetTarget();

		Debug.Log("Hit on " + target.name);
	}

}
