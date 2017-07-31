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
		Debug.Log("collision with " + collision.gameObject.tag);
		if (collision.gameObject.tag == "Player")
		{
			Debug.Log("Hit to player");
		}

		Destroy(gameObject);
	}
}
