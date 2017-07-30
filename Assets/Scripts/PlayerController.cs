/*
* Author: Ricardo Franco Mart�n
*/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	public enum PlayerState
	{
		FIGHTING,
		SPAWNING
	}

	public PlayerState CurrentState;

	public float speed;
	public int minimumEnergyAmount;

	Energy energy;
	public HandController handController;

	public GameObject shootGO;
	public GameObject handPoint;
	public GameObject hand;
	Transform handParentTransform;
	public float shootSpeed;
	public float shootCheckDistance;
	public float shootBackInterval;
	Vector3 shootDirection;
	Vector3 shootInitialPoint;
	Vector3 shootTargetPoint;
	bool canShoot = true;
	bool shoot;

	[SerializeField]
	Energy targetHitEnergy;

	SpawnMachine spawnMachine;

	Animator anim;

	public PlayerStats DefaultStats;

	public PlayerStats CurrentStats;


	private void Awake()
	{
		anim = GetComponent<Animator>();
	}

	// Use this for initialization
	void Start()
	{
		energy = GetComponent<Energy>();
		energy.OnEnergyChanged += EnergyChanged;

		UpdateEnergyUI();

		handController.OnHitTarget += SetTarget;

		spawnMachine = GetComponent<SpawnMachine>();
		spawnMachine.OnMachineSpawned += OnMachineSpawned;

		handParentTransform = hand.transform.parent;

		CurrentStats = DefaultStats;
	}

	public bool CanSpendEnergy(int cost)
	{
		return energy.Amount - minimumEnergyAmount >= cost;
	}

	public void UpgradeStat(BaseMachineUI.UpgradeType upgradeType, float newValue, int cost)
	{
		bool statsUpdated = true;
		switch (upgradeType)
		{
			case BaseMachineUI.UpgradeType.CAPACITY:
				CurrentStats.ChargeCapacity = newValue;
				break;

			case BaseMachineUI.UpgradeType.MULTIPLIER:
				CurrentStats.ChargeMultiplier = newValue;
				break;

			case BaseMachineUI.UpgradeType.RANGE:
				CurrentStats.Range = newValue;
				break;

			case BaseMachineUI.UpgradeType.SPEED:
				CurrentStats.StealSpeed = newValue;
				break;

			case BaseMachineUI.UpgradeType.BAIT:
				Debug.Log("Spawn bait");

				statsUpdated = false;
				break;

			case BaseMachineUI.UpgradeType.TURRET:
				Debug.Log("Spawn turret");

				statsUpdated = false;
				break;
		}
		energy.Amount -= cost;
		

		if (statsUpdated)
		{
			OnStatsUpdated();
		}
	}

	void OnStatsUpdated()
	{
		energy.MaxAmount = (int)CurrentStats.ChargeCapacity;
		UpdateEnergyUI();
		Debug.Log("Stats updated");
	}

	void ShowStats()
	{
		Debug.Log("Stats: " + CurrentStats);


	}

	void EnergyChanged(Energy energy)
	{
		Debug.Log("Energy of player changed");
		UpdateEnergyUI();
	}

	void UpdateEnergyUI()
	{
		UIManager.Instance.SetPlayerEnergy(energy.Amount, energy.MaxAmount);
	}

	void SetTarget(GameObject target)
	{
		if (shooting)
		{
			targetHitEnergy = target.GetComponent<Energy>();

			//handController.transform.SetParent(target.transform);
			//shootGO.transform.SetParent(target.transform);

			if (targetHitEnergy != null)
			{

				switch (targetHitEnergy.gameObject.tag)
				{
					case "Enemy":
						targetHitEnergy.GetComponent<Enemy>().OnEnemyDestroyed += OnEnemyDestroyed;
						break;

					case "Machine":
						targetHitEnergy.GetComponent<Machine>().OnMachineEnergyFilled += OnMachineEnergyFilled;
						break;
				}
			}
		}
	}

	// Update is called once per frame
	void Update()
	{
		// Comprobamos la entrada de usuario
		float hAxis = Input.GetAxis("Horizontal");
		float vAxis = Input.GetAxis("Vertical");
		//bool shootButton = Input.GetButtonDown("Fire1");

		if (Input.GetKeyDown(KeyCode.E))
		{
			CurrentState = PlayerState.SPAWNING;
		}

		Move(hAxis, vAxis);

		Vector2 move = new Vector2(hAxis, vAxis);
		anim.SetFloat("Speed", move.magnitude);

		Aim();

		switch (CurrentState)
		{
			case PlayerState.FIGHTING:
				spawnMachine.enabled = false;

				HandleShoot(Input.GetButtonDown("Fire1"));
				HandleEnergy();

				break;

			case PlayerState.SPAWNING:
				spawnMachine.enabled = true;

				break;
		}

	}

	bool aimToGround;

	#region Player Mechanics
	void Aim()
	{
		// Apuntado
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		Vector3 point = Vector3.zero;

		RaycastHit hit;

		if (Physics.Raycast(ray, out hit, 10000, 1 << LayerMask.NameToLayer("3DUI")))
		{
			aimToGround = false;
		}
		else if (Physics.Raycast(ray, out hit, 10000, 1 << LayerMask.NameToLayer("Ground")))
		{
			aimToGround = true;
			point = hit.point;

			point.y = transform.position.y;
			Vector3 direction = (transform.position - point).normalized;

			transform.forward = -direction;
		}
	}

	void HandleShoot(bool shootButton)
	{
		if(!aimToGround)
		{
			return;
		}
		if (targetHitEnergy == null)
		{
			if (canShoot)
			{
				shoot = shootButton;
			}
		}
		else
		{
			if (shootButton)
			{
				ClearTargetHit();
			}
		}

		if (shoot)
		{
			StartCoroutine(ShootAnimation(transform.forward));
		}
	}

	void Move(float hMove, float vMove)
	{
		Vector3 movement = (Vector3.forward * hMove + -Vector3.right * vMove) * speed * Time.deltaTime;

		transform.position += movement;
	}

	void HandleEnergy()
	{
		if (targetHitEnergy != null)
		{
			switch (targetHitEnergy.gameObject.tag)
			{
				case "Enemy":
					StealEnergyTarget();
					break;

				case "Machine":
					SendEnergyTarget();
					break;
			}
		}
	}

	void StealEnergyTarget()
	{
		if (targetHitEnergy.CanSteal)
		{
			int stealedEnergy = targetHitEnergy.Steal(1);

			int receivedEnergy = energy.Receive(stealedEnergy);
		}
		else
		{
			//Debug.Log("Cant steal energy from " + targetHitEnergy.gameObject.name);
		}
	}

	void SendEnergyTarget()
	{
		if (energy.Amount > minimumEnergyAmount)
		{

			if (targetHitEnergy.CanReceive)
			{
				int sentEnergy = targetHitEnergy.Receive(1);

				energy.Amount -= sentEnergy;
			}
			else
			{
				//Debug.Log("Cant steal energy from " + targetHitEnergy.gameObject.name);

			}
		}
		else
		{
			ClearTargetHit();
		}
	}

	bool shooting;
	private void LateUpdate()
	{
		if (shooting)
		{
			if (targetHitEnergy != null)
			{
				shootGO.transform.position = targetHitEnergy.transform.position;
				hand.transform.localPosition = Vector3.zero;

				Vector3 targetDirection = (hand.transform.position - targetHitEnergy.transform.position).normalized;
				hand.transform.right = -targetDirection;

			}
			else
			{
				hand.transform.localPosition = Vector3.zero;
				hand.transform.right = -shootDirection;
			}
		}
	}

	IEnumerator ShootAnimation(Vector3 direction)
	{
		shoot = false;
		shooting = true;
		canShoot = false;

		shootDirection = direction;
		shootTargetPoint = handPoint.transform.position + shootDirection * CurrentStats.Range * Time.deltaTime;
		shootTargetPoint.y = handPoint.transform.position.y;

		shootGO.transform.SetParent(null);
		hand.transform.SetParent(shootGO.transform);


		while (targetHitEnergy == null)
		{
			shootGO.transform.position = Vector3.Lerp(shootGO.transform.position, shootTargetPoint, shootSpeed * Time.deltaTime);
			//Debug.Log("target " + shootTargetPoint);

			float dist = Vector3.Distance(shootGO.transform.position, shootTargetPoint);

			if (dist <= shootCheckDistance)
			{
				break;
			}
			yield return null;
		}

		do
		{

			yield return new WaitForSeconds(shootBackInterval);
		}
		while (targetHitEnergy != null);

		OnShootAnimationCompleted();
	}

	void OnShootAnimationCompleted()
	{
		StartCoroutine(HandBackAnimation());
	}

	IEnumerator HandBackAnimation()
	{
		shootGO.transform.SetParent(transform);

		while (true)
		{
			shootGO.transform.localPosition = Vector3.Lerp(shootGO.transform.localPosition, Vector3.zero, shootSpeed * Time.deltaTime);

			float dist = Vector3.Distance(shootGO.transform.localPosition, Vector3.zero);

			if (dist <= shootCheckDistance)
			{
				break;
			}
			yield return null;
		}

		shootGO.transform.localPosition = Vector3.zero;

		ClearTargetHit();
		canShoot = true;
		shooting = false;
		hand.transform.SetParent(handParentTransform);
	}
	#endregion


	void OnMachineEnergyFilled(Machine machine)
	{
		ClearTargetHit();
	}

	void OnEnemyDestroyed(Enemy enemy)
	{
		ClearTargetHit();
	}

	void ClearTargetHit()
	{
		targetHitEnergy = null;
	}

	void OnMachineSpawned(Machine machine)
	{
		CurrentState = PlayerState.FIGHTING;
	}


}
