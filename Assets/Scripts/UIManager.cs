/*
* Author: Ricardo Franco Mart�n
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
	private static UIManager instance;

	public static UIManager Instance
	{
		get
		{
			return instance;
		}
	}

	void Awake()
	{
		instance = this;
	}


	public Text playerEnergyText;

	public void SetPlayerEnergy(int energy, int maxEnergy)
	{
		playerEnergyText.text = string.Format("Energy: {0} / {1}", energy, maxEnergy);
	}
}
