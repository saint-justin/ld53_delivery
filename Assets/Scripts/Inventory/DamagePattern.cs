using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DamagePattern : MonoBehaviour
{
	private BlockUI[] _blocks;

	private Image _icon;

	private Canvas _canvas;

	private GraphicRaycaster _raycaster;

	private EventSystem _eventSystem;

	private PointerEventData _ptrEventData;

	private bool _validLocation;

	Vector3 _raycastPosition;

	private int _maxAttempts;

	public void Initialize(GraphicRaycaster raycaster, Canvas canvas, int maxAttempts, bool visible)
	{
		if (raycaster == null || canvas == null)
		{
			Debug.Log("Missing something");
		}

		ItemUI item = GetComponent<ItemUI>();

		_blocks = item.Blocks;

		_icon = item.Icon;

		for (int i = 0; i < _blocks.Length; i++)
		{
			_blocks[i].SetupAsDamage(visible);
		}

		_icon.enabled = false;

		_raycaster = raycaster;

		_eventSystem = EventSystem.current;

		_ptrEventData = new PointerEventData(_eventSystem);

		_canvas = canvas;

		_validLocation = false;

		_maxAttempts = maxAttempts;
	}


	public void ApplyDamage(bool damage)
	{
		if (!_validLocation)
		{
			return;
		}

		if (_eventSystem == null)
		{
			_eventSystem = EventSystem.current;

			_ptrEventData = new PointerEventData(_eventSystem);
		}


		for (int i = 0; i < _blocks.Length; i++)
		{
			List<RaycastResult> hits = new List<RaycastResult>();

			_ptrEventData.position = _blocks[i].transform.position;

			_raycaster.Raycast(_ptrEventData, hits);

			for (int hit = 0; hit < hits.Count; hit++)
			{
				if (hits[hit].gameObject != null && hits[hit].gameObject.CompareTag("Shield"))
				{
					break;
				}


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


	public bool CheckPlacement(bool avoidDamaged, bool asHeat)
	{
		if (_eventSystem == null)
		{
			Debug.LogError("Replaced EventSystem");
			_eventSystem = EventSystem.current;

			_ptrEventData = new PointerEventData(_eventSystem);
		}

		for (int i = 0; i < _blocks.Length; i++)
		{
			List<RaycastResult> hits = new List<RaycastResult>();

			_ptrEventData.position = _blocks[i].transform.position;

			//Debug.Log($"{gameObject.name} Block {i}: {_blocks[i].transform.position}");

			_raycaster.Raycast(_ptrEventData, hits);

			SlotUI slot = null;
			BlockUI block = null;

			if (hits.Count > 0)
			{
				//Debug.Log(hits.Count);
			}

			for (int hit = 0; hit < hits.Count; hit++)
			{
				if (hits[hit].gameObject != null)
				{
					//Debug.LogError($"Hit {hits[hit].gameObject.name} at {hits[hit].gameObject.transform.position}");
				}

				if (hits[hit].gameObject != null && hits[hit].gameObject.CompareTag("Slot"))
				{
					slot = hits[hit].gameObject.GetComponent<SlotUI>();
				}

				if (hits[hit].gameObject != null && hits[hit].gameObject.CompareTag("Item"))
				{
					block = hits[hit].gameObject.GetComponent<BlockUI>();
				}
			}

			if (block != null)
			{
				Debug.Log(block.transform.parent.gameObject.name + " " + block.IsDamagePattern);
			}

			if (block != null && block.IsDamagePattern)
			{
				return false;
			}


			if (slot == null || (avoidDamaged && slot.IsDamaged) || (asHeat && slot.GroupType == GroupType.Hat))
			{
				return false;
			}
		}


		return true;
	}


	public bool FindValidSpot(Vector3 center, bool avoidDamaged, bool asHeat)
	{
		EnableRaycastTarget(false);

		_validLocation = false;

		int tries = 0;

		for (int i = 0; i < _maxAttempts; i++)
		{
			tries++;

			int x = UnityEngine.Random.Range(-5, 5);
			int y = UnityEngine.Random.Range(-5, 7);

			_raycastPosition.x = (x * 30 + 15) * _canvas.scaleFactor;

			_raycastPosition.y = y * 30 * _canvas.scaleFactor;

			transform.position = _raycastPosition + center;

			if (CheckPlacement(avoidDamaged, asHeat))
			{
				Debug.Log($"Placed Damage in {tries} attempts");

				_validLocation = true;
				break;
			}
		}


		if (!_validLocation)
		{
			for (int i = 0; i < 120; i++)
			{
				tries++;

				int x = (i % 10) - 5;

				int y = (i / 10) - 5;

				_raycastPosition.x = (x * 30 + 15) * _canvas.scaleFactor;

				_raycastPosition.y = y * 30 * _canvas.scaleFactor;

				transform.position = _raycastPosition + center;

				if (CheckPlacement(avoidDamaged, asHeat))
				{
					Debug.Log($"Placed Damage in {tries} attempts");

					_validLocation = true;
					break;
				}
			}
		}
		


		gameObject.SetActive(_validLocation);
		EnableRaycastTarget(true);

		return _validLocation;
	}


	public void EnableRaycastTarget(bool enable)
	{
		for (int i = 0; i < _blocks.Length; i++)
		{
			_blocks[i].EnableRayCast(enable);
		}
	}


	public void SetVisible(bool visible)
	{
		foreach(BlockUI block in _blocks)
		{
			block.SetWarningVisible(visible);
		}
	}


	public void SetPosition(Vector2 position, Vector2 center)
	{
		_validLocation = true;

		transform.position = position * _canvas.scaleFactor + center;
	}
}
