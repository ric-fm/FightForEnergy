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

				machineGhost.GetComponent<Collider>().enabled = false;
			}

			machineGhost.transform.position = spawnPosition;

			if (Input.GetButtonDown("Fire1"))
			{
				if (CanSpawnMachineOnPosition())
				{
					Machine machine = machineGhost.GetComponent<Machine>();
					machine.CheckOn();
					machine.GetComponent<Collider>().enabled = true;

					NotifySpawn(machineGhost);
					machineGhost = null;
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
