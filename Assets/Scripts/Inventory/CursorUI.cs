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

	public bool HasItem { get { return _item != null; } }

	private GraphicRaycaster _raycaster;

	private Image _image;

	private ItemUI _item;

	private GameObject _prevHit;


	public void PickItem(ItemUI item)
	{
		_item = item;

		_item.transform.SetParent(transform);

		_item.PickItem();

		_image.enabled = false;
	}


	public void PlaceItem(SlotUI slot)
	{
		if (_item.CheckPlacement())
		{
			_item.transform.position = slot.transform.position;

			_item.PlaceItem();

			_item = null;

			_image.enabled = true;
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
		// Move the item towards the current mouse position
		RectTransformUtility.ScreenPointToLocalPointInRectangle(_canvas.transform as RectTransform, Input.mousePosition, _canvas.worldCamera, out Vector2 position);

		



		PointerEventData ptrEventData = new PointerEventData(_eventSystem);
		ptrEventData.position = Input.mousePosition;

		List<RaycastResult> results = new List<RaycastResult>();

		_raycaster.Raycast(ptrEventData, results);

		if (results.Count > 0 && results[0].gameObject != null)
		{
			if (_gridSnap)
			{
				transform.position = Vector3.Lerp(transform.position, results[0].gameObject.transform.position, 1f - _smoothing);
			}
			else
			{
				transform.localPosition = Vector3.Lerp(transform.localPosition, position, 1f - _smoothing);
			}

			//if (results[0].gameObject != _prevHit)
			//{
			//	_prevHit = results[0].gameObject;

			//	if (_item != null && !_item.CheckPlacement())
			//	{
			//		_item.SetColor(Color.red);
			//	}
			//}
		}
		else
		{
			transform.localPosition = Vector3.Lerp(transform.localPosition, position, 1f - _smoothing);

			//if (_prevHit != null)
			//{
			//	_prevHit = null;

			//	if (_item != null)
			//	{
			//		_item.SetColor(Color.white);
			//	}
			//}
		}


		if (_item != null)
		{
			if (!_item.CheckPlacement())
			{
				_item.SetColor(Color.red);
			}
			else
			{
				_item.SetColor(Color.white);
			}
			
		}
	}
}
