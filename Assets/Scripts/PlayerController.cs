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
	public float shootSpeed;
	public float shootRange;
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
		targetHitEnergy = target.GetComponent<Energy>();

		switch(targetHitEnergy.gameObject.tag)
		{
			case "Enemy":
				targetHitEnergy.GetComponent<Enemy>().OnEnemyDestroyed += OnEnemyDestroyed;
				break;

			case "Machine":
				targetHitEnergy.GetComponent<Machine>().OnMachineEnergyFilled += OnMachineEnergyFilled;
				break;
		}
	}

	// Update is called once per frame
	void Update()
	{
		// Comprobamos la entrada de usuario
		float hAxis = Input.GetAxis("Horizontal");
		float vAxis = Input.GetAxis("Vertical");
		//bool shootButton = Input.GetButtonDown("Fire1");

		if(Input.GetKeyDown(KeyCode.E))
		{
			CurrentState = PlayerState.SPAWNING;
		}

		Move(hAxis, vAxis);

		Vector2 speed = new Vector2(hAxis, vAxis);
		anim.SetFloat("Speed", speed.magnitude);

		Aim();

		switch(CurrentState)
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

#region Player Mechanics
	void Aim()
	{
		// Apuntado
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		Vector3 point = Vector3.zero;

		RaycastHit hit;

		if (Physics.Raycast(ray, out hit, 10000, 1 << LayerMask.NameToLayer("Ground")))
		{
			point = hit.point;

			point.y = transform.position.y;
			Vector3 direction = (transform.position - point).normalized;

			transform.right = direction;
		}
	}

	void HandleShoot(bool shootButton)
	{
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

	IEnumerator ShootAnimation(Vector3 direction)
	{
		shoot = false;
		canShoot = false;

		shootDirection = direction;
		shootInitialPoint = transform.position;
		shootTargetPoint = transform.position + -shootDirection * shootRange * Time.deltaTime;

		shootGO.transform.SetParent(null);

		while (targetHitEnergy == null)
		{
			shootGO.transform.position = Vector3.Lerp(shootGO.transform.position, shootTargetPoint, shootSpeed * Time.deltaTime);

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
		shootGO.transform.SetParent(handPoint.transform);

		Debug.Log("back");

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
