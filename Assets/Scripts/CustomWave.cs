/*
* Author: Ricardo Franco Martín
*/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CustomWave
{
	public List<CustomSpawn> spaws;
	public float delay;
}

[System.Serializable]
public class CustomSpawn
{
	public GameObject spawnTemplate;
	public int rate;
	public float delay;
}
