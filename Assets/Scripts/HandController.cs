/*
* Author: Ricardo Franco Martín
*/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class HandController : MonoBehaviour
{
	public delegate void HandEvent(GameObject target);

	public event HandEvent OnHitTarget;

	private void OnTriggerEnter(Collider other)
	{
		NotifyHitTarget(other.gameObject);
	}

	void NotifyHitTarget(GameObject target)
	{
		if(OnHitTarget != null)
		{
			OnHitTarget(target);
		}
	}
}
