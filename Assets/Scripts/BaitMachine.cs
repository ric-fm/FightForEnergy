/*
* Author: Ricardo Franco Martín
*/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RAIN.Entities;
public class BaitMachine : Machine
{
	public GameObject particles;

	bool isRunning = false;

	public EntityRig entity;

	public void On()
	{
		isRunning = true;
		particles.SetActive(true);
		entity.enabled = true;
	}

	public void Off()
	{
		isRunning = false;
		particles.SetActive(false);
		entity.enabled = false;
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

	protected override void Start()
	{
		base.Start();
	}

	protected virtual void OnEnergyChanged(Energy energy)
	{
		base.OnEnergyChanged(energy);

		CheckOn();
	}
}
