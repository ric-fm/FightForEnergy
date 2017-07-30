/*
* Author: Ricardo Franco Martín
*/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SpawnMachine : MonoBehaviour
{
	public delegate void SpawnMachineEvent(Machine machine);

	public GameObject machineTemplate;

	public GameObject machineGhost;
	Collider machineGhostCollider;

	Vector3 spawnPosition;

	public event SpawnMachineEvent OnMachineSpawned;

	void Start()
	{

	}

	void Update()
	{
		spawnPosition = GameManager.Instance.CursorPosition;
		spawnPosition = GameManager.Instance.GridedCursorPosition;

		if (machineTemplate != null)
		{
			if (machineGhost == null)
			{
				machineGhost = GameObject.Instantiate(machineTemplate, spawnPosition, Quaternion.identity);
			}

			machineGhost.transform.position = spawnPosition;

			if (Input.GetButtonDown("Fire1"))
			{
				if (CanSpawnMachineOnPosition())
				{
					GameObject newMachine = GameObject.Instantiate(machineTemplate, spawnPosition, Quaternion.identity);
					machineGhost = null;

					NotifySpawn(newMachine);
				}
			}
		}
	}

	bool CanSpawnMachineOnPosition()
	{
		return true;
	}

	void NotifySpawn(GameObject machineGO)
	{
		if (OnMachineSpawned != null)
		{
			OnMachineSpawned(machineGO.GetComponent<Machine>());
		}
	}
}
