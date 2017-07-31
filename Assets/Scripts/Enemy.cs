/*
* Author: Ricardo Franco Martín
*/


using RAIN.Core;
using RAIN.Memory;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Enemy : MonoBehaviour
{
	public delegate void EnemyEvent(Enemy enemy);

	protected Energy energy;

	public event EnemyEvent OnEnemyDestroyed;

	public AIRig ai;

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

	protected virtual void Start () {
		energy = GetComponent<Energy>();
		energy.OnEnergyChanged += OnEnergyChanged;
	}

	protected virtual void OnEnergyChanged(Energy energy)
	{
		if(!energy.HasEnergy)
		{
			NotifyDestroyed();
			Destroy(gameObject);
		}
	}
	
	protected void NotifyDestroyed()
	{
		if(OnEnemyDestroyed != null)
		{
			OnEnemyDestroyed(this);
		}
	}

	private void OnDestroy()
	{
		GameManager.Instance.NotifyEnemyDestroyed(this);
	}
}
