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
	public List<CustomWave> customWaves;

	public ProgressiveWaves progressiveWave;


	public int currentWave;
	public float range;

	void Start()
	{
		progressiveWave.OrderProbabilities();

		StartCoroutine(SpawnLoop());
	}

	bool waveCompleted = false;

	void CustomSpawnWave(CustomWave wave)
	{

	}

	IEnumerator SpawnLoop()
	{
		while (true)
		{
			if (currentWave < customWaves.Count)
			{
				Debug.Log("Custom spawn");
				CustomWave customWave = customWaves[currentWave];

				foreach(CustomSpawn customSpawn in customWave.spaws)
				{
					yield return new WaitForSeconds(customWave.delay);

					for(int i = 0; i < customSpawn.rate * progressiveWave.rateMultiplier; i++)
					{
						float randX = Random.Range(transform.position.x - range, transform.position.x + range);
						float randZ = Random.Range(transform.position.z - range, transform.position.z + range);

						Vector3 randPosition = new Vector3(randX, 0.0f, randZ);
						GameObject spawnEntityGO = GameObject.Instantiate(customSpawn.spawnTemplate, randPosition, Quaternion.identity);

						yield return new WaitForSeconds(customSpawn.delay);
					}

				}

				++currentWave;
			}
			else
			{

				Debug.Log("Progresive spawn");

				float randValue = Random.Range(0.0f, 1.0f);
				float randX = Random.Range(transform.position.x - range, transform.position.x + range);
				float randZ = Random.Range(transform.position.z - range, transform.position.z + range);

				Vector3 randPosition = new Vector3(randX, 0.0f, randZ);

				SpawnStats spawn = progressiveWave.GetSpawnStats(randValue);

				if (spawn != null)
				{
					Spawn(spawn, randPosition);
				}

				//if (probabilities.Count == 0)
				//{
				//	waveCompleted = true;
				//}

				if (waveCompleted)
				{
					waveCompleted = false;
					NextProggresiveWave();

					yield return new WaitForSeconds(progressiveWave.delayBetweenWaves * progressiveWave.delayBetweenWavesMultiplier);
				}
				else
				{
					if (spawn != null)
					{
						yield return new WaitForSeconds(spawn.interval * progressiveWave.intervalMultiplier);
					}
					else
					{
						waveCompleted = true;
					}
				}
			}

		}
	}

	void NextProggresiveWave()
	{
		++currentWave;
		progressiveWave.UpdateDifficulty();
	}


	

	void Spawn(SpawnStats spawnStat, Vector3 position)
	{
		GameObject spawnEntityGO = GameObject.Instantiate(spawnStat.spawnTemplate, position, Quaternion.identity);
		//GameObject spawnEntityGO = new GameObject(spawnStat.spawnTemplate.name);
		//spawnEntityGO.transform.SetParent(transform, true);

		spawnStat.spawnedEntities.Add(spawnEntityGO);

		if (spawnStat.rate == spawnStat.spawnedEntities.Count)
		{
			progressiveWave.OrderProbabilities();
		}
	}
}


