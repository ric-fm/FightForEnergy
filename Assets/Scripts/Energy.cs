/*
* Author: Ricardo Franco Martín
*/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Energy : MonoBehaviour
{
	public delegate void EnergyEvent(Energy energy);

	[SerializeField]
	int amount;

	public int Amount
	{
		get
		{
			return amount;
		}
		set
		{
			int previousAmount = amount;
			amount = value;

			if (previousAmount != value)
			{
				NotifyEnergyChanged();
			}
		}
	}
	public int MaxAmount;

	public bool CanSteal = true;
	public bool CanReceive = true;

	public float stealCoolDown;
	public float receiveCoolDown;

	public bool HasEnergy
	{
		get
		{
			return Amount > 0;
		}
	}

	public event EnergyEvent OnEnergyChanged;

	public int Steal(int amount)
	{
		int stealedAmount = Amount;
		if (amount <= Amount)
		{
			stealedAmount = amount;
		}
		Amount -= stealedAmount;

		//if (stealedAmount != 0)
		//{
		//	NotifyEnergyChanged();
		//}

		StartCoroutine(StealCoolDown());

		return stealedAmount;
	}

	IEnumerator StealCoolDown()
	{
		CanSteal = false;

		yield return new WaitForSeconds(stealCoolDown);

		CanSteal = true;
	}

	public int Receive(int amount)
	{
		int receivedAmount = amount;

		if (amount <= MaxAmount - Amount)
		{
			receivedAmount = amount;
			Amount += receivedAmount;
		}

		//if (receivedAmount != 0)
		//{
		//	NotifyEnergyChanged();
		//}

		StartCoroutine(ReceiveCoolDown());

		return receivedAmount;
	}

	IEnumerator ReceiveCoolDown()
	{
		CanReceive = false;

		yield return new WaitForSeconds(stealCoolDown);

		CanReceive = true;
	}

	void NotifyEnergyChanged()
	{
		if (OnEnergyChanged != null)
		{
			OnEnergyChanged(this);
		}
	}
}
