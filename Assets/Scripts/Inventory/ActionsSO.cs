using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Inventory/ActionsSO")]
public class ActionsSO : ScriptableObject
{
	public void UseAction(int optionCode, ItemUI item)
	{
		Debug.Log(item.name + " Used Action");
	}
}
