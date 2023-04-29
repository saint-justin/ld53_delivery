using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlockUI : MonoBehaviour
{
	private ItemUI _parentItem;
	public ItemUI GetParentItem { get { return _parentItem; } }


	[SerializeField]
	private Image _raycastTarget;

	void Start()
	{
		_parentItem = GetComponentInParent<ItemUI>();
	}


	public void EnableRayCast(bool enable)
	{
		_raycastTarget.raycastTarget = enable;
	}
}
