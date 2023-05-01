using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Inventory/ActionsSO")]
public class ActionsSO : ScriptableObject
{
	public void UseAction(int optionCode, ItemUI item)
	{
		Debug.Log(item.name + " Used Action " + item.ItemSO.AbilityValue);
	}


	public void DealDamage(int optionCode, ItemUI item)
	{
		Debug.Log(item.name + " DealDamage");
	}


	public void Move(int optionCode, ItemUI item)
	{
		Debug.Log(item.name + " Move");
	}


	public void UseShield(int optionCode, ItemUI item)
	{
		Debug.Log(item.name + " UseShield");
	}
}
