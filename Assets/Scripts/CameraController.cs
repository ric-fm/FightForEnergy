/*
* Author: Ricardo Franco Martín
*/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CameraController : MonoBehaviour
{

	public float smoothTime = 0.125f;
	public float lookAtSpeed;

	public Transform target;
	Vector3 desiredPosition;

	public bool followTarget = true;
	public bool lookAtTarget = true;

	Vector3 offset = Vector3.zero;

	void Start()
	{
		offset = transform.position - target.position;
	}

	void FixedUpdate()
	{
		if (target != null)
		{
			if (followTarget)
			{

				Vector3 currentVelocity = Vector2.zero;

				Vector3 offsetPosition = target.position + offset;

				desiredPosition = Vector3.SmoothDamp(transform.position, offsetPosition, ref currentVelocity, smoothTime);

				//desiredPosition.y = transform.position.y;



				transform.position = desiredPosition;
			}

			if(lookAtTarget)
			{
				//transform.LookAt(target);

				Quaternion targetRotation = Quaternion.LookRotation(target.position - transform.position);

				// Smoothly rotate towards the target point.
				transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, lookAtSpeed * Time.deltaTime);
			}
		}
	}

	public void Shake(float magnitude, float duration)
	{
		StopAllCoroutines();
		StartCoroutine(PlayShake(magnitude, duration));
	}

	IEnumerator PlayShake(float magnitude, float duration)
	{

		float elapsed = 0.0f;

		//Vector3 originalCamPos = transform.position;

		while (elapsed < duration)
		{

			elapsed += Time.deltaTime;

			float percentComplete = elapsed / duration;
			float damper = 1.0f - Mathf.Clamp(4.0f * percentComplete - 3.0f, 0.0f, 1.0f);

			// map value to [-1, 1]
			float x = Random.value * 2.0f - 1.0f;
			float y = Random.value * 2.0f - 1.0f;
			x *= magnitude * damper;
			y *= magnitude * damper;

			x += desiredPosition.x;
			y += desiredPosition.y;

			transform.position = new Vector3(x, y, desiredPosition.z);

			yield return null;
		}

		transform.position = desiredPosition;
	}
}
