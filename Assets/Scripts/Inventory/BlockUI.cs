using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlockUI : MonoBehaviour
{
	private ItemUI _parentItem;
	public ItemUI GetParentItem { get { return _parentItem; } }

	[SerializeField]
	private Image _damageIcon;

	public Transform _raycastDebug;


	[SerializeField]
	private Image _raycastTarget;

	private bool _isDamaged;
	public bool IsDamaged { get { return _isDamaged; } }


	void Start()
	{
		_parentItem = GetComponentInParent<ItemUI>();

		SetDamaged(false);
	}


	public void EnableRayCast(bool enable)
	{
		_raycastTarget.raycastTarget = enable;
	}


	public void SetDamaged(bool damaged)
	{
		_isDamaged = damaged;

		_damageIcon.enabled = damaged;

		if (damaged != _parentItem.IsDamaged)
		{
			_parentItem.CheckDamage();
		}
	}
}
