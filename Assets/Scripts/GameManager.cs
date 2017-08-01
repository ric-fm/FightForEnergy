/*
* Author: Ricardo Franco Martín
*/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

		StartTimer();
	}

	public bool timerOn = false;
	public float timer;

	private void Update()
	{
		if (timerOn == true)
		{
			timer += Time.deltaTime;
		}

		if(Input.GetKeyDown(KeyCode.Escape))
		{
			Application.Quit();
		}
	}

	public void StartTimer()
	{
		timer = 0.0f;
		timerOn = true;
	}

	public void PauseTimer()
	{
		timerOn = false;
	}

	public void ResumeTimer()
	{
		timerOn = true;
	}

	public void ResetTimer()
	{
		timerOn = false;
		timer = 0.0f;
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

	public int EnemiesDestroyedCount = 0;

	public void NotifyEnemyDestroyed(Enemy enemy)
	{
		++EnemiesDestroyedCount;
		if(OnEnemyDestroyed != null)
		{
			OnEnemyDestroyed(enemy);
		}
	}

	public AudioClip GameOverSound;

	public void GameOver()
	{
		SoundManager.Instance.PlaySingleAtLocation(GameOverSound, playerController.transform.position);
		PauseTimer();

		UIManager.Instance.ShowGameOver();
		Time.timeScale = 0.0f;
	}

	public void Restart()
	{
		Time.timeScale = 1.0f;
		//SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		SceneManager.LoadScene("mainmenu_scene");
	}

}
