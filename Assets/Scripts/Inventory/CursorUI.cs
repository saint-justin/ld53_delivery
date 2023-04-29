using System.Collections;
using System.Collections.Generic;
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

	public bool HasItem { get { return _item != null; } }

	private GraphicRaycaster _raycaster;

	private Image _image;

	private ItemUI _item;

	private GameObject _prevHit;
	private GameObject _currentHit;

	private bool flipflop;


	public void PickItem(ItemUI item)
	{
		_item = item;

		_item.transform.SetParent(transform);

		_item.PickItem();

		_image.enabled = false;

		Debug.Log("Pick Item");
	}


	public void PlaceItem(SlotUI slot)
	{
		if (_item.CheckPlacement())
		{
			_item.PlaceItem();

			_item = null;

			_image.enabled = true;
		}

		Debug.Log("Place Item");
	}


	public void DeleteItem()
	{
		if (_item != null)
		{
			Destroy(_item.gameObject);

			_item = null;
		}
	}


	private void Start()
	{
		_raycaster = _canvas.GetComponent<GraphicRaycaster>();
		_image = GetComponent<Image>();

		Cursor.visible = false;

		RectTransformUtility.ScreenPointToLocalPointInRectangle(_canvas.transform as RectTransform, Input.mousePosition, _canvas.worldCamera, out Vector2 position);
		transform.localPosition = position;
	}


	private void Update()
	{
		
		PointerEventData ptrEventData = new PointerEventData(_eventSystem);
		ptrEventData.position = Input.mousePosition;

		List<RaycastResult> results = new List<RaycastResult>();

		_raycaster.Raycast(ptrEventData, results);

		if (results.Count > 0)
		{
			_currentHit = results[0].gameObject;
		}
		else
		{
			_currentHit = null;
		}


		if (_currentHit != null && _gridSnap)
		{
			transform.position = Vector3.Lerp(transform.position, results[0].gameObject.transform.position, 1f - _smoothing);
		}
		else
		{
			// Move the item towards the current mouse position
			RectTransformUtility.ScreenPointToLocalPointInRectangle(_canvas.transform as RectTransform, Input.mousePosition, _canvas.worldCamera, out Vector2 position);
			transform.localPosition = Vector3.Lerp(transform.localPosition, position, 1f - _smoothing);
		}


		if (_currentHit != _prevHit)
		{
			_prevHit = _currentHit;

			if (_item != null)
			{
				if (_item.CheckPlacement())
				{
					_item.SetColor(_validColor);
				}
				else
				{
					_item.SetColor(_invalidColor);
				}
			}
		}
	}
}
