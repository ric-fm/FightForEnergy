/*
* Author: Ricardo Franco Martín
*/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class BaseMachine : Machine
{
	//public List<GameObject> UIS;
	public GameObject UIS;

	bool playerIn = false;

	bool canShowUpgrades = true;

	

	protected override void Start()
	{
		base.Start();

		canShowUpgrades = energy.Amount >= minimunEnergyAmount;

		HideUI();
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "Player")
		{
			playerIn = true;
			//if(canShowUpgrades)
			//{
			//	ShowUI();
			//}
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.gameObject.tag == "Player")
		{
			playerIn = false;
			//HideUI();
		}
	}

	void ShowUI()
	{
		UIS.SetActive(true);
		//foreach (GameObject ui in UIS)
		//{
		//	ui.SetActive(true);
		//}
	}

	void HideUI()
	{
		UIS.SetActive(false);

		//foreach (GameObject ui in UIS)
		//{
		//	ui.SetActive(false);
		//}
	}

	private void Update()
	{
		if (playerIn)
		{
			if(canShowUpgrades)
			{
				ShowUI();
			}
		}
		else
		{
			HideUI();
		}
	}

	protected override void OnEnergyChanged(Energy energy)
	{
		base.OnEnergyChanged(energy);
		Debug.Log("Machine( " + gameObject.name + ") energy " + energy.Amount);

		canShowUpgrades = energy.Amount >= minimunEnergyAmount;

		//if(!canShowUpgrades)
		//{
		//	HideUI();
		//}
	}
}
