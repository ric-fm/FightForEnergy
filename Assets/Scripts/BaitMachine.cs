/*
* Author: Ricardo Franco Martín
*/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RAIN.Entities;
using RAIN.Entities.Aspects;
public class BaitMachine : Machine
{
	public GameObject particles;

	bool isRunning = false;

	

	public void On()
	{
		isRunning = true;
		particles.SetActive(true);
		//entity.enabled = true;

		//entityGO.AddComponent<EntityRig>();
		AddEntity();
		

	}

	public void Off()
	{
		isRunning = false;
		particles.SetActive(false);
		//entity.enabled = false;

		//Destroy(entityGO.GetComponent<EntityRig>());
		RemoveEntity();
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

	protected override void OnEnergyChanged(Energy energy)
	{
		base.OnEnergyChanged(energy);

		CheckOn();
	}
}
