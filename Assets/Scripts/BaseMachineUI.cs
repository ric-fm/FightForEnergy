/*
* Author: Ricardo Franco Martín
*/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class BaseMachineUI : MonoBehaviour
{
	public enum UpgradeType
	{
		CAPACITY,
		MULTIPLIER,
		SPEED,
		RANGE,
		TURRET,
		BAIT
	}

	public UpgradeType type;
	public float value;
	public int energyCost = 1;

	public float coolDownInterval = 1.0f;
	bool canUpgrade = true;

	PlayerController playerController;

	private void Start()
	{
		playerController = FindObjectOfType<PlayerController>();
	}

	private void OnMouseDown()
	{
		if (canUpgrade && playerController.CanSpendEnergy(energyCost))
		{
			Debug.Log("Upgrade " + type.ToString());
			playerController.UpgradeStat(type, value, energyCost);
		}
		else
		{
			Debug.Log("Cant Upgrade " + type.ToString());
		}
	}

	IEnumerator CoolDown()
	{
		canUpgrade = false;
		yield return new WaitForSeconds(coolDownInterval);
		canUpgrade = true;
	}
}
