/*
* Author: Ricardo Franco Martín
*/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class BaseMachine : Machine
{
	public List<BaseMachineUI> UIS;
	//public GameObject UIS;

	bool playerIn = false;

	bool canShowUpgrades = true;

	MeshRenderer meshRenderer;

	protected override void Start()
	{
		base.Start();

		meshRenderer = GetComponent<MeshRenderer>();

		canShowUpgrades = energy.Amount >= minimunEnergyAmount;

		HideUI();
	}

	void SetAlpha(float value)
	{
		//float newAlpha = Mathf.Lerp(renderer.material.color.a, 0F, Time.deltaTime * fadeLerpConstant);
		meshRenderer.material.color = new Color(meshRenderer.material.color.r, meshRenderer.material.color.g, meshRenderer.material.color.b, value);
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

			SetAlpha(0.5f);
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.gameObject.tag == "Player")
		{
			playerIn = false;
			//HideUI();

			//SetAlpha(1.0f);

		}
	}

	void ShowUI()
	{
		//UIS.SetActive(true);
		foreach (BaseMachineUI ui in UIS)
		{
			ui.Show();
		}
	}

	void HideUI()
	{
		//UIS.SetActive(false);

		foreach (BaseMachineUI ui in UIS)
		{
			ui.Hide();
		}
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

		if(energy.Amount == 0)
		{
			GameManager.Instance.GameOver();
		}

		//if(!canShowUpgrades)
		//{
		//	HideUI();
		//}
	}
}
