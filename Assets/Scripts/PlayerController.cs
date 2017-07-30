/*
* Author: Ricardo Franco Mart�n
*/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	public float speed;
	public int MaxEnergy;

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
	bool isTargetHit;

	[SerializeField]
	GameObject targetHit;

	[SerializeField]
	Energy targetHitEnergy;

	// Use this for initialization
	void Start()
	{
		energy = GetComponent<Energy>();
		energy.OnEnergyChanged += EnergyChanged;

		UpdateEnergyUI();

		handController.OnHitTarget += HitTarget;
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

	void HitTarget(GameObject target)
	{
		targetHit = target;
		targetHitEnergy = target.GetComponent<Energy>();
		isTargetHit = true;
	}

	// Update is called once per frame
	void Update()
	{

		float hAxis = Input.GetAxis("Horizontal");
		float vAxis = Input.GetAxis("Vertical");

		bool shootButton = Input.GetButtonDown("Fire1");

		if (!isTargetHit)
		{
			if (canShoot)
			{
				shoot = shootButton;
			}
		}
		else
		{
			if(shoot)
			{
				isTargetHit = false;
			}
		}

		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		Vector3 point = Vector3.zero;


		RaycastHit hit;

		if (Physics.Raycast(ray, out hit, 10000, 1 << LayerMask.NameToLayer("Ground")))
		{
			point = hit.point;

			point.y = transform.position.y;
			Vector3 direction = (transform.position - point).normalized;

			transform.forward = direction;
		}

		//point.y = transform.position.y;
		//Vector3 direction = (transform.position - point).normalized;

		//transform.forward = direction;

		Vector3 movement = (Vector3.forward * hAxis + -Vector3.right * vAxis) * speed * Time.deltaTime;

		transform.position += movement;

		if (shoot)
		{
			StartCoroutine(Shoot(transform.forward));
		}

		if (targetHitEnergy != null)
		{
			if (targetHitEnergy.CanSteal)
			{
				int stealedEnergy = targetHitEnergy.Steal(1);
				Debug.Log("Stealing " + stealedEnergy + " energy from " + targetHitEnergy.gameObject.name);

				int receivedEnergy = energy.Receive(stealedEnergy);

				Debug.Log("Receiving " + receivedEnergy);
			}
			else
			{
				//Debug.Log("Cant steal energy from " + targetHitEnergy.gameObject.name);
			}
		}
		else
		{
			isTargetHit = false;
		}
	}

	IEnumerator Shoot(Vector3 direction)
	{
		shoot = false;
		canShoot = false;

		shootDirection = direction;
		shootInitialPoint = transform.position;
		shootTargetPoint = transform.position + -shootDirection * shootRange * Time.deltaTime;

		shootGO.transform.SetParent(null);

		while (!isTargetHit)
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
			Debug.Log("waiting");

			yield return new WaitForSeconds(shootBackInterval);
		}
		while (isTargetHit);

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

		isTargetHit = false;
		canShoot = true;
	}
}
