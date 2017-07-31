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
	public AIRig ai;

	protected override void Start()
	{
		base.Start();
	}

	public void Hit()
	{
		GameObject target = GetTarget();

		Debug.Log("Hit on " + target.name);
	}

	public GameObject GetTarget()
	{
		AI ai2 = ai.AI;

		RAINMemory memory = ai2.WorkingMemory;

		object obj = memory.GetItem("attacktarget");

		if (obj != null)
		{
			return (GameObject)obj;
		}

		return null;
	}
}
