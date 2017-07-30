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

	protected override void Start()
	{
		base.Start();

		HideUI();
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "Player")
		{
			playerIn = true;
			ShowUI();
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.gameObject.tag == "Player")
		{
			playerIn = false;
			HideUI();
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
}
