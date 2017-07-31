/*
* Author: Ricardo Franco Martín
*/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SoundGameObject : MonoBehaviour
{
	public AudioSource source;

	void Awake()
	{
		source = GetComponent<AudioSource>();
	}

	public void Play(AudioClip clip, float volume)
	{
		source.clip = clip;

		source.volume = volume;

		source.Play();

		Destroy(gameObject, source.clip.length);
	}

	public void Play(AudioClip clip)
	{
		source.clip = clip;

		source.Play();

		Destroy(gameObject, source.clip.length);
	}
}
