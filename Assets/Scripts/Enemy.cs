/*
* Author: Ricardo Franco Martín
*/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Enemy : MonoBehaviour
{
	public delegate void EnemyEvent(Enemy enemy);

	Energy energy;

	public event EnemyEvent OnEnemyDestroyed;

	void Start () {
		energy = GetComponent<Energy>();
		energy.OnEnergyChanged += OnEnergyChanged;
	}

	void OnEnergyChanged(Energy energy)
	{
		if(!energy.HasEnergy)
		{
			NotifyDestroyed();
			Destroy(gameObject);
		}
	}
	
	void NotifyDestroyed()
	{
		if(OnEnemyDestroyed != null)
		{
			OnEnemyDestroyed(this);
		}
	}
}
