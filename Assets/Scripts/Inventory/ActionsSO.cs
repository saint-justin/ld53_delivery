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

		EncounterManager.Instance.DealDamage(item);
	}


	public void MoveSpaces(int optionCode, ItemUI item)
	{
		Debug.Log(item.name + " Move");

		int option = item.ItemSO.AbilityValue;

		int spaces = 0;

		int direction = 0;

		if (option == 1)
		{
			spaces = UtilityFunctions.RollDice(1);
			direction = 0;
		}
		else if (option == 2)
		{
			spaces = UtilityFunctions.RollDice(2);
			direction = 0;
		}
		else if (option == 3)
		{
			spaces = UtilityFunctions.RollDice(1);
			direction = 1;
		}
		else if (option == 4)
		{
			spaces = UtilityFunctions.RollDice(1);
			direction = 3;
		}

		InventoryUI.Instance.MoveSpaces(direction, spaces);


		EncounterManager.Instance.MoveSpaces(item);
	}


	public void ApplyShield(int optionCode, ItemUI item)
	{
		Debug.Log(item.name + " UseShield");

		InventoryUI.Instance.GenerateShield(item.ItemSO.AbilityValue);

		EncounterManager.Instance.ApplyShield(item);

	}


	public void GenerateEnergy(int optionCode, ItemUI item)
	{
		EncounterManager.Instance.GenerateEnergy(item);
	}


	public void HeatSink(int optionCode, ItemUI item)
	{
		EncounterManager.Instance.HeatSink(item);
	}
}
