/*
* Author: Ricardo Franco Martín
*/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class DeathOnEnergyOut : MonoBehaviour
{
	Energy energy;

	void Start () {
		energy = GetComponent<Energy>();
		energy.OnEnergyChanged += DestroyIfEnergyOut;
	}

	void DestroyIfEnergyOut(Energy energy)
	{
		if(!energy.HasEnergy)
		{
			Destroy(this.gameObject);
		}
	}
	
	void Update () {
		
	}
}
