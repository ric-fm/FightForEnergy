/*
* Author: Ricardo Franco Martín
*/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Machine : MonoBehaviour
{
	protected Energy energy;

	public delegate void MachineEvent(Machine machine);

	public event MachineEvent OnMachineEnergyFilled;


	protected virtual void Awake()
	{
		energy = GetComponent<Energy>();
		
	}
	protected virtual void Start () {
		energy.OnEnergyChanged += OnEnergyChanged;
	}

	protected virtual void OnEnergyChanged(Energy energy)
	{
		Debug.Log("Machine( " + gameObject.name + ") energy " + energy.Amount);

		if(energy.Amount == energy.MaxAmount)
		{
			Debug.Log("Machine(" + gameObject.name + ") filled");
			NotifyEnergyFilled();
		}
	}

	public virtual void CheckOn()
	{

	}

	void NotifyEnergyFilled()
	{
		if(OnMachineEnergyFilled != null)
		{
			OnMachineEnergyFilled(this);
		}
	}
}
