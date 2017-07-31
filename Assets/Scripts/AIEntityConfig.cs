/*
* Author: Ricardo Franco Martín
*/

using RAIN.Entities;
using RAIN.Entities.Aspects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class AIEntityConfig : MonoBehaviour
{

	public EntityRig entity;

	void Start()
	{
		if (entity != null)
		{
			entity.Entity.Form = gameObject;


			foreach (RAINAspect aspect in entity.Entity.Aspects)
			{
				aspect.MountPoint = transform;
			}
			
		}
		else
		{
			Debug.Log("Entity not established in " + name);
		}
	}
}
