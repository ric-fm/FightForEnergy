/*
* Author: Ricardo Franco Martín
*/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using System;
using System.Linq;


public class Spawner : MonoBehaviour
{
	public List<SpawnStats> spawnStats;

	public float range;

	public int currentWave;

	public float difficultyMultiplier = 1.0f;

	public float delayMultiplier = 1.0f;

	public float delayBetweenWaves = 5.0f;

	Dictionary<SpawnStats, float> probabilities = new Dictionary<SpawnStats, float>();

	void Start () {
		OrderProbabilities();

		StartCoroutine(SpawnLoop());
	}

	bool waveCompleted = false;

	IEnumerator SpawnLoop()
	{
		while(true)
		{
			float randValue = Random.Range(0.0f, 1.0f);
			float randX = Random.Range(transform.position.x - range, transform.position.x + range);
			float randZ = Random.Range(transform.position.z - range, transform.position.z + range);

			Vector3 randPosition = new Vector3(randX, 0.0f, randZ);

			SpawnStats spawn = GetSpawnStats(randValue);

			if(spawn != null)
			{
				Spawn(spawn, randPosition);
			}

			if(probabilities.Count == 0)
			{
				waveCompleted = true;
			}

			if(waveCompleted)
			{
				waveCompleted = false;
				++currentWave;
				
				yield return new WaitForSeconds(delayBetweenWaves);
			}
			else
			{
				if(spawn != null)
				{
					yield return new WaitForSeconds(spawn.interval);
				}
				else
				{
					waveCompleted = true;
				}
			}
		}
	}

	void OrderProbabilities()
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

	public SpawnStats GetSpawnStats(float randValue)
	{
		SpawnStats spawn = null;
		foreach(SpawnStats key in probabilities.Keys)
		{
			if(randValue <= probabilities[key])
			{
				spawn = key;
				break;
			}
		}
		if(spawn == null && probabilities.Count > 0)
		{
			spawn = probabilities.First().Key;
		}
		return spawn;
	}

	void Spawn(SpawnStats spawnStat, Vector3 position)
	{
		GameObject spawnEntityGO = GameObject.Instantiate(spawnStat.spawnTemplate, position, Quaternion.identity);
		//GameObject spawnEntityGO = new GameObject(spawnStat.spawnTemplate.name);
		//spawnEntityGO.transform.SetParent(transform, true);

		spawnStat.spawnedEntities.Add(spawnEntityGO);

		if(spawnStat.rate == spawnStat.spawnedEntities.Count)
		{
			OrderProbabilities();
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
