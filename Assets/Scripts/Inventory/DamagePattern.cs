using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DamagePattern : MonoBehaviour
{
	private BlockUI[] _blocks;

	private Image _icon;

	private GraphicRaycaster _raycaster;

	private EventSystem _eventSystem;

	private PointerEventData _ptrEventData;



	public void Initialize(GraphicRaycaster raycaster)
	{
		ItemUI item = GetComponent<ItemUI>();

		_blocks = item.Blocks;

		_icon = item.Icon;

		for (int i = 0; i < _blocks.Length; i++)
		{
			_blocks[i].SetupAsDamage();
		}

		_icon.enabled = false;

		_raycaster = raycaster;

		_eventSystem = EventSystem.current;

		_ptrEventData = new PointerEventData(_eventSystem);
	}


	public void ApplyDamage(bool damage)
	{
		if (_eventSystem == null)
		{
			_eventSystem = EventSystem.current;
		}


		for (int i = 0; i < _blocks.Length; i++)
		{
			List<RaycastResult> hits = new List<RaycastResult>();

			_ptrEventData.position = _blocks[i].transform.position;

			_raycaster.Raycast(_ptrEventData, hits);

			for (int hit = 0; hit < hits.Count; hit++)
			{
				if (hits[hit].gameObject != null && hits[hit].gameObject.CompareTag("Slot"))
				{
					SlotUI slot = hits[hit].gameObject.GetComponent<SlotUI>();

					if (slot != null)
					{
						slot.SetDamaged(damage);
					}
				}
			}
		}
	}
}
