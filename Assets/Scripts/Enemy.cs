/*
* Author: Ricardo Franco Martín
*/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Enemy : MonoBehaviour
{
	public delegate void EnemyEvent(Enemy enemy);

	protected Energy energy;

	public event EnemyEvent OnEnemyDestroyed;

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
}
