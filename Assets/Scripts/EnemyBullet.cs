/*
* Author: Ricardo Franco Martín
*/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class EnemyBullet : MonoBehaviour
{
	public float speed;
	Vector3 direction;

	Rigidbody rb;

	public int damage = 1;

	public AudioClip PlayerHitSound;
	public AudioClip MachineHitSound;

	void Awake()
	{
		rb = GetComponent<Rigidbody>();
	}

	public void Shoot(Vector3 direction, float speed)
	{
		this.direction = direction;
		this.speed = speed;

		rb.velocity = -direction * speed * Time.deltaTime;
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Machine")
		{
			StealEnergy(collision.gameObject);
		}

		Destroy(gameObject);
	}

	void StealEnergy(GameObject target)
	{
		Energy energy = target.GetComponent<Energy>();

		if (energy.CanSteal)
		{
			if (target.tag == "Player")
			{
				SoundManager.Instance.PlaySingleAtLocation(PlayerHitSound, target.transform.position);
			}
			else
			{
				SoundManager.Instance.PlaySingleAtLocation(MachineHitSound, target.transform.position);
			}

			energy.Steal(damage, 1);
		}
	}
}
