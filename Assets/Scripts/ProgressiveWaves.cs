/*
* Author: Ricardo Franco Martín
*/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[System.Serializable]
public class ProgressiveWaves
{
	public List<SpawnStats> spawnStats;

	public float difficultyMultiplier = 1.0f;

	public float delayMultiplier = 1.0f;

	public float delayBetweenWaves = 5.0f;

	Dictionary<SpawnStats, float> probabilities = new Dictionary<SpawnStats, float>();


	[HideInInspector]
	public float probabilityMultiplier = 1;

	[HideInInspector]
	public float intervalMultiplier = 1;

	[HideInInspector]
	public int rateMultiplier = 1;

	[HideInInspector]
	public float delayBetweenWavesMultiplier = 1;


	public float probabilityIncrement = 0;
	public float maxProbabilityIncrement = 1;

	public float maxIntervalIncrement = 0;
	public float intervalIncrement = 1;

	public int rateIncrement = 0;
	public int maxRateIncrement = 1;

	public float delayBetweenWavesIncrement = 0;
	public float maxDelayBetweenWaves = 1;


	public SpawnStats GetSpawnStats(float randValue)
	{
		SpawnStats spawn = null;
		foreach (SpawnStats key in probabilities.Keys)
		{
			if (randValue <= probabilities[key])
			{
				spawn = key;
				break;
			}
		}
		if (spawn == null && probabilities.Count > 0)
		{
			spawn = probabilities.First().Key;
		}
		return spawn;
	}

	public void OrderProbabilities()
	{
		float probabilitySum = 0;
		probabilities.Clear();

		List<SpawnStats> spawnProbabilities = spawnStats.OrderBy(sp => sp.probability).Reverse().ToList();


		for (int i = 0; i < spawnProbabilities.Count; i++)
		{
			if (spawnProbabilities[i].rate > spawnProbabilities[i].spawnedEntities.Count)
			{
				probabilitySum += spawnStats[i].probability;
			}
		}

		float probSum = 0;
		for (int i = 0; i < spawnProbabilities.Count; i++)
		{
			if (spawnProbabilities[i].rate > spawnProbabilities[i].spawnedEntities.Count)
			{
				float prob = spawnProbabilities[i].probability / probabilitySum + probSum;
				probSum = prob;
				probabilities.Add(spawnProbabilities[i], prob);
			}

		}
	}


	public void UpdateDifficulty()
	{
		if (Mathf.Abs(probabilityMultiplier) < maxProbabilityIncrement)
		{
			probabilityMultiplier += probabilityIncrement;
		}

		if (Mathf.Abs(intervalMultiplier) < maxIntervalIncrement)
		{
			intervalMultiplier += intervalIncrement;
		}

		if (Mathf.Abs(rateMultiplier) < maxRateIncrement)
		{
			rateMultiplier += rateIncrement;
		}

		if (Mathf.Abs(delayBetweenWavesMultiplier) < maxDelayBetweenWaves)
		{
			delayBetweenWavesMultiplier += delayBetweenWavesIncrement;
		}
	}
}


[System.Serializable]
public class SpawnStats
{
	public GameObject spawnTemplate;

	public List<GameObject> spawnedEntities;

	public int rate;

	public float probability;

	public float interval;


}