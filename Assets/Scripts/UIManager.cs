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

	public GameObject UpgradePanel;
	public Text UpgradeText;
	public Text UpgradeValueText;
	public Text UpgradeCostText;

	public GameObject MachinePanel;
	public Text MachineEnergyText;
	public GameObject machineOnText;
	public GameObject machineOffText;


	public GameObject GameOverPanel;

	public Text GameOverTimerText;

	public Text TimerValueText;
	public Text EnemyDestroyedText;

	public void SetPlayerEnergy(int energy, int maxEnergy)
	{
		float energyPercent = (float)energy / (float)maxEnergy;
		energyBar.size = energyPercent;
	}

	public void ShowUpgrade(string text, bool showValue, float value, float cost)
	{
		UpgradeText.text = text;
		UpgradePanel.SetActive(true);

		if(showValue)
		{
			UpgradeValueText.text = "Value: " + value;
		}
		else
		{
			UpgradeValueText.text = "";
		}

		UpgradeCostText.text = "Energy Cost: " + cost;
	}

	public void HideUpgrade()
	{
		UpgradePanel.SetActive(false);
		UpgradeText.text = "";
		UpgradeValueText.text = "";
		UpgradeCostText.text = "";

	}

	public void ShowMachineEnergyInfo(Machine machine, Energy energy)
	{
		MachineEnergyText.text = string.Format("{0} / {1}", energy.Amount, energy.MaxAmount);
		MachinePanel.SetActive(true);

		if(energy.Amount < machine.minimunEnergyAmount)
		{
			machineOffText.SetActive(true);
			machineOnText.SetActive(false);
		}
		else
		{
			machineOnText.SetActive(true);
			machineOffText.SetActive(false);
		}
	}

	public void HideMachineEnergyInfo()
	{
		MachinePanel.SetActive(false);
		MachineEnergyText.text = "";
	}

	public void ShowGameOver()
	{
		
		EnemyDestroyedText.text = GameManager.Instance.EnemiesDestroyedCount.ToString();

		float timer = GameManager.Instance.timer;
		GameOverTimerText.text = string.Format("{0}:{1}", Mathf.Floor(timer / 60).ToString("00"), (timer % 60).ToString("00"));

		GameOverPanel.SetActive(true);
	}

	private void Update()
	{
		float timer = GameManager.Instance.timer;
		TimerValueText.text = string.Format("{0}:{1}", Mathf.Floor(timer / 60).ToString("00"), (timer % 60).ToString("00"));
	}

}
