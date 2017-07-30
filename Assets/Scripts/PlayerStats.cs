/*
* Author: Ricardo Franco Martín
*/

using System;

[Serializable]
public class PlayerStats
{
	public enum StatType
	{
		CAPACITY,
		MULTIPLIER,
		SPEED,
		RANGE
	}

	public float ChargeCapacity;

	public float ChargeMultiplier;

	public float StealSpeed;

	public float Range;
}
