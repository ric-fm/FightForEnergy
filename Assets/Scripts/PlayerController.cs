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

	Rigidbody rb;
	Animator anim;

	public PlayerStats DefaultStats;

	public PlayerStats CurrentStats;

	public GameObject baitMachineTemplate;
	public GameObject turretMachineTemplate;

	AudioSource audioSource;
	public AudioClip ChargeSound;
	public AudioClip StealEnergySound;


	private void Awake()
	{
		rb = GetComponent<Rigidbody>();
		anim = GetComponent<Animator>();

		audioSource = GetComponent<AudioSource>();
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
				spawnMachine.machineTemplate = baitMachineTemplate;

				CurrentState = PlayerState.SPAWNING;

				statsUpdated = false;
				break;

			case BaseMachineUI.UpgradeType.TURRET:
				spawnMachine.machineTemplate = turretMachineTemplate;

				CurrentState = PlayerState.SPAWNING;


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
		SoundManager.Instance.PlaySingleAtLocation(UpgradeSound, Camera.main.transform.position);

		energy.MaxAmount = (int)CurrentStats.ChargeCapacity;
		UpdateEnergyUI();
	}

	void EnergyChanged(Energy energy)
	{
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

						SoundManager.Instance.PlaySingleAtLocation(PlugEnemySound, targetHitEnergy.transform.position);

						audioSource.clip = StealEnergySound;
						audioSource.Play();
						break;

					case "Machine":
						targetHitEnergy.GetComponent<Machine>().OnMachineEnergyFilled += OnMachineEnergyFilled;

						SoundManager.Instance.PlaySingleAtLocation(PlugMachineSound, targetHitEnergy.transform.position);

						audioSource.clip = ChargeSound;
						audioSource.Play();

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

	bool lastUpgradeSelect = false;
	public AudioClip SelectSound;
	public AudioClip DeselectSound;

	public AudioClip ShootSound;
	public AudioClip PlugEnemySound;
	public AudioClip PlugMachineSound;

	public AudioClip UpgradeSound;

	void Aim()
	{
		// Apuntado
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		Vector3 point = Vector3.zero;

		RaycastHit hit;

		if (Physics.Raycast(ray, out hit, 10000, 1 << LayerMask.NameToLayer("3DUI")))
		{
			aimToGround = false;

			BaseMachineUI upgrade = hit.collider.gameObject.GetComponent<BaseMachineUI>();

			bool showValue = upgrade.type != BaseMachineUI.UpgradeType.BAIT && upgrade.type != BaseMachineUI.UpgradeType.TURRET;

			UIManager.Instance.ShowUpgrade(upgrade.text, showValue, upgrade.Value, upgrade.Cost);

			if (!lastUpgradeSelect)
			{
				SoundManager.Instance.PlaySingleAtLocation(SelectSound, Camera.main.transform.position);
				lastUpgradeSelect = true;
			}
		}
		else
		{
			if (lastUpgradeSelect)
			{
				SoundManager.Instance.PlaySingleAtLocation(DeselectSound, Camera.main.transform.position);

				lastUpgradeSelect = false;
			}
			UIManager.Instance.HideUpgrade();

			if (Physics.Raycast(ray, out hit, 10000, 1 << LayerMask.NameToLayer("Machine")))
			{
				Machine machine = hit.collider.gameObject.GetComponent<Machine>();
				Energy machineEnergy = hit.collider.gameObject.GetComponent<Energy>();

				UIManager.Instance.ShowMachineEnergyInfo(machine, machineEnergy);
			}
			else
			{
				UIManager.Instance.HideMachineEnergyInfo();
			}


			if (Physics.Raycast(ray, out hit, 10000, 1 << LayerMask.NameToLayer("Ground")))
			{
				aimToGround = true;
				point = hit.point;

				point.y = transform.position.y;
				Vector3 direction = (transform.position - point).normalized;

				transform.forward = -direction;
			}
		}
	}

	void HandleShoot(bool shootButton)
	{
		if (!aimToGround)
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
		//Vector3 movement = (Vector3.forward * hMove + -Vector3.right * vMove) * speed * Time.deltaTime;
		Vector3 movement = (Vector3.forward * hMove + -Vector3.right * vMove);

		//transform.position += movement;

		rb.velocity = movement;
		rb.velocity = movement.normalized * speed * Time.deltaTime;
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
			//int stealedEnergy = targetHitEnergy.Steal(1);
			int stealedEnergy = targetHitEnergy.Steal(1, CurrentStats.StealSpeed);

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
				int sentEnergy = targetHitEnergy.Receive((int)CurrentStats.ChargeMultiplier);

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
			//if(!backing)
			//{ }
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
		canShoot = false;



		anim.SetTrigger("Shoot");

		yield return new WaitForSeconds(0.2f);
		shooting = true;

		SoundManager.Instance.PlaySingleAtLocation(ShootSound, /*Camera.main.*/transform.position);

		shootDirection = direction;
		shootTargetPoint = handPoint.transform.position + shootDirection * CurrentStats.Range * Time.deltaTime;
		shootTargetPoint.y = handPoint.transform.position.y;

		shootGO.transform.SetParent(null);
		shootGO.transform.position = handPoint.transform.position;
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
		audioSource.Stop();
	}

	void OnMachineSpawned(Machine machine)
	{
		CurrentState = PlayerState.FIGHTING;
	}


}
