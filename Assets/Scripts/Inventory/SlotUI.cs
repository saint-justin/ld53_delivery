using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotUI : MonoBehaviour
{
	public bool HasItem { get { return _item != null; } }

	private ItemUI _item;

	public void PlaceItem(ItemUI item)
	{
		_item = item;

		Debug.Log("Placed item in slot");
	}

	public void PickItem()
	{
		_item = null;

		Debug.Log("Removed item in slot");
	}

}
