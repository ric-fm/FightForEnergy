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

	public bool IsUnique = false;
	public UpgradeStats UniqueStat;

	public List<UpgradeStats> Stats;
	UpgradeStats CurrentStats;
	public int currentStatsIndex = 0;
	bool canUpgrade = true;

	PlayerController playerController;

	private void Start()
	{
		playerController = FindObjectOfType<PlayerController>();

		if(IsUnique)
		{
			CurrentStats = UniqueStat;
		}
		else
		{
			CurrentStats = Stats[0];
		}
	}

	private void OnMouseDown()
	{
		if (canUpgrade && playerController.CanSpendEnergy(CurrentStats.Cost))
		{
			Debug.Log("Upgrade " + type.ToString());
			playerController.UpgradeStat(type, CurrentStats.Value, CurrentStats.Cost);
			float delay = CurrentStats.CoolDownInterval;
			if(!IsUnique)
			{
				CheckValue();
			}
				
			StartCoroutine(CoolDown(delay));
		}
		else
		{
			Debug.Log("Cant Upgrade " + type.ToString());
		}
	}

	void CheckValue()
	{
		++currentStatsIndex;
		if(currentStatsIndex >= Stats.Count)
		{
			Destroy(gameObject);
		}
		else
		{
			CurrentStats = Stats[currentStatsIndex];
		}
	}

	IEnumerator CoolDown(float interval)
	{
		canUpgrade = false;
		yield return new WaitForSeconds(interval);
		canUpgrade = true;
	}
}

[System.Serializable]
public class UpgradeStats
{
	public float Value;

	public int Cost;

	public float CoolDownInterval = 1.0f;
}