using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "ScriptableObjects/Inventory/ItemSO")]
public class ItemSO : ScriptableObject
{
	public string Name;

	public string Description;

	public int ItemID;

	public GroupType GroupType;

	public bool Durable;

	public int Energy;

	public int Heat;

	public int Water;

	public int Ammo;

	public int Value;

	public int AbilityValue;

	public Sprite Icon;

	public ItemUI Prefab;

	public UnityEvent<int, ItemUI> ActionEvent;
}
