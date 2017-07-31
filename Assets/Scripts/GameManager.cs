/*
* Author: Ricardo Franco Martín
*/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class GameManager : MonoBehaviour
{
	static GameManager instance;
	public static GameManager Instance
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

	PlayerController playerController;

	private void Start()
	{
		playerController = FindObjectOfType<PlayerController>();
	}

	Vector3 cursorPosition;
	public Vector3 CursorPosition
	{
		get
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			Vector3 point = Vector3.zero;

			RaycastHit hit;

			if (Physics.Raycast(ray, out hit, 10000, 1 << LayerMask.NameToLayer("Ground")))
			{
				cursorPosition = hit.point;

				cursorPosition.y = playerController.transform.position.y;
			}

			return cursorPosition;
		}
	}

	public int GridTileSize = 1;
	Vector3 gridedCursorPosition;
	public Vector3 GridedCursorPosition
	{
		get
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			Vector3 point = Vector3.zero;

			RaycastHit hit;

			if (Physics.Raycast(ray, out hit, 10000, 1 << LayerMask.NameToLayer("Ground")))
			{
				gridedCursorPosition = hit.point;

				gridedCursorPosition.y = playerController.transform.position.y;
			}

			//gridedCursorPosition.x = gridedCursorPosition.x - GridTileSize;
			gridedCursorPosition.x = Mathf.Ceil(gridedCursorPosition.x);
			//gridedCursorPosition.x += GridTileSize / 2;

			//gridedCursorPosition.z = gridedCursorPosition.z - GridTileSize;

			gridedCursorPosition.z = Mathf.Ceil(gridedCursorPosition.z);
			//gridedCursorPosition.z += GridTileSize / 2;

			return gridedCursorPosition;
		}
	}


	public delegate void EnemyEvent(Enemy enemy);

	public event EnemyEvent OnEnemyDestroyed;

	public void NotifyEnemyDestroyed(Enemy enemy)
	{
		if(OnEnemyDestroyed != null)
		{
			OnEnemyDestroyed(enemy);
		}
	}
}
