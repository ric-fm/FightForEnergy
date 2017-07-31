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

	public Scrollbar energyBar;

	public void SetPlayerEnergy(int energy, int maxEnergy)
	{
		float energyPercent = (float)energy / (float)maxEnergy;
		energyBar.size = energyPercent;
	}
}
