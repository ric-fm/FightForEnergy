/*
* Author: Ricardo Franco Martín
*/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Machine : MonoBehaviour
{
	Energy energy;

	public delegate void MachineEvent(Machine machine);

	public event MachineEvent OnMachineEnergyFilled;

	protected virtual void Start () {
		energy = GetComponent<Energy>();
		energy.OnEnergyChanged += OnEnergyChanged;
	}

	void OnEnergyChanged(Energy energy)
	{
		Debug.Log("Machine( " + gameObject.name + ") energy " + energy.Amount);

		if(energy.Amount == energy.MaxAmount)
		{
			Debug.Log("Machine(" + gameObject.name + ") filled");
			NotifyEnergyFilled();
		}
	}

	void NotifyEnergyFilled()
	{
		if(OnMachineEnergyFilled != null)
		{
			OnMachineEnergyFilled(this);
		}
	}
}
