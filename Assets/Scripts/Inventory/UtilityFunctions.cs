using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UtilityFunctions
{
	/// <summary>
	/// Roll for 1: (1-3), 2: (2-5), 3: (3-7)
	/// </summary>
	/// <param name="dieChoice"></param>
	/// <returns></returns>
	public static int RollDice(int dieChoice)
	{
		if (dieChoice == 1)
		{
			return Random.Range(1, 4);
		}
		else if (dieChoice == 2)
		{
			return Random.Range(2, 6);
		}
		else
		{
			return Random.Range(3, 8);
		}
	}
}
