using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CursorUI : MonoBehaviour
{
	[SerializeField]
	private Canvas _canvas;

	[SerializeField]
	private EventSystem _eventSystem;

	[SerializeField]
	private bool _gridSnap;

	[SerializeField][Range(0f, 0.99f)]
	private float _smoothing;

	[SerializeField]
	private Color _validColor;

	[SerializeField]
	private Color _invalidColor;

	[SerializeField]
	private Image _toolTip;

	[SerializeField]
	private TextMeshProUGUI _toolTipName;

	[SerializeField]
	private TextMeshProUGUI _toolTipDescription;


	public bool HasItem { get { return _item != null; } }

	private GraphicRaycaster _raycaster;

	private PointerEventData _ptrEventData;

	[SerializeField]
	private Image _icon;

	private ItemUI _item;
	public ItemUI CurrentItem { get { return _item; } }

	private GameObject _prevHit;
	private GameObject _currentHit;

	private bool _wasValid;


	public void PickItem(ItemUI item)
	{
		_item = item;

		_item.transform.position = transform.position;

		_item.PickItem();

		_icon.enabled = false;

		//Debug.Log("Pick Item");

		_wasValid = false;
		_item.SetColor(_invalidColor);

		SetToolTip(null);

		_prevHit = null;
	}


	public bool PlaceItem(SlotUI slot)
	{
		if (_item.CheckPlacement(out bool onGrid, out Vector3 snapPosition))
		{
			_item.PlaceItem(snapPosition);

			_item = null;

			_icon.enabled = true;

			return true;
		}

		return false;
	}


	public void DeleteItem()
	{
		if (_item != null)
		{
			Destroy(_item.gameObject);

			_item = null;

			_icon.enabled = true;
		}
	}


	private void SetToolTip(ItemUI itemUI)
	{
		if (itemUI == null)
		{
			_toolTip.gameObject.SetActive(false);
			return;
		}


		_toolTip.gameObject.SetActive(true);

		_toolTipName.text = itemUI.ItemSO.Name + ":";

		_toolTipDescription.text = itemUI.ItemSO.Description;

	}


	private void Start()
	{
		_raycaster = _canvas.GetComponent<GraphicRaycaster>();

		_ptrEventData = new PointerEventData(_eventSystem);

		//Cursor.visible = false;

		RectTransformUtility.ScreenPointToLocalPointInRectangle(_canvas.transform as RectTransform, Input.mousePosition, _canvas.worldCamera, out Vector2 position);
		transform.localPosition = position;
	}


	private void FixedUpdate()
	{
		// Move the item towards the current mouse position
		RectTransformUtility.ScreenPointToLocalPointInRectangle(_canvas.transform as RectTransform, Input.mousePosition, _canvas.worldCamera, out Vector2 position);
		transform.localPosition = Vector3.Lerp(transform.localPosition, position, 1f - _smoothing);

		if (_item != null)
		{
			// Need to temporarilly move the item to the cursor location to perform the raycasts
			Vector3 prevPosition = _item.transform.position;
			_item.transform.position = transform.position;

			bool isValid = _item.CheckPlacement(out bool onGrid, out Vector3 snapPostition);

			if (isValid != _wasValid)
			{
				//Debug.Log(snapPostition);

				_wasValid = isValid;

				if (isValid)
				{
					_item.SetColor(_validColor);
				}
				else
				{
					_item.SetColor(_invalidColor);
				}
			}


			if (onGrid && _gridSnap)
			{
				_item.transform.position = Vector3.Lerp(prevPosition, snapPostition, 1f - _smoothing);
				_item.SetRaycastDebug(snapPostition);
			}
		}
		else
		{
			if (_eventSystem == null)
			{
				_ptrEventData = new PointerEventData(EventSystem.current);
			}

			_ptrEventData.position = transform.position;

			List<RaycastResult> results = new List<RaycastResult>();

			_raycaster.Raycast(_ptrEventData, results);

			if (results.Count > 0)
			{
				_currentHit = results[0].gameObject;
			}

			if (_currentHit != _prevHit)
			{
				_prevHit = _currentHit;

				//Debug.Log("Hit Something New");

				if (_currentHit != null)
				{
					BlockUI block = _currentHit.GetComponent<BlockUI>();

					if (block != null)
					{
						SetToolTip(block.GetParentItem);
					}
					else
					{
						SetToolTip(null);
					}
				}
				else
				{
					SetToolTip(null);
				}
			}
		}
	}
}
