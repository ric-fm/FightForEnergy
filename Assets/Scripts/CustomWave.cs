/*
* Author: Ricardo Franco Martín
*/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CustomWave
{
	public List<CustomSpawn> spaws;

	List<GameObject> spawnedEntities = new List<GameObject>();

	public float delay;

	public int GetAlliveEnemiesSpawned()
	{
		return spawnedEntities.Count;
	}

	public void EnemyDestroyed(Enemy enemy)
	{
		spawnedEntities.Remove(enemy.gameObject);
	}

	public void SpawnedEntity(GameObject enemyGO)
	{
		spawnedEntities.Add(enemyGO);
	}
}

[System.Serializable]
public class CustomSpawn
{
	public GameObject spawnTemplate;
	public int rate;
	public float delay;
}
