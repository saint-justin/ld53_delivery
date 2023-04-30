using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "ScriptableObjects/Inventory/ItemSO")]
public class ItemSO : ScriptableObject
{
	public string Name;

	public int ItemID;

	public GroupType GroupType;

	public bool Durable;

	public int Energy;

	public int Heat;

	public int Water;

	public int Ammo;

	public Sprite Icon;

	public ItemUI Prefab;
}
