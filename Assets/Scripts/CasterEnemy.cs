/*
* Author: Ricardo Franco Martín
*/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CasterEnemy : Enemy
{

	public Transform shootPoint;

	protected override void Start()
	{
		base.Start();
	}

	public void Shoot()
	{
		GameObject target = GetTarget();

		Debug.Log("shoot to " + target.name);
	}
}
