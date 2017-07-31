/*
* Author: Ricardo Franco Martín
*/


using RAIN.Core;
using RAIN.Perception.Sensors;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class AIConfig : MonoBehaviour
{
	public AIRig ai;

	void Start () {
		if(ai != null)
		{
			ai.AI.Body = gameObject;

			RAINSensor sightSensor = ai.AI.Senses.GetSensor("Sight");
			sightSensor.MountPoint = transform;
		}
		else
		{
			Debug.Log("AI not established in " + name);
		}
	}
}
